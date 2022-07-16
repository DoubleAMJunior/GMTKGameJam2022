using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMTKGameJam2022.Race
{
    public class RaceManager : MonoBehaviour
    {
        public RaceRoutines raceRoutines;
        public List<GameObject> participants;

        private void Start()
        {
            raceRoutines.SubscribeSetCar(SetStartLine);
            raceRoutines.Init();
        }
        
        private void SetStartLine(Vector3 pos)
        {
            foreach (var participant in participants)
            {
                participant.transform.position = pos;
            }
        }
    }
}
