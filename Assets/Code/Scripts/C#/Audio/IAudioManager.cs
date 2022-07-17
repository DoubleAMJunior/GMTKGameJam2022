using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioManager
{
    public void PlayMainMenuBgm();
    
    public void PlayMainGameBgm();
    
    public void PlayButtonClickSfx();
    
    public void PlayCarSpeedingUpSfx();
    
    public void PlayCarEngineRunningSfx();
    
    public void PlayCarCrashSfx();
    
    public void PlayCarFallingIntoTrap();

    public void PlayCarEngineTurningOffSfx();

    public void PlayTrafficLightCountdownSfx();

    public void PlayWinningSfx();

    public void PlayObjectPlacementSfx();
}
