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

    void Start()
    {
        //dragons.Clear();
        for (int i = 0; i < _Storage.RNG(1, 25); i++)
        {
            GameObject headObject = Instantiate(head);
            dragons.Add(new cls_Dragon(headObject));
        }

        foreach (cls_Dragon d in dragons)
        {
            Color colorA = _Storage.Storage().colors[_Storage.RNG(0, _Storage.Storage().colors.Count)];
            Color colorB = _Storage.Storage().colors[_Storage.RNG(0, _Storage.Storage().colors.Count)];

            int amountOfSegments = _Storage.RNG(3, 30);

            for (int i = 0; i < amountOfSegments; i++)
            {
                //Make color lighter the more segments there are
                float b = ((float)i + 1.0f) / (float)amountOfSegments;
                Color tempColor = Color.Lerp(colorA, colorB, b);

                SpawnSegments(prefabs[_Storage.RNG(0, prefabs.Count)], d, tempColor);
            }
        }

        //Attach the camera

        GameObject.FindGameObjectWithTag("MainCamera").transform.parent = dragons[0].segments[0].transform;
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

        yield return new WaitForEndOfFrame();

        StartCoroutine(MoveBlocks());
        yield return null;
    }

    void SpawnSegments(GameObject _prefab, cls_Dragon d, Color _color)
    {
        GameObject segment = Instantiate(_prefab);

        Color tempColor = new Color(_color.r, _color.g, _color.b, _color.a);

        foreach (Transform t in segment.GetComponentInChildren<Transform>())
        {
            if (t.GetComponent<Material>() != null)
                t.GetComponent<Material>().SetColor("_Color", _color);

            foreach (Transform tt in t.GetComponentInChildren<Transform>())
            {
                if (tt.GetComponent<Material>() != null)
                    tt.GetComponent<Material>().SetColor("_Color", _color);

                foreach (Transform ttt in tt.GetComponentInChildren<Transform>())
                {
                    if (ttt.GetComponent<Material>() != null)
                        ttt.GetComponent<Material>().SetColor("_Color", _color);
                }
            }

        }


        //float randomScale = _Storage.RNG(1, 5);

        segment.transform.localScale = new Vector3(_Storage.RNG(1, 3), _Storage.RNG(1, 3), _Storage.RNG(1, 3));

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
        segment.GetComponent<LineRenderer>().SetColors(_color, _color);

        segment.GetComponent<LineRenderer>().SetWidth(this.GetComponent<LineRenderer>().startWidth, this.GetComponent<LineRenderer>().endWidth);
    }
}

[System.Serializable]
public class cls_Dragon
{
    public List<GameObject> segments = new List<GameObject>();

    public cls_Dragon(GameObject _head)
    {
        segments.Add(_head);
    }
}

[System.Serializable]
public class Grid
{
    public bool opuated;
    public Vector3 pos;
    public GameObject obj;

    public Grid(Vector3 _pos, GameObject _obj)
    {
        pos = _pos;
        obj = _obj;
    }
}