using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Spawner : MonoBehaviour
{

    public GameObject obj;
    public int amount;

    // Use this for initialization
    void Start()
    {

        amount = _Storage.RNG(3, 9);
        float angle = 360 / amount;
        for (int i = 0; i < amount; i++)
        {
            print ("Geenratring");
            GameObject objClone = Instantiate(obj, this.transform);
            objClone.transform.position = this.transform.position;
            objClone.transform.localEulerAngles = new Vector3(0,0,(angle*(i+1)));
            
        }
    }
}
