using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour {

	public UnityEvent<Vector2> directionInput;
	private void FixedUpdate()
	{
		//get x/y input, send as vector to PlayerMovement
		Vector2 inputVector = Vector2.zero;
		inputVector.x = Input.GetAxis("Horizontal");
		inputVector.y = Input.GetAxis("Vertical");
		directionInput?.Invoke(inputVector);
	}
}