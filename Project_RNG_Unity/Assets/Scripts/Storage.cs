using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Storage : MonoBehaviour
{
    [Header("Controls")]
    public bool generateColors;

    [Header("Prefabs")]
    public List<GameObject> appendages;
    public List<Color> colors;

    [Header("RNG stuff")]
    public System.Random pseudoRandom;
    private string seed;

    void Awake()
    {
        seed = GetComponent<scr_Generation>().seed;
        pseudoRandom = new System.Random(seed.GetHashCode());
    }

    void Update() {
        if (generateColors) {
            GenerateColors(colors.Count);
            generateColors = false;
        }
    }

    void GenerateColors(int _amount) {
        colors.Clear();
        for (int i = 0; i < _amount; i++) {
            colors.Add(new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
        }
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