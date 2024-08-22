using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollowObject : MonoBehaviour
{
    [SerializeField] Transform followObject;
    private Vector3 velocity = Vector3.zero;
    public bool warpOnEnable;

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, followObject.position-offset, Time.deltaTime * 5);
        transform.position = Vector3.SmoothDamp(transform.position, followObject.position,ref velocity, 0.3f);
    }

}
