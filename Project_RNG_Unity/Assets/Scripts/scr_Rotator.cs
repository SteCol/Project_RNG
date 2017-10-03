using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Rotator : MonoBehaviour {

    public float speed;

    void Start() {
        speed = _Storage.RNG(-180, 180);
    }

	void Update () {
        this.transform.Rotate(Vector3.forward, speed * Time.deltaTime);
	}
}
