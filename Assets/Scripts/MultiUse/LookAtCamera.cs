using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LookAtCamera : MonoBehaviour
{
    public bool LockX;
    public bool LockY;
    public bool LockZ;
    // Update is called once per frame
    void Update()
    {
        if (!LockX && !LockY && !LockZ)
        {
            transform.DOLookAt(Camera.main.transform.position, 0.0f, AxisConstraint.W, Vector3.up);
        }
        else if (LockX)
        {
            transform.DOLookAt(Camera.main.transform.position, 0.0f, AxisConstraint.X, Vector3.up);
        }
        else if (LockY)
            transform.DOLookAt(Camera.main.transform.position, 0.0f, AxisConstraint.Y, Vector3.up);
        else if (LockZ)
            transform.DOLookAt(Camera.main.transform.position, 0.0f, AxisConstraint.Z, Vector3.up);
    }
}
