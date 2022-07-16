using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// ReSharper disable once CheckNamespace
namespace GMTKGameJam2022
{
	public class TrapInputManager : MonoBehaviour
	{
		public KeyCode input = KeyCode.Mouse0;
		
		[Header("When placing trap")]
		public UnityEvent<Vector3> OnTrapPlace;
		
		[Header("When mouse press UI button to change trap")]
		public UnityEvent OnChangeTrap;

		Camera cam;
		Vector3 getMouse => cam.ScreenToWorldPoint ( new Vector3 ( Input.mousePosition.x, Input.mousePosition.y, 0 ) );
		void Awake () => cam = Camera.main;

		public void Update ()
		{
			if ( !Input.GetKeyDown ( input ) ) return;

			if ( !EventSystem.current.IsPointerOverGameObject () )
			{
				// if cursor not in UI button / placing trap
				// The "Vector3.forward * 10" is for:
				// 		if the camera z and the object is same, the object will not visible 
				OnTrapPlace?.Invoke ( getMouse - Vector3.forward * cam.transform.position.z );
			}
			else
			{
				// if cursor on UI button
				OnChangeTrap?.Invoke ();
			}
		}
	}
}