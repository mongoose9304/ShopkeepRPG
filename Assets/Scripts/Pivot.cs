using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour
{

    public Transform pivotPoint;
    public float rSpeed = 2.5f;
    void Start()
    {
        
    }

    void Update()
    {
        // this script just spins around a point, i needed this for testing iso angles
        if (pivotPoint != null){
            float rAngle = rSpeed * Time.deltaTime;

            transform.RotateAround(pivotPoint.position, Vector3.up, rAngle);
        }
        else {
            Debug.LogError("Pivot Point is null");
        }
    }
}
