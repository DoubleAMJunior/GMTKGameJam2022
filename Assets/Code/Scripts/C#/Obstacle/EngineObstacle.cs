using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineObstical : MonoBehaviour, IObstacle
{
    [Range(0, 100)]
    public int SlowPercent;
    public void OnHit(ICarHitResponse player)
    {
        player.DisableEngine(SlowPercent);
        Destroy(gameObject);
    }
}