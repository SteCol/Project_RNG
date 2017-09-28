using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_BoneRotator : MonoBehaviour {
    public List<cls_Bone> bones;

	// Use this for initialization
	void Start () {
        foreach (cls_Bone b in bones) {
            int i = _Storage.RNG(0, 45);
            //b.boneA.transform.eulerAngles = new Vector3(i, 0, 0);
            //b.boneB.transform.eulerAngles = new Vector3(90-i, 0, 0);

            b.boneA.transform.Rotate(i, 0, 0);
            b.boneB.transform.Rotate(i, 0, 0);

        }
    }
}

[System.Serializable]
public class cls_Bone {
    public GameObject boneA, boneB;
}
