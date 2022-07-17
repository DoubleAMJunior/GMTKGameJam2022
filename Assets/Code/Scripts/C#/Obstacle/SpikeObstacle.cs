using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeObstacle : MonoBehaviour, IObstacle
{
    [Range(0, 30)]
    public int SlowPercent;
    public void OnHit(ICarHitResponse player)
    {
        player.SlowDown(SlowPercent);
        Destroy(gameObject);
    }
}