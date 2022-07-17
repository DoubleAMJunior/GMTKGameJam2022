using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilObstical : MonoBehaviour, IObstacle
{
    [Range(0, 100)]
    public float time;
    public void OnHit(ICarHitResponse player)
    {
        player.SkidOut(time);
        Destroy(gameObject);
    }
}