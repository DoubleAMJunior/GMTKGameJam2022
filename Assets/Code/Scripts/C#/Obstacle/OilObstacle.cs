using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilObstacle : MonoBehaviour, IObstacle
{
    [Range(0, 1)]
    public float time;
    public void OnHit(ICarHitResponse player)
    {
        player.SkidOut(time);
        Destroy(gameObject);
    }
}