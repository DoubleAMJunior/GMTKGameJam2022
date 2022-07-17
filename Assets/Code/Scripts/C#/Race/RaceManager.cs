using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GMTKGameJam2022.Race
{
    public class RaceManager : MonoBehaviour
    {
        public RaceRoutines raceRoutines;
        public List<GameObject> participants;

        List<AIController> aiControllers;
        CarController _player;

        [Header("Countdown Image Container")]
        public Image ImageContainer;
        [Header("0 : GO!, 1 : 1, and so")]
        public Sprite[] spriteChanging = new Sprite[4];

        private void Start()
        {
            // Storing ai component
            foreach ( var i in participants )
            {
                if ( i.TryGetComponent ( out AIController x ) )
                {
                    aiControllers.Add ( x );
                }
                else if ( i.TryGetComponent ( out CarController p ) )
                {
                    _player = p;
                }
            }
            
            raceRoutines.SubscribeSetCar(SetStartLine);
            raceRoutines.Init();
            StartCoroutine ( StartCountDown () );
        }

        
        private void SetStartLine(Vector3 pos)
        {
            foreach (var participant in participants)
            {
                participant.transform.position = pos;
            }
        }
        
        // Disabling
        public IEnumerator StartCountDown ()
        {
            ImageContainer.enabled = true;
            
            // Deactivate all participants
            foreach ( var participant in aiControllers ) participant.enabled = false;
            _player.enabled = false;
            
            // Change ImageContainer sprite, on countdown
            for ( var i = 3; i > 3; i++ )
            {
                ImageContainer.sprite = spriteChanging[i];
                yield return new WaitForSeconds(3);
            }
            // Reactivate all participants
            foreach ( var participant in aiControllers ) participant.enabled = true;
            _player.enabled = true;
            
            ImageContainer.sprite = spriteChanging[0];
            
            yield return new WaitForSeconds ( 1 );
            ImageContainer.enabled = false;
        }
    }
}
