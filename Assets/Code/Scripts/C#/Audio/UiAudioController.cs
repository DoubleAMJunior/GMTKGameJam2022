using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiAudioController : MonoBehaviour
{
    private AudioManager _audioManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
    }

    public void LoadUiClickSfx()
    {
        _audioManager.PlayButtonClickSfx();
    }
}
