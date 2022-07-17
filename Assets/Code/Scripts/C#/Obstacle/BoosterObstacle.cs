using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoosterObstacle : MonoBehaviour, IObstacle
{
    [Range(0, 5)]
    public float amount;
    public void OnHit(ICarHitResponse player)
    {
        player.SpeedIncrease(amount);
        Destroy(gameObject);
    }
}