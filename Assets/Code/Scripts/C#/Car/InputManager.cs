using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
	CarInterface playerMovement;


	private void FixedUpdate()
	{
		//get x/y input, send as vector to PlayerMovement
		Vector2 inputVector = Vector2.zero;
		inputVector.x = Input.GetAxis("Horizontal");
		inputVector.y = Input.GetAxis("Vertical");

		playerMovement.SetInputVector(inputVector);
	}

	//insert collision mechanics
}