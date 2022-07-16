using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GMTKGameJam2022.Track
{
    public class StartPoint : MonoBehaviour
    {
        public RaceRoutines race;
        private void Start()
        {
            race.SubscribeInit(Init);
        }

        private void Init()
        {
            var cam = Camera.main;
            cam.gameObject.transform.position =new Vector3(transform.position.x,transform.position.y,cam.transform.position.z);
            race.SetCars(transform.position);
        }
    }
}
