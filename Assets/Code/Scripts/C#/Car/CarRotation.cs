using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRotation : MonoBehaviour
{
	//[SerializeField] private Transform car;
	[SerializeField] private Animator anim;
	[SerializeField] private Transform car;
	[SerializeField] private CarController carController;

    private void Update()
    {
        transform.position = new Vector3(car.position.x, car.position.y, transform.position.z);
    }

	public void Animate()
	{
		float value = carController.rotationAngle % 360;
		value = (value + 360) % 360;
		anim.SetFloat("CarRotationAngle", value);
	}
}
