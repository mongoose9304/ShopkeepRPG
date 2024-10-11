using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LookAtCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.DOLookAt(Camera.main.transform.position, 0.0f, AxisConstraint.X, Vector3.up);
    }
    private void OnEnable()
    {
        transform.DOLookAt(Camera.main.transform.position, 0.0f, AxisConstraint.None, Vector3.up);
    }
}
