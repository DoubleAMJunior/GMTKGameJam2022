using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioController : MonoBehaviour
{
    private AudioManager _audioManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        LoadMainMenuBgm();
    }

    private void LoadMainMenuBgm()
    {
        _audioManager.StopBGM();
        _audioManager.PlayMainMenuBgm();
    }
}
