using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


// ReSharper disable once CheckNamespace
namespace GMTKGameJam2022
{
	public class TrapPlacement : MonoBehaviour
	{
		public GameObject selectedTrap, trapSpriteShowcase;
		public Text trapNameTxt, trapInfoTxt;
		public Image SelectedSprite;
		public Sprite warningSprite;
		[Header("Add collider on the track and traps, collider can isTrigger")]
		public LayerMask trackLayer;
		public LayerMask trapLayer;
		public Camera cam;

		[Tooltip ( "Minimum Distance Between Traps" )]
		public float minDistanceBetweenTrap = 2f;

		[Header ( "When placing trap, but too close to other trap" )]
		public UnityEvent OnTooCloseTooOtherTraps;

		readonly List<Transform> trapsReference = new List<Transform> ();

        private void Update()
        {
			Vector3 mousePos=cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)) - Vector3.forward * Camera.main.transform.position.z;
			if (selectedTrap != null && Physics2D.OverlapPoint(mousePos, trackLayer))
			{
				if (!trapSpriteShowcase.activeSelf)
					trapSpriteShowcase.SetActive(true);

				trapSpriteShowcase.transform.position = mousePos;
				if (Physics2D.OverlapCircle(mousePos, minDistanceBetweenTrap, trapLayer))
					trapSpriteShowcase.GetComponent<SpriteRenderer>().sprite = warningSprite;
				else
					trapSpriteShowcase.GetComponent<SpriteRenderer>().sprite = selectedTrap.GetComponent<SpriteRenderer>().sprite;
			}
			else if (trapSpriteShowcase.activeSelf)
				trapSpriteShowcase.SetActive(false);

		}
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

		public void SelectTrap(GameObject trap) 
		{
			selectedTrap = trap;
			GameObject clicked = EventSystem.current.currentSelectedGameObject;
			if(clicked != null)
				SelectedSprite.sprite = clicked.GetComponent<Image>().sprite;
		}
		public void SetTrapName(String trapName) => trapNameTxt.text = trapName;
		public void SetTrapInfo(String trapInfo) => trapInfoTxt.text = trapInfo;

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
