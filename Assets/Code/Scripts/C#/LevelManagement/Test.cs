using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMTKGameJam2022.Test{
    public class Test : MonoBehaviour
    {
        public GameObject prefab;

        public void Make()
        {
            Instantiate(prefab);
        }
    }
}
