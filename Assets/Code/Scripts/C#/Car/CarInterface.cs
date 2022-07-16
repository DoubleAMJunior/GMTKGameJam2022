using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInterface : ScriptableObject
{
	//adjustable values for testing
	 public float accelerationInput = 0f;
	 public float steeringInput = 0f;

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }
}
