using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobberLogic : MonoBehaviour
{
    public bool isActive = false;
    Material material;
    public Vector3 targetPosition;
    public Vector3 startPosition;
    public float t;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }
    private void Update()
    {

        if (isActive == true)
        {
            if (t < 1.0f)
            {
                t += Time.deltaTime;
            }
            else
            {
                t = 1.0f;
            }

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.position = new Vector3(transform.position.x, -0.5f + 3.0f - (Mathf.Abs(t - 0.5f) * 6.0f), transform.position.z);
        }

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
