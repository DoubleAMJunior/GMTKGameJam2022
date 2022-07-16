using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// ReSharper disable once CheckNamespace
namespace GMTKGameJam2022
{
	public class TrapPlacement : MonoBehaviour
	{
		public GameObject selectedTrap;
		[Header("Add collider on the track and traps, collider can isTrigger")]
		public LayerMask trackLayer;
		public LayerMask trapLayer;

		[Tooltip ( "Minimum Distance Between Traps" )]
		public float minDistanceBetweenTrap = 3f;

		[Header ( "When placing trap, but too close to other trap" )]
		public UnityEvent OnTooCloseTooOtherTraps;

		readonly List<Transform> trapsReference = new List<Transform> (); 

		/// <summary>
		/// Call this method to place a trap
		/// </summary>
		public void PlaceTrap (Vector3 trapPosition)
		{
			if ( Physics2D.OverlapCircle ( trapPosition, minDistanceBetweenTrap, trapLayer ) )
			{
				print("Too close to other trap");
				OnTooCloseTooOtherTraps?.Invoke ();
				return;
			}
			

			// Check if its inside track
			if ( Physics2D.OverlapPoint ( trapPosition, trackLayer ) )
				// Place trap if its pass all requirement
				trapsReference.Add ( Instantiate ( selectedTrap, trapPosition, Quaternion.identity ).transform );
			
		}

		public void SelectTrap (GameObject trap) => selectedTrap = trap;

#if UNITY_EDITOR
		void OnDrawGizmosSelected ()
		{
			// Draw traps distance in editor 
			foreach ( var _ref in trapsReference )
			{
				Gizmos.DrawWireSphere ( _ref.position,minDistanceBetweenTrap );
				
			}
		}
#endif
	}
}
