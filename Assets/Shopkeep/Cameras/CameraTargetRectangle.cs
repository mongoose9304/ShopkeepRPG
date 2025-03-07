using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CameraRectangle {
    public float x, y, width, height;
    public float averageX, averageY;

    public CameraRectangle(float _x, float _y, float _width, float _height)
    {
        x = _x;
        y = _y;
        width = _width;
        height = _height;
        averageX = 0.0f;
        averageY = 0.0f;
    }

    public CameraRectangle(float _x, float _y, float _width, float _height, float xAverage, float yAverage)
    {
        x = _x;
        y = _y;
        width = _width;
        height = _height;
        averageX = xAverage;
        averageY = yAverage;
    }
}

public class CameraTargetRectangle : MonoBehaviour
{
    private Vector2 padding;
    private UInt32 targetLayersMask;
    [SerializeField]
    private float damping;
    [SerializeField]
    private float minimumZoom;
    [SerializeField]
    private float maximumZoom;


    // Start is called before the first frame update
    void Start()
    {
        AddLayer(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddLayer(int layer)
    {
        targetLayersMask |= 1u << layer;
    }

    public void RemoveLayer(int layer)
    {
        targetLayersMask ^= 1u << layer;
    }

    public CameraRectangle GetTargetRectangle()
    {
        CameraTarget[] cameraTargets = FindObjectsOfType<CameraTarget>();
        Debug.Log("Found " + cameraTargets.Length + " targets");
        if (cameraTargets.Length >= 2)
        {
            // Need at least 2 targets for the rectangle method to work. Will handle the special case of 1 in the else
            float xMin = float.MaxValue, xMax = float.MinValue;
            float zMin = float.MaxValue, zMax = float.MinValue;
            float totalX = 0.0f;
            float totalZ = 0.0f;

            for (int i = 0; i < cameraTargets.Length; ++i)
            {
                if ((cameraTargets[i].GetLayerMask() & targetLayersMask) > 0)
                {
                    totalX += cameraTargets[i].transform.position.x;
                    totalZ += cameraTargets[i].transform.position.z;
                    // The object matches at least 1 of the camera's layers
                    if (cameraTargets[i].transform.position.x < xMin)
                    {
                        xMin = cameraTargets[i].transform.position.x;
                    }
                    if (cameraTargets[i].transform.position.x > xMax)
                    {
                        xMax = cameraTargets[i].transform.position.x;
                    }

                    if (cameraTargets[i].transform.position.z < zMin)
                    {
                        zMin = cameraTargets[i].transform.position.z;
                    }
                    if (cameraTargets[i].transform.position.z > zMax)
                    {
                        zMax = cameraTargets[i].transform.position.z;
                    }
                }
            }

            float avgX = totalX / cameraTargets.Length;
            float avgZ = totalZ / cameraTargets.Length;

            float width = xMax - xMin;
            float height = zMax - zMin;

            return new CameraRectangle(xMin - padding.x, zMin - padding.y, width + 2 * padding.x, height + 2 * padding.y, avgX, avgZ);
        }

        else if (cameraTargets.Length > 0)
        {
            Transform tc = cameraTargets[0].gameObject.transform;
            // Just return a rectangle of some default size centered on the one target
            return new CameraRectangle(tc.position.x - 10.0f, tc.position.z - 10.0f, 50.0f, 50.0f, tc.position.x, tc.position.z);
        }
        else
        {
            // If you get here something is really fucked
            return new CameraRectangle(0.0f, 0.0f, 0.0f, 0.0f);
        }
    }
}
