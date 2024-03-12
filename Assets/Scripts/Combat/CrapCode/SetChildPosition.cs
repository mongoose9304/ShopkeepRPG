using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChildPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = this.transform.GetChild(0).gameObject;
        obj.transform.position = this.transform.position;
    }

   
}
