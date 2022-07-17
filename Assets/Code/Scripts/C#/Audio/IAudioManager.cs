using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioManager
{
    public void PlaySFX(AudioClip clip);
    public void PlayBGM(AudioClip clip);
    public void PlayRandomSFX(params AudioClip[] clips);
    public void StopBGM();

}
