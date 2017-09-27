using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Sliert : MonoBehaviour {

    public float distance;
    public List<GameObject> realPoints;
    public List<Vector3> points;
    public int speed = 10;

    // Use this for initialization
    void Start()
    {
        this.GetComponent<LineRenderer>().positionCount = points.Count;
        this.GetComponent<LineRenderer>().materials = GameObject.FindGameObjectWithTag("GameController").GetComponent<LineRenderer>().materials;
        this.GetComponent<LineRenderer>().SetColors(_Storage.Storage().colors[_Storage.RNG(0, _Storage.Storage().colors.Count)], _Storage.Storage().colors[_Storage.RNG(0, _Storage.Storage().colors.Count)]);


        speed = Random.Range(1, 10);
    }

    // Update is called once per frame
    void Update () {
        for (int i = 0; i < realPoints.Count; i++)
                this.GetComponent<LineRenderer>().SetPosition(i, realPoints[i].transform.position);

        for (int i = 1; i < realPoints.Count; i++)
        {
            if (realPoints[i].transform.parent != null)
                realPoints[i].transform.parent = null;

            float dist = Vector3.Distance(realPoints[i].transform.position, realPoints[i - 1].transform.position);

            if (dist > distance)
            {
                realPoints[i].transform.LookAt(realPoints[i - 1].transform.position);
                realPoints[i].transform.Translate(0, 0, dist/ speed);
            }
        }
            
	}
}
