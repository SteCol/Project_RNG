using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class scr_Generation : MonoBehaviour
{
    public string seed;
    public int segmentsToSpawn;
    public int dragonsToSpawn;
    public bool addBlock;
    public bool follow;

    public GameObject head;
    public GameObject mainHead;
    public Vector3 cameraPos;
    public Quaternion cameraRot;

    public List<GameObject> prefabs;
    public List<cls_Dragon> dragons;

    public List<GameObject> segments;

    [Header("UI stuff")]
    public InputField seedString;

    void Start()
    {
        Regen();
    }

    public void Regen()
    {

        _Storage.Storage().SetSeed(seedString.text);

        dragons.Clear();
        mainHead.transform.position = new Vector3(0, 0, 0);
        mainHead.transform.rotation = new Quaternion(0, 180, 0, 0);
        dragons.Add(new cls_Dragon(mainHead));

        foreach (GameObject s in GameObject.FindGameObjectsWithTag("Segment"))
        {
            Destroy(s);
        }

        //for (int i = 0; i < dragons.Count; i++) {
        //    for (int j = 0; j < dragons[i].segments.Count; j++)
        //    {
        //        if (i == 0 && j == 0)
        //        {
        //            print("this is player head do not destroy");
        //        }
        //        else
        //        {
        //            toDestroy = dragons[i].segments[j];
        //            dragons[i].segments.Remove(toDestroy);
        //            Destroy(toDestroy);
        //        }
        //    }
        //}


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
            d.segments[0].GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", colorA);

            for (int i = 0; i < amountOfSegments; i++)
            {
                //Make color lighter the more segments there are
                float b = ((float)i + 1.0f) / (float)amountOfSegments;
                Color tempColor = Color.Lerp(colorA, colorB, b);

                StartCoroutine(iSpawnSegments(prefabs[_Storage.RNG(0, prefabs.Count)], d, tempColor));
            }
        }

        //Attach the camera

        //GameObject.FindGameObjectWithTag("MainCamera").transform.parent = dragons[0].segments[0].transform;
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
                            //Quaternion rotation = Quaternion.LookRotation(g.position - d.segments[i].transform.position);
                            //d.segments[i].transform.rotation = Quaternion.Slerp(d.segments[i].transform.rotation, rotation, Time.deltaTime * dist * 3);
                        }

                    d.segments[i].transform.eulerAngles = new Vector3(d.segments[i].transform.eulerAngles.x, d.segments[i].transform.eulerAngles.y, d.segments[i - 1].transform.eulerAngles.z);
                    d.segments[i].transform.Translate(0, 0, /*1.5f*/ 100 * Time.deltaTime);
                }

                //Get the rotation
                //if (dist > 4.0f)
                //{
                //    foreach (Transform g in d.segments[i - 1].GetComponentInChildren<Transform>())
                //        if (g.gameObject.name == "Point_B")
                //        {
                //            //Hard look at
                //            d.segments[i].transform.LookAt(g.transform);

                //            //Smooth look at
                //            //Quaternion rotation = Quaternion.LookRotation(g.position - d.segments[i].transform.position);
                //            //d.segments[i].transform.rotation = Quaternion.Slerp(d.segments[i].transform.rotation, rotation, Time.deltaTime * 1.0f);
                //        }

                //    d.segments[i].transform.eulerAngles = new Vector3(d.segments[i].transform.eulerAngles.x, d.segments[i].transform.eulerAngles.y, d.segments[i - 1].transform.eulerAngles.z);
                //}
            }
        }

        yield return new WaitForEndOfFrame();

        StartCoroutine(MoveBlocks());
        yield return null;
    }

    IEnumerator iSpawnSegments(GameObject _prefab, cls_Dragon d, Color _color)
    {
        GameObject segment = Instantiate(_prefab);

        Color tempColor = new Color(_color.r, _color.g, _color.b, _color.a);

        //Set random schale
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

        SetAnimationOffset(segment, (float)_Storage.RNG(0, 100) / 100);

        yield return new WaitForEndOfFrame();

        //int layerdepth = 5; 
        //GameObject seg = segment;
        //GameObject seg2 = seg;
        //List<GameObject> toChangeColor = new List<GameObject>();
        //toChangeColor.Add(segment);

        //for (int i = 0; i < layerdepth; i++)
        //{
        //    foreach (GameObject g in toChangeColor) {
        //        foreach (Transform t in g.GetComponent<Transform>()) {
        //            toChangeColor.Add(t.gameObject);
        //            yield return new WaitForEndOfFrame();
        //        }
        //    }
        //}

        //foreach (GameObject g in toChangeColor) {
        //    SetColour(g.gameObject, _color);
        //}

        foreach (Transform t in segment.GetComponentInChildren<Transform>())
        {
            SetColour(t.gameObject, _color);

            foreach (Transform tt in t.GetComponentInChildren<Transform>())
            {
                SetColour(tt.gameObject, _color);

                foreach (Transform ttt in tt.GetComponentInChildren<Transform>())
                {
                    SetColour(ttt.gameObject, _color);

                    foreach (Transform tttt in ttt.GetComponentInChildren<Transform>())
                        SetColour(tttt.gameObject, _color);
                }
            }
        }
        yield return null;
    }

    void SetColour(GameObject _g, Color _c)
    {
        if (_g.GetComponent<MeshRenderer>() != null)
            _g.GetComponent<MeshRenderer>().material.SetColor("_Color", _c);

        if (_g.GetComponent<SkinnedMeshRenderer>() != null)
            _g.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", _c);
    }

    void SetAnimationOffset(GameObject _g, float _f)
    {
        if (_g.GetComponent<Animator>() != null)
            _g.GetComponent<Animator>().SetFloat("AnimationOffset", _f);

        foreach (Transform t in _g.GetComponentInChildren<Transform>())
        {
            if (t.GetComponent<Animator>() != null && t.GetComponent<Animator>().runtimeAnimatorController != null)
                if (t.GetComponent<Animator>().GetFloat("AnimationOffset") != null)
                    t.GetComponent<Animator>().SetFloat("AnimationOffset", _f);
        }
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