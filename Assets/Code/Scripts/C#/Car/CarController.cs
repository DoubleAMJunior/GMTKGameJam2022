using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class CarController : MonoBehaviour, ICarHitResponse
{
	[SerializeField] private CarData data;
	float dragValue, angularDrag, friction, forwardMovement;
    [SerializeField] protected CarInterface carInterface;
	private Rigidbody2D rb;
	private float rotationAngle = 0f;
	private float velocityVsUp = 0f;

	protected PlayerRankData rankData;

	protected IEnumerator skidCoroutine;
	protected IEnumerator engineCoroutine;
	protected bool bEngineActive = true, bKillSideVelocity = true;
	
	private SpriteRenderer spriteRenderer;
	private TrailRenderer trailRenderer;
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		rankData = GetComponent<PlayerRankData>();
		trailRenderer = GetComponent<TrailRenderer>();
		trailRenderer.emitting = false;
		dragValue = data.dragValue;
		angularDrag = rb.angularDrag;
		forwardMovement = data.accelerationFactor;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	protected virtual void FixedUpdate()
	{
		////AssignSprite();
		if (bEngineActive)
			ApplyForwardForce(carInterface.accelerationInput);
		if (bKillSideVelocity)
			KillSideVelocity();
		ApplySteering(carInterface.steeringInput);
	}

	protected void ApplyForwardForce(float force)
	{
		//apply drag when there is no accelerationInput
		if (force <= 0.5f && force >= -0.5f)
		{
			rb.drag = Mathf.Lerp(0, dragValue, Time.fixedDeltaTime * data.dragTime);
		}
		else
		{
			rb.drag = 0;
		}

		//limit max forward speed and reverse speed
		velocityVsUp = Vector2.Dot(transform.up, rb.velocity);
		if (velocityVsUp > data.maxSpeed && force > 0)
		{
			return;
		}

		if (velocityVsUp > data.maxSpeed && force < 0) //max speed
		{
			return;
		}

		if (velocityVsUp < -data.maxSpeed*0.5f && force < 0) //reverse speed
		{
			return;
		}

		if (rb.velocity.sqrMagnitude > data.maxSpeed * data.maxSpeed && force > 0) //cannot go faster in any direction while accelerating
		{
			return;
		}

		//move car forward
		Vector2 engineForceVector = transform.up * force * forwardMovement;
		rb.AddForce(engineForceVector, ForceMode2D.Force);
	}

	protected void ApplySteering(float force)
	{
		//prevent car from turning while stopped
        float minSpeedBeforeAllowTurningFactor = (rb.velocity.magnitude / 2);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

		int dir = velocityVsUp > 0 ? 1: -1;
		rotationAngle -= force * data.turnFactor * minSpeedBeforeAllowTurningFactor * dir;
		rb.MoveRotation(rotationAngle);
	}

	protected void KillSideVelocity()
	{
		//pivoting for controlled turns
		Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);
		rb.velocity = forwardVelocity + rightVelocity * data.driftFactor;
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
		var obstacle = other.gameObject.GetComponent<IObstacle>();
		if (obstacle != null)
			obstacle.OnHit(this);
	}

    public void SlowDown(int percent)
    {
		int reverseP = 100 - percent;
        rb.velocity *= reverseP/100;
    }

	public void SpeedIncrease(float amount)
    {
		rb.velocity *= amount;
    }

    public void DisableEngine(float time)
    {
		if (engineCoroutine != null)
			StopCoroutine(engineCoroutine);
		engineCoroutine = EngineTime(time);
		StartCoroutine(engineCoroutine);
    }

    public void SkidOut(float time)
    {
		if (skidCoroutine != null)
			StopCoroutine(skidCoroutine);
		skidCoroutine = SkidTime(time);
		StartCoroutine(skidCoroutine);
	}

	IEnumerator SkidTime(float time)
	{
		dragValue = 5;
		rb.angularDrag = 0;
		bKillSideVelocity = false;
		rb.sharedMaterial = data.skidPhysics;
		forwardMovement = 1;
		trailRenderer.emitting = true;
		yield return new WaitForSeconds(time);
		dragValue = data.dragValue;
		rb.angularDrag = angularDrag;
		bKillSideVelocity = true;
		rb.sharedMaterial = data.normalPhysics;
		forwardMovement = data.accelerationFactor;
		trailRenderer.emitting = false;
	}
	IEnumerator EngineTime(float time)
	{
		bEngineActive = false;
		spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);//data.wrechedCar;
		yield return new WaitForSeconds(time);
		bEngineActive = true;
		spriteRenderer.color = new Color(1, 1, 1, 1);//data.wrechedCar;
	}

	void AssignSprite()
    {
		transform.GetChild(0).rotation = Quaternion.identity;

		if (rb.velocity.x > 0 && rb.velocity.y > 0)
			spriteRenderer.sprite = data.carSprites[2];
		else if (rb.velocity.x < 0 && rb.velocity.y > 0)
			spriteRenderer.sprite = data.carSprites[0];
		else if (rb.velocity.x > 0 && rb.velocity.y < 0)
			spriteRenderer.sprite = data.carSprites[7];
		else if (rb.velocity.x < 0 && rb.velocity.y < 0)
			spriteRenderer.sprite = data.carSprites[5];
		else if (rb.velocity.y > 0)
			spriteRenderer.sprite = data.carSprites[6];
		else if (rb.velocity.y < 0)
			spriteRenderer.sprite = data.carSprites[8];
		else if (rb.velocity.x < 0)
			spriteRenderer.sprite = data.carSprites[3];
		else if (rb.velocity.x > 0)
			spriteRenderer.sprite = data.carSprites[1];
	}
}