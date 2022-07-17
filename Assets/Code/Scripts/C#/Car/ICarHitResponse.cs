using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarHitResponse 
{
    public void SlowDown(int percent);
    public void SpeedIncrease(float amount);
    public void DisableEngine(float time);
    public void SkidOut(float time);

}
