using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Camera cam;
    private void Start()
    {
        cam =Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        cam.transform.position = new Vector3(transform.position.x,transform.position.y,cam.transform.position.z);
    }
}
