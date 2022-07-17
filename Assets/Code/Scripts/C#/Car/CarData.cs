//for bigger tracks or faster movement, start testing from commented values

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( fileName = "CarData" , menuName = "Game/Car/CarData",order =0 )]
public class CarData : ScriptableObject {
	public float accelerationMax = 5f; //10f;
	public float maxSpeed = 10f; //50f;
	public float accelerationFactor = .01f; //2f;
	public float turnFactor = 40f;
	public float dragValue = 100f;
	public float dragTime = 3f;
	public float driftFactor = .5f;
	public PhysicsMaterial2D normalPhysics, skidPhysics;
	public Sprite normalCar, wrechedCar;

	public Sprite[] carSprites = new Sprite[8];
}