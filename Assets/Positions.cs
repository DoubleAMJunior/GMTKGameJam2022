using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positions : MonoBehaviour
{
    public static int levelLoaded;
    public Vector3[] camPositionsPerLevel;
    public float[] camSizes;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = camPositionsPerLevel[levelLoaded];
        GetComponent<Camera>().orthographicSize = camSizes[levelLoaded];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
