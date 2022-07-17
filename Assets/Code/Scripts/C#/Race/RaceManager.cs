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

        public Text playerPosTxt;
        PlayerRankData rankData;
        public List<AIController> aiControllers;
        CarController _player;

        [Header("Countdown Image Container")]
        public Image ImageContainer;
        [Header("0 : GO!, 1 : 1, and so")]
        public Sprite[] spriteChanging = new Sprite[4];

        private void Start()
        {
            playerPosTxt.gameObject.SetActive(false);
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
                    rankData = _player.GetComponent<PlayerRankData>();
                }
            }
            
            raceRoutines.SubscribeSetCar(SetStartLine);
            raceRoutines.Init();
            StartCoroutine ( StartCountDown () );
        }

        private void Update()
        {
            playerPosTxt.text = rankData.CurrentRank.ToString();
        }

        private void SetStartLine(Vector3 pos, int direction)//0 up, 1 down, 2 left, 3 right
        {
            for(int i = 0; i < participants.Count; i++)
            {
                Vector3 rotation =  Vector3.zero;// = participants[i].transform.rotation.eulerAngles;
                if (direction == 0)
                {
                    pos.y -= 0.3f * i;
                    rotation = new Vector3(0, 0, 0);
                }
                else if (direction == 1)
                {
                    pos.y += 0.3f * i;
                    rotation = new Vector3(0, 0, 180);
                }
                else if (direction == 2)
                {
                    pos.x -= 0.3f * i;
                    rotation = new Vector3(0, 0, 90);
                }
                else if (direction == 3)
                {
                    pos.x += 0.3f * i;
                    rotation = new Vector3(0, 0, 270);
                }
                //pos.y -= 0.3f * i;
                // if (i % 2 == 0)
                //     pos.x += 0.3f;
                participants[i].transform.position = pos;
                participants[i].transform.rotation = Quaternion.Euler(rotation);
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
            for ( var i = 3; i > 0; i-- )
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
            playerPosTxt.gameObject.SetActive(true);
        }
    }
}
