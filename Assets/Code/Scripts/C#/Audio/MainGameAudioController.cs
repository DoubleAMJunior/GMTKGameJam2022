using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameAudioController : MonoBehaviour
{
    
    private AudioManager _audioManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        LoadMainGameBgm();
    }

    private void LoadMainGameBgm()
    {
        _audioManager.StopBGM();
        _audioManager.PlayMainGameBgm();
    }
}
