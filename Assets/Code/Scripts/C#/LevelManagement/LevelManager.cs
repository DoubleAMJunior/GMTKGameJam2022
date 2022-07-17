using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GMTKGameJam2022
{ 
    [CreateAssetMenu( fileName ="LevelManager",menuName ="Game/LevelManager",order =0)]
    public class LevelManager : ScriptableObject
    {
        public void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }
        public List<string> TrackList;
        public string TrapPlacementScene;
        public string RaceScene;
        public string TransitionScene;
        [HideInInspector][SerializeField]private LevelState levelState;
        private void OnEnable()
        {

            levelState = LevelState.Idle;
            hideFlags = HideFlags.DontUnloadUnusedAsset;

        }

        public void LoadTrack(int index)
        {
            if (levelState == LevelState.TrapPlace)
                return;
            levelState = LevelState.TrapPlace;
            SceneManager.LoadScene(TransitionScene);
            var op =SceneManager.LoadSceneAsync(TrackList[index],LoadSceneMode.Additive);
            var op2= SceneManager.LoadSceneAsync(TrapPlacementScene,LoadSceneMode.Additive);
            int counter = 0;
            op.completed += (o) =>
            {
                if (counter > 0)
                    SceneManager.UnloadSceneAsync(TransitionScene);
                counter++;
            };
            op2.completed += (o) =>
            {
                if (counter > 0)
                    SceneManager.UnloadSceneAsync(TransitionScene);
                counter++;
            };
        }
        public void LoadRace()
        {
            if (levelState != LevelState.TrapPlace)
                return;
            levelState = LevelState.Race;
            SceneManager.LoadScene(TransitionScene,LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync(TrapPlacementScene);
            var op=SceneManager.LoadSceneAsync(RaceScene,LoadSceneMode.Additive);
            op.completed += (o) =>
            {
                SceneManager.UnloadSceneAsync(TransitionScene);
            };
        }
    }
    public enum LevelState
    {
        TrapPlace,Race,Idle
    }

}