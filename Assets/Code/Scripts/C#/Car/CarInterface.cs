using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CarInterface",menuName ="Game/Car/CarInterface",order =0)]
public class CarInterface : ScriptableObject
{
	//adjustable values for testing
	[HideInInspector] public float accelerationInput = 0f;
	[HideInInspector] public float steeringInput = 0f;

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }
}
