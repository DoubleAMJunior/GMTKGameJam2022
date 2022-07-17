using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoosterObstical : MonoBehaviour, IObstacle
{
    [Range(0, 10000)]
    public float amount;
    public void OnHit(ICarHitResponse player)
    {
        player.SpeedIncrease(amount);
        Destroy(gameObject);
    }
}