using UnityEngine;

// ReSharper disable once CheckNamespace
namespace GMTKGameJam2022
{
	public class TrapPlacement : MonoBehaviour
	{
		public GameObject selectedTrap;
		public LayerMask trackLayer;

		[Tooltip ( "Minimum Distance Between Traps" )]
		public float minDistanceBetweenTrap = 3f;


		/// <summary>
		/// Call this method to place a trap
		/// </summary>
		public void PlaceTrap (Vector3 trapPosition)
		{
			if ( Physics2D.OverlapCircle ( trapPosition, minDistanceBetweenTrap ) )
			{
				print("Too close to other trap");
				return;
			}
			
			// Check if its inside track
			// TODO

			if ( Physics2D.OverlapPoint ( trapPosition, trackLayer ) )
			{
				// Place trap if its pass all requirement
				Instantiate ( selectedTrap, trapPosition, Quaternion.identity );
			}
		}

		public void SelectTrap (GameObject trap) => selectedTrap = trap;
	}
}