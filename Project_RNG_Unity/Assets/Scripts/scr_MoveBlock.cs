using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_MoveBlock : MonoBehaviour {

    public float turnSpeed;

	void Update () {
        this.transform.Translate(0, 0, 1.5f);
        this.transform.Rotate(Input.GetAxis("Vertical")  * turnSpeed, 0, -Input.GetAxis("Horizontal") * turnSpeed);
	}
}
