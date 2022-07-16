/* unfinished!
to fix:
	- scriptable object (player data) :sob:
to add:
	- oil mechanic
	- only allow acceleration when event
	- collision (player movement script)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class CarController : MonoBehaviour ,ICarHitResponse
{
	[SerializeField] private CarData data;
    [SerializeField] private CarInterface carInterface;
	private Rigidbody2D rb;
	private float rotationAngle = 0f;
	private float velocityVsUp = 0f;

	
	
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		ApplyForwardForce();
		KillSideVelocity();
		ApplySteering();

	}

	void ApplyForwardForce()
	{
		//apply drag when there is no accelerationInput
		if (carInterface.accelerationInput == 0)
		{
			rb.drag = Mathf.Lerp(rb.drag, data.dragValue, Time.fixedDeltaTime * data.dragTime);
		}
		else
		{
			rb.drag = 0;
		}

		//limit max forward speed and reverse speed
		velocityVsUp = Vector2.Dot(transform.up, rb.velocity);
		if (velocityVsUp > data.maxSpeed && carInterface.accelerationInput > 0)
		{
			return;
		}

		if (velocityVsUp < data.maxSpeed && carInterface.accelerationInput < 0) //max speed
		{
			return;
		}

		if (velocityVsUp < -.5f * data.maxSpeed && carInterface.accelerationInput < 0) //reverse speed
		{
			return;
		}

		if (rb.velocity.sqrMagnitude > data.maxSpeed * data.maxSpeed && carInterface.accelerationInput > 0) //cannot go faster in any direction while accelerating
		{
			return;
		}

		//move car forward
		Vector2 engineForceVector = transform.up * carInterface.accelerationInput * data.accelerationFactor;
		rb.AddForce(engineForceVector, ForceMode2D.Force);
	}

	void ApplySteering()
	{
		rotationAngle -= carInterface.steeringInput * data.turnFactor;
		rb.MoveRotation(rotationAngle);
	}

	void KillSideVelocity()
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