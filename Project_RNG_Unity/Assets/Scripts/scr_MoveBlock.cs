using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_MoveBlock : MonoBehaviour {

	void Update () {
        this.transform.Translate(0, 0, 1.5f);
        this.transform.Rotate(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal"));
	}
}
