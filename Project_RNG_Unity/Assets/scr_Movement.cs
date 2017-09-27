using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Movement : MonoBehaviour {

    public List<Vector3> waypoints;
    public int rngValueA, rngValueB;

    void Awake()
    {
        for (int i = 0; i < 20; i++)
        {
            waypoints.Add(new Vector3(Random.Range(rngValueA, rngValueB), Random.Range(rngValueA, rngValueB), Random.Range(rngValueA, rngValueB)));
        }
    }
}
