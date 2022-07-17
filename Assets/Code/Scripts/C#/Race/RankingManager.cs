using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance;

    private GameObject[] checkpointsGameObjects;
    private List<Checkpoint> Checkpoints;
    private GameObject[] playersGameObjects;
    [SerializeField] public List<PlayerRankData> Ranks;

    public int highestCheckpointNumber;
    
    // checks the ranks when RankCheckTime is passed and resets the timer.
    public const float RankCheckTime = 0.3f;
    private float RankCheckTimer;

    private void Awake()
    {
        Instance = this;
        checkpointsGameObjects = GameObject.FindGameObjectsWithTag("Checkpoint");
        Checkpoints = new List<Checkpoint>();
        playersGameObjects = GameObject.FindGameObjectsWithTag("Player");
        Ranks = new List<PlayerRankData>();
    }

    void Start()
    {
        // find checkpoints and add to "Checkpoints"
        foreach (GameObject checkpointsGameObject in checkpointsGameObjects)
        {
            Checkpoint tmp = checkpointsGameObject.GetComponent<Checkpoint>();
            if (tmp.checkpointNumber >= highestCheckpointNumber)
                highestCheckpointNumber = tmp.checkpointNumber;
            Checkpoints.Add(tmp);
        }
        foreach (GameObject playersGameObject in playersGameObjects)
        {
            Ranks.Add(playersGameObject.GetComponent<PlayerRankData>());
            Debug.Log("Added a rank to the AI System");
        }
        
    }

    void Update()
    {
        //update ranks interval times
        RankCheckTimer += Time.deltaTime;

        if (RankCheckTimer > RankCheckTime)
        {
            RankCheckTimer = 0f;

            UpdatePlayerRanks();
        }
    }

    private void UpdatePlayerRanks()
    {
        Ranks.Sort((x, y) => y.CompareTo(x));
        UpdatePlayerDataRankVariables();
    }

    private void UpdatePlayerDataRankVariables()
    {
        for (int i = 1; i < Ranks.Count + 1; i++)
        {
            Ranks[i-1].UpdateCurrentRank(i);
        }
    }

    public Checkpoint GetCheckpoint(int checkpointNumber)
    {
        foreach (Checkpoint checkpoint in Checkpoints)
        {
            if (checkpoint.checkpointNumber == checkpointNumber)
            {
                return checkpoint;
            }
        }

        Debug.Log("Couldn't find the " + checkpointNumber + "'th checkpoint! next checkpoint is start/end checkpoint");
        return Checkpoints[0];
    }
}
