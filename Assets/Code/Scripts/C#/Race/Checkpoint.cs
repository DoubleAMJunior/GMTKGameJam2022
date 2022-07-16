using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointNumber;
    public bool EndOfRace;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerRankData playerData = other.GetComponent<PlayerRankData>();

        if (playerData == null)
        {
            return;
        }
        
        bool isNextCheckpoint = playerData.LastCheckpointNumber + 1 == checkpointNumber;
        bool isEndOfLap = checkpointNumber == 0 &&
                          playerData.LastCheckpointNumber == RankingManager.Instance.highestCheckpointNumber;
        
        if(isNextCheckpoint || isEndOfLap)
            UpdatePlayerRankData(playerData);
        else
        {
            Debug.Log("YOU MISSED A CHECKPOINT! " +
                      "(check if checkpoint collider and size is correct! or maybe numbering of checkpoints are wrong)");
        }
    }

    private void UpdatePlayerRankData(PlayerRankData player)
    {
        UpdateLastCheckpoint(player);
        if (checkpointNumber == 0)
        {
            UpdateLapNumber(player);
        }
    }

    private void UpdateLastCheckpoint(PlayerRankData player)
    {
        player.LastCheckpointNumber = this.checkpointNumber;
        player.LastCheckpoint = this;
    }

    private void UpdateLapNumber(PlayerRankData player)
    {
        player.increaseLap();
    }

    public float DistanceFromPosition(Vector2 playerPosition)
    {
        Vector3 pos = transform.position;
        Vector2 checkpointPosition = new Vector2(pos.x, pos.y);

        float result = Vector2.Distance(playerPosition, checkpointPosition);
        result = math.abs(result);

        return result;
    }

}
