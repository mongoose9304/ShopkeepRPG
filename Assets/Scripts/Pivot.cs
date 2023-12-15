using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour
{

    public Transform pivotPoint;
    public float rSpeed = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pivotPoint != null){
            float rAngle = rSpeed * Time.deltaTime;

            transform.RotateAround(pivotPoint.position, Vector3.up, rAngle);
        }
        else {
            Debug.LogError("Pivot Point is null");
        }
    }
}
