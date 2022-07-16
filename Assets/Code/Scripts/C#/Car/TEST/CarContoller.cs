using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarContoller : MonoBehaviour
{
    #region properties

    [HideInInspector] public Vector3 targetLookDirectionNormalized;
    [HideInInspector] private float moveController = 0;
    [HideInInspector] public bool controlRotate;
    
    [SerializeField] private float boostPower = 1;
    [SerializeField] private float accelerationPower=50f;
    [SerializeField] private float topSpeed=10f;
    [Space]
    [SerializeField] private float dragChangeSpeed=0.01f;
    [SerializeField] private float movingDrag=0f;
    [SerializeField] private float idleDrag=0.8f;
    [SerializeField] private float driftingDragAccelerate=0.1f;
    [SerializeField] private float driftingDragNoGas=0.5f;
    [Space]
    [SerializeField] private float TurnSpeedNormal=3f;
    [SerializeField] private float TurnSpeedNormalNoGas = 0.07f;

    [Space] 
    [SerializeField] private float breakDriftCartTurnSpeed=7f;
    [SerializeField] private float DriftCarTurnSpeed=100f;
    [SerializeField] private float DriftRotateTurnSpeed=0.05f;
    [SerializeField] private AnimationCurve DriftDegreeCurve;
    [Space]
    [SerializeField] private float BreakSpeed=1f;
    [Space]
    [SerializeField] private float stoppingAngularDrag=20f;
    [SerializeField] private float movingAngularDrag=0.01f;


    public  Rigidbody sphereRB;
    //public  CarBoost carBoost;
    private float breakIntensity;
    private float gassIntensity;
    private int driftDirection;
    private bool inDrift;
    private bool reverse;
    private int currentDirection = 1;
    private Rigidbody2D rb;
    private Quaternion rotator;
    private const float MAX_DRIFT_DEGREE =120;
    private const float MAX_DRIFT_SPEED_HANDLE = 2;
    private const float MIN_DRIFT_ROTATE = 0.01f;
    private const float speedMultiplier = 100;
    private const float DRIFT_VELCHANGE_DEGREE = 90;
    private const float DRIFT_TO_NORMAL_DEGREE = 2;
    private const float fastTrunRate = 5;
    private const float slowTurnRate = 1;
    private const float MAXSPEEDEASE = 1;
    private const float BOOSTMULTIPLIER=1000;


    #endregion

    #region methods

 /*   public void Boost()
    {
        if (carBoost.Boost())
        {
            sphereRB.velocity = transform.forward * sphereRB.velocity.magnitude;
            sphereRB.AddForce(transform.forward * boostPower * BOOSTMULTIPLIER, ForceMode.Impulse);
            StopDrift();
        }
    }*/
    public void BreakAndReverseInput(float intensity)
    {
        breakIntensity=intensity;
        /*var speed = sphereRB.velocity.magnitude;
        if (!reverse)
        {
            if (intensity>0)
            {
                if (speed > 0)
                {
                    // inDrift = true;
                    DecreaseSpeed();
                }
                else
                {
                    currentDirection = -1;
                    reverse = true;
                }
            }
            
        }
        else
        {
            if (intensity==0 && speed.Equals(0f))
            {
                currentDirection = 1;
                reverse = false;
                return;
            }
            SetMoveController(-1 * intensity);
        }*/
    }

    public void GasInput(float intensity)
    {
        gassIntensity = intensity;
//        if (reverse)
//        {
//            if (intensity>0)
//            {
//                DecreaseSpeed();
//            }
//        }
//        else
//        {
//            SetMoveController( intensity);
//        }
    }
    
    public void Rotate(float direction)
    {
        if (inDrift)
        {
            DriftRotate(direction);
        }
        else
        {
            NormalRotate(direction);
        }
    }
    
    public void NoRotation()
    {
        rb.angularDrag = stoppingAngularDrag;
//        if (moveController > 0)
//            rb.drag = movingDrag;
//        else
//            rb.drag = stoppingDrag;
    }

    #endregion
    private void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        sphereRB.angularDrag = idleDrag;
    }

    private void Update()
    {
        transform.position = sphereRB.position;
        /*RaycastHit hit;
        Physics.Raycast(transform.position, transform.up * -1,out hit);
        if (!(hit.collider is null))
        {
            transform.up = hit.normal;
        }*/
    }
    
    private void FixedUpdate () {
        EvaluateState();
        if (moveController != 0)
        {
            if (inDrift)
            {
                DriftAccelerate();
            }
            else
            {
                NormalAccelerate();
            }
        }
        if (inDrift)
        {
            FixDriftVelocity();
        }
    }

    private void StopDrift()
    {
        driftDirection = 0;    
        inDrift = false;
    }
    private void EvaluateState()
    {
        if (inDrift)
        {
            if (Mathf.Sign(Vector3.SignedAngle(sphereRB.velocity, transform.forward,transform.up))!=driftDirection)
            {
                StopDrift();
            }
        }
        if (gassIntensity>0 && breakIntensity>0)
        {
            if(!reverse)
                inDrift = true;
            Break();
            if (sphereRB.velocity.magnitude.Equals(0f))
            {
                inDrift = false;
                SetMoveController(0);
            }
        }
        else if (gassIntensity>0)
        {
            if (reverse)
            {
                Break();
                SetMoveController(0);
                if (sphereRB.velocity.magnitude.Equals(0f))
                {
                    reverse = false;
                }
            }
            else
            {
                SetMoveController(gassIntensity);
            }
        } 
        else if(breakIntensity>0)
        {
            if (reverse)
            {
                if (sphereRB.velocity.magnitude.Equals(0f))
                {
                    reverse = false;
                    SetMoveController(0);
                }
                else
                {
                    SetMoveController(-1*breakIntensity);
                }
            }
            else
            {
                Break();
               inDrift = true;
                SetMoveController(0);
                if (sphereRB.velocity.magnitude.Equals(0f))
                {
                  inDrift = false;
                    reverse = true;
                }
            }
        }
        else
        {
            SetMoveController(0);
        }
    }

    private void FixDriftVelocity()
    {
        var currentAngle = Vector3.Angle(sphereRB.velocity, transform.forward);
        Vector3 newDir = Vector3.RotateTowards(sphereRB.velocity,transform.forward,
            DriftRotateTurnSpeed*DriftDegreeCurve.Evaluate(currentAngle/DRIFT_VELCHANGE_DEGREE),0);
        sphereRB.velocity = newDir;
    } 

    private void NormalAccelerate()
    {
        if (sphereRB.velocity.magnitude >= topSpeed  * Mathf.Abs(moveController))
        {
            sphereRB.velocity =Vector3.MoveTowards(sphereRB.velocity, transform.forward.normalized * topSpeed * moveController,MAXSPEEDEASE);
        }
        sphereRB.AddForce(transform.forward * accelerationPower * speedMultiplier* Mathf.Sign(moveController), ForceMode.Force);
    }

    private void NormalRotate(float direction)
    {
        var sphereVelMagn = sphereRB.velocity.magnitude;
        if (sphereVelMagn>0)
        {
            var carReverse = reverse ? -1 : 1;
            var rotateSpeed = gassIntensity > 0 ? TurnSpeedNormal : TurnSpeedNormalNoGas;
            Vector3 newDir = Vector3.RotateTowards(transform.forward,transform.right*Mathf.Sign(direction)*carReverse,
                Mathf.Lerp(0,rotateSpeed,sphereVelMagn/topSpeed),0);
            transform.forward = newDir;
            sphereRB.velocity = sphereVelMagn * newDir * carReverse;
        }
        
    }
    
    private void DriftAccelerate()
    {
        var velocityNormal = sphereRB.velocity.normalized;
        if (sphereRB.velocity.magnitude >= topSpeed  * moveController)
        {
            sphereRB.velocity =Vector3.MoveTowards(sphereRB.velocity, velocityNormal* topSpeed * moveController,MAXSPEEDEASE);
        }
        sphereRB.AddForce(velocityNormal* accelerationPower * speedMultiplier, ForceMode.Force);
    }
    private void DriftRotate(float direction)
    {
        var cAngle=Vector3.SignedAngle(sphereRB.velocity, transform.forward, transform.up);
        if ( (Mathf.Abs(cAngle)<MAX_DRIFT_DEGREE || Mathf.Sign(cAngle)!=Mathf.Sign(direction)))
        {
            rb.angularDrag = Mathf.Lerp(stoppingAngularDrag, movingAngularDrag, sphereRB.velocity.magnitude / MAX_DRIFT_SPEED_HANDLE);
            driftDirection = driftDirection == 0 ? (int) Mathf.Sign(direction) : driftDirection;
            var rotateSpeed = breakIntensity > 0 ? breakDriftCartTurnSpeed : DriftCarTurnSpeed;
            rb.AddTorque(
                new Vector3(0,
                    Mathf.Lerp(MIN_DRIFT_ROTATE, rotateSpeed, sphereRB.velocity.magnitude / MAX_DRIFT_SPEED_HANDLE) *
                    direction, 0), ForceMode.Force);
        }
        else
        {
            NoRotation();
        }
    }

    private void Break()
    {
        var speedMagnitude = sphereRB.velocity.magnitude;
        speedMagnitude = Mathf.MoveTowards(speedMagnitude, 0,BreakSpeed);
        sphereRB.velocity = sphereRB.velocity.normalized * speedMagnitude;
    }

   
    private void SetMoveController(float val)
    {
        //TODO drag also be included with angular drag
        float targetDrag = 0;
        if (val>0)
        {
            if (inDrift)
            {
                targetDrag=driftingDragAccelerate;
            }
            else
            {
                targetDrag = movingDrag;    
            }
        }
        else
        {
            if (inDrift)
            {
                targetDrag = driftingDragNoGas;
            }
            else
            {
                targetDrag = idleDrag;
            }
        }
        sphereRB.angularDrag = Mathf.MoveTowards(sphereRB.angularDrag,targetDrag,dragChangeSpeed);
        moveController = val;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position,transform.position+sphereRB.velocity);
    }
}