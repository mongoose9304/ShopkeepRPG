using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetRectangleFollower : MonoBehaviour
{
    [SerializeField]
    private CameraTargetRectangle rectangle;
    private float distance = 10.0f;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CameraRectangle rect = rectangle.GetTargetRectangle();
        float aspectRatio = 1.7777777f; // Force aspect ratio
        float absoluteWidth = 0.0f;
        float absoluteHeight = 0.0f;
        if (rect.width / 16.0f > rect.height / 9.0f)
        {
            absoluteWidth = rect.width;
            absoluteHeight = absoluteWidth / aspectRatio;
        }
        else
        {
            absoluteHeight = rect.height;
            absoluteWidth = rect.height * aspectRatio;
        }
        distance = absoluteWidth * 0.4f;

        transform.position = new Vector3(rect.averageX, 6.0f + distance * 0.5f, rect.averageY - 10.0f) + transform.forward * -distance;
    }
}
