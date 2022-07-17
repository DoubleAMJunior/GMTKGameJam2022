using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System.Reflection;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]public int checkpointNumber;
    public Checkpoint nextCheckpoint;
    public bool EndOfRace;
    private void Start()
    {
        SetCheckStuff();
    }
    public void SetCheckStuff()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            Checkpoint checkpoint = transform.parent.GetChild(i).gameObject.GetComponent<Checkpoint>();
            checkpoint.checkpointNumber = i;
            checkpoint.nextCheckpoint = transform.parent.GetChild(i + 1 >= transform.parent.childCount ? 0 : i + 1).gameObject.GetComponent<Checkpoint>();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerRankData playerData = other.GetComponent<PlayerRankData>();

        if (playerData == null)
        {
            return;
        }
        
        bool isNextCheckpoint = playerData.LastCheckpointNumber + 1 >= checkpointNumber || playerData.LastCheckpointNumber == -1;
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
        player.nextCheckpoint = nextCheckpoint;

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
