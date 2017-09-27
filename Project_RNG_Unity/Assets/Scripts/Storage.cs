using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [Header("Prefabs")]
    public List<GameObject> appendages;

    [Header("RNG stuff")]
    public System.Random pseudoRandom;
    private string seed;

    void Awake()
    {
        seed = GetComponent<scr_Generation>().seed;
        pseudoRandom = new System.Random(seed.GetHashCode());
    }

    public int RNG(int a, int b)
    {
        int i = pseudoRandom.Next(a, b);
        return i;
    }
}

public class _Storage
{
    public static Storage Storage()
    {
        return GameObject.FindGameObjectWithTag("GameController").GetComponent<Storage>();
    }

    public static int RNG(int a, int b)
    {
        int i = 0;

        i = Storage().pseudoRandom.Next(a, b);
        //i = Storage().

        return i;
    }
}