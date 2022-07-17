using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class CarController : MonoBehaviour ,ICarHitResponse
{
	[SerializeField] private CarData data;
    [SerializeField] protected CarInterface carInterface;
	private Rigidbody2D rb;
	private float rotationAngle = 0f;
	private float velocityVsUp = 0f;

	protected PlayerRankData rankData;	
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		rankData = GetComponent<PlayerRankData>();
	}

	protected virtual void FixedUpdate()
	{
		ApplyForwardForce(carInterface.accelerationInput);
		KillSideVelocity();
		ApplySteering(carInterface.steeringInput);
	}

	protected void ApplyForwardForce(float force)
	{
		//apply drag when there is no accelerationInput
		if (force <= 0.5f && force >= -0.5f)
		{
			rb.drag = Mathf.Lerp(0, data.dragValue, Time.fixedDeltaTime * data.dragTime);
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
		Vector2 engineForceVector = transform.up * force * data.accelerationFactor;
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
}