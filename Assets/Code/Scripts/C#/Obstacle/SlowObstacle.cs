using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowObstacle : MonoBehaviour, IObstacle
{
    [Range(0,100)]
    public int SlowPercent;
    public void OnHit(ICarHitResponse player)
    {
        player.SlowDown(SlowPercent);
    }
}
