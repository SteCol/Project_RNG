using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_AppendageSpawner : MonoBehaviour {
    void Awake() {
        GameObject appendage = Instantiate(_Storage.Storage().appendages[Random.Range(0, _Storage.Storage().appendages.Count)]);
        appendage.transform.parent = this.transform;
        appendage.transform.position = this.transform.position;
    }
}
