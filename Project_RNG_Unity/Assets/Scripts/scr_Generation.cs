using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Generation : MonoBehaviour
{

    public string seed;
    public int segmentsToSpawn;
    public int dragonsToSpawn;
    public bool addBlock;
    public bool follow;

    public GameObject head;

    public List<GameObject> prefabs;
    public List<cls_Dragon> dragons;

    public List<GameObject> segments;
    //public List<GameObject> parts;

    void Awake() {
        dragons.Clear();
        for (int i = 0; i < dragonsToSpawn; i++) {
            GameObject headObject = Instantiate(head);
            dragons.Add(new cls_Dragon(headObject));
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int i = 0; i < 10; i++)
            print(pseudoRandom.Next(0, 10).ToString());

        foreach (cls_Dragon d in dragons)
        {
            for (int i = 0; i < segmentsToSpawn; i++)
            {
                SpawnSegments(prefabs[pseudoRandom.Next(0, prefabs.Count)], pseudoRandom, d);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (follow == true)
        {
            follow = false;
            StartCoroutine(MoveBlocks());
        }
    }

    IEnumerator MoveBlocks()
    {
        foreach (cls_Dragon d in dragons)
        {
            for (int i = 1; i < d.segments.Count; i++)
            {
                float dist = 0.0f;

                //get the distance
                foreach (Transform g in d.segments[i - 1].GetComponentInChildren<Transform>())
                    if (g.gameObject.name == "Point_B")
                    {
                        dist = Vector3.Distance(d.segments[i].transform.position, g.transform.position);
                        d.segments[i].GetComponent<LineRenderer>().SetPosition(0, g.transform.position);
                        d.segments[i].GetComponent<LineRenderer>().SetPosition(1, d.segments[i].transform.position);
                    }

                //Move it if you need to
                if (dist > 2)
                {
                    foreach (Transform g in d.segments[i - 1].GetComponentInChildren<Transform>())
                        if (g.gameObject.name == "Point_B")
                        {
                            //Hard look at
                            d.segments[i].transform.LookAt(g.transform);

                            //Smooth look at
                            //Quaternion rotation = Quaternion.LookRotation(g.position - segments[i].transform.position);
                            //segments[i].transform.rotation = Quaternion.Slerp(segments[i].transform.rotation, rotation, Time.deltaTime * 10.0f);
                        }

                    d.segments[i].transform.eulerAngles = new Vector3(d.segments[i].transform.eulerAngles.x, d.segments[i].transform.eulerAngles.y, d.segments[i - 1].transform.eulerAngles.z);
                    d.segments[i].transform.Translate(0, 0, 1.5f);
                }
            }
        }

        //yield return new WaitForSeconds(0.5f);
        yield return new WaitForEndOfFrame();

        StartCoroutine(MoveBlocks());
        yield return null;
    }

    void SpawnSegments(GameObject _prefab, System.Random _pseudoRandom, cls_Dragon d)
    {
            GameObject segment = Instantiate(_prefab);

            float randomScale = _pseudoRandom.Next(1, 5);

            segment.transform.localScale = new Vector3(_pseudoRandom.Next(1, 3), _pseudoRandom.Next(1, 3), _pseudoRandom.Next(1, 3));

            //Get the hingepoint
            foreach (Transform g in d.segments[d.segments.Count - 1].GetComponentInChildren<Transform>())
                if (g.gameObject.name == "Point_B")
                    segment.transform.position = g.position;

            //Rotatie van de vorige block overnemen
            //segment.transform.rotation = segments[segments.Count - 1].transform.rotation;
            //segment.transform.Rotate(0, _pseudoRandom.Next(-90, 90), 0);

            d.segments.Add(segment);

            segment.AddComponent<LineRenderer>();
            segment.GetComponent<LineRenderer>().materials = this.GetComponent<LineRenderer>().materials;
            segment.GetComponent<LineRenderer>().SetWidth(this.GetComponent<LineRenderer>().startWidth, this.GetComponent<LineRenderer>().endWidth);

    }
}

[System.Serializable]
public class cls_Dragon {
    public List<GameObject> segments = new List<GameObject>();

    public cls_Dragon(GameObject _head) {
        segments.Add(_head);
    }
}

