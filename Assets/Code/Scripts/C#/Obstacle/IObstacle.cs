using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IObstacle
{
    void OnHit(ICarHitResponse player);
}
