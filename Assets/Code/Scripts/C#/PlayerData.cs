using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : ScriptableObject {
	public float accelerationMax = 10f;
	public float maxSpeed = 2f;
	public float accelerationFactor = 5f;
	public float turnFactor = 3.5f;
	public float dragValue = 1f;
	public float dragTime = 1f;
	public float driftFactor = .95f;
}