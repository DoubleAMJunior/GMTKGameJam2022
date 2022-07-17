using System;
using UnityEngine;

namespace GMTKGameJam2022
{
    [CreateAssetMenu(fileName ="RaceRoutine",menuName ="Game/RaceRoutine",order =1)]
    public class RaceRoutines : ScriptableObject
    {
        private Action initialize;
        private Action startRace;
        private Action<Vector3, int> setCars;
        private void OnEnable()
        {
            initialize = null;
            startRace = null;
            setCars = null;
        }

        public void SubscribeInit(Action action)
        {
            if (action != null)
                initialize += action;
        }

        public void Init()
        {
            initialize?.Invoke();
        }
        public void SubscribeStart(Action action)
        {
            if (action != null)
                startRace += action;
        }

        public void StartRace()
        {
            startRace?.Invoke();
        }
        public void SubscribeSetCar(Action<Vector3, int> action)
        {
            if (action != null)
                setCars += action;
        }

        public void SetCars(Vector3 pos, int direction)
        {
            setCars?.Invoke(pos, direction);
        }
    }
}