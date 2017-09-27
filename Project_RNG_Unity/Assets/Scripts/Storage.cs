using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [Header("Prefabs")]
    public List<GameObject> appendages;

}

public class _Storage {
    public static Storage Storage() {
        return GameObject.FindGameObjectWithTag("GameController").GetComponent < Storage>();
    }
}