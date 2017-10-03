﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_MoveBlock : MonoBehaviour {

    public float turnSpeed;
    public float speed;

	void Update () {
        this.transform.Translate(0, 0, speed * Time.deltaTime);
        this.transform.Rotate(Input.GetAxis("Vertical")  * turnSpeed * Time.deltaTime, 0, -Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime);
	}
}
