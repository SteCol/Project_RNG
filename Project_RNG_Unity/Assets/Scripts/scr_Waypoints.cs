using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Waypoints : MonoBehaviour
{
    public float speed;
    public List<Vector3> waypoints;
    public int rngValueA, rngValueB;

    void Start()
    {
        string seed = GameObject.FindGameObjectWithTag("GameController").GetComponent<scr_Generation>().seed;
        //System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int i = 0; i < 10; i++)
            waypoints.Add(new Vector3(_Storage.RNG(rngValueA, rngValueB), _Storage.RNG(rngValueA, rngValueB), _Storage.RNG(rngValueA, rngValueB)));

        StartCoroutine(iNavigateTo(waypoints[_Storage.RNG(0, waypoints.Count)]));
    }

    void Update()
    {
        this.transform.Translate(0, 0, speed * Time.deltaTime);
        //this.transform.Translate(0, 0, 1.5f);

    }

    public IEnumerator iNavigateTo(Vector3 _point)
    {
        int r = Random.Range(-180, 180);
        for (float i = 0; i < 0.2f; i = i + 0.001f)
        {
            if (Vector3.Distance(this.transform.position, _point) < 1.0f)
                break;

            Quaternion rotation = Quaternion.LookRotation(_point - this.transform.position);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, i);
            float zRot = Mathf.Lerp(this.transform.eulerAngles.z, r, i/7);
            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, transform.eulerAngles.y, zRot);

            yield return new WaitForSeconds(0.001f);
        }

        StartCoroutine(iNavigateTo(waypoints[_Storage.RNG(0, waypoints.Count)]));
        yield return null;
    }
}
