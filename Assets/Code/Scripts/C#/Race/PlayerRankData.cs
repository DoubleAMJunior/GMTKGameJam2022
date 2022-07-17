using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerRankData : MonoBehaviour, IComparable<PlayerRankData>
{
    public int CurrentRank;
    public int LastCheckpointNumber = -1;
    public Checkpoint LastCheckpoint, nextCheckpoint;
    public int CurrentLap = 0;
    public bool bIsPlayer = false;
    public Text lapTxt;

    // IMPORTANT: make this a reference to PLAYER and get position verctor2 of it when needed
    private Vector2 PlayerPosition;
    

    private void Start()
    {
    }


    public static bool operator >(PlayerRankData a, PlayerRankData b)
    {
        if (a.CurrentLap > b.CurrentLap)
        {
            return true;
        }

        if (a.CurrentLap < b.CurrentLap)
        {
            return false;
        }

        if (a.LastCheckpointNumber > b.LastCheckpointNumber)
        {
            return true;
        }

        if (a.LastCheckpointNumber < b.LastCheckpointNumber)
        {
            return false;
        }

        if (LowerRankOnSameCheckpoint(a, b) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool operator <(PlayerRankData a, PlayerRankData b)
    {
        if (a.CurrentLap < b.CurrentLap)
        {
            return true;
        }

        if (a.CurrentLap > b.CurrentLap)
        {
            return false;
        }

        if (a.LastCheckpointNumber < b.LastCheckpointNumber)
        {
            return true;
        }

        if (a.LastCheckpointNumber > b.LastCheckpointNumber)
        {
            return false;
        }

        if (LowerRankOnSameCheckpoint(a, b) == -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int CompareTo(PlayerRankData b)
    {
        if (this.CurrentLap > b.CurrentLap)
        {
            return 1;
        }

        if (this.CurrentLap < b.CurrentLap)
        {
            return -1;
        }

        if (this.LastCheckpointNumber > b.LastCheckpointNumber)
        {
            return 1;
        }

        if (this.LastCheckpointNumber < b.LastCheckpointNumber)
        {
            return -1;
        }

        int result = LowerRankOnSameCheckpoint(this, b);

        return result;
    }

    public static int LowerRankOnSameCheckpoint(PlayerRankData player, PlayerRankData enemy)
    {
        // IMPORTANT: get both player and enemy PlayerPosition!
        if (player.CurrentLap == 0 && enemy.CurrentLap == 0 && player.LastCheckpoint is null || enemy.LastCheckpoint is null)
            return 0;

        float playerDistance = player.LastCheckpoint.DistanceFromPosition(player.PlayerPosition);
        float enemyDistance = enemy.LastCheckpoint.DistanceFromPosition(player.PlayerPosition);
        int result;
        result = playerDistance > enemyDistance ? 1 : 0;
        result = playerDistance < enemyDistance ? -1 : result;

        return result;
    }

    public void increaseLap()
    {
        CurrentLap++;
        if(bIsPlayer)
        {
            lapTxt.text = CurrentLap + "/3";
        }
        //you can check end of race here!
        if(CurrentLap > 3)
        {
            if(bIsPlayer)
            {
                SceneManager.LoadScene("Win");
            }
            else
            {
                SceneManager.LoadScene("Lose");
            }
            //win the game
            //check if this is the player, otherwise lose
        }
    }

    public void UpdateCurrentRank(int rank)
    {
        CurrentRank = rank;
    }
}
