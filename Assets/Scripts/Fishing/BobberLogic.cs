using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobberLogic : MonoBehaviour
{
    public bool isActive = false;
    Material material;
    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }
    private void Update()
    {
        if (isActive == true)
        {
            Color c = material.color;
            c.a = 1.0f;
            material.color = c;
        }
        else
        {
            Color c = material.color;
            c.a = 0.4f;
            material.color = c;
        }
    }
}
