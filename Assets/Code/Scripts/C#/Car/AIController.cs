using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : CarController
{
    [Range(-180, 180)]
    public float maxPartialSteering; //whats the max angle the car can be before it using full steering
    protected override void FixedUpdate()
    {
        float steerForce = CalculateSteerForce();
        if (bEngineActive)
            ApplyForwardForce(ThrottleForce(steerForce));

        if (bKillSideVelocity)
            KillSideVelocity();

        ApplySteering(steerForce);
        ////carRotation.Animate();
    }

    float CalculateSteerForce()
    {
        if (rankData.nextCheckpoint != null)
        {
            Vector2 targetDir = rankData.nextCheckpoint.gameObject.transform.position - transform.position;
            targetDir = targetDir.normalized;

            ////Debug.DrawRay(transform.position, targetDir * 2);
            float angleToTarget = Vector2.SignedAngle(transform.up, targetDir) * -1; //-1 as need to move opposide dir to the way angle would take us


            float value = angleToTarget / maxPartialSteering;
            value = Mathf.Clamp(value, -1, 1);
            Debug.Log("Angle to the target checkpoint: " + angleToTarget);

            return value;
        }
        return 0;
    }

    float ThrottleForce(float steerSpeed)
    {
        return 1.1f - Mathf.Abs(steerSpeed);
    }
}
