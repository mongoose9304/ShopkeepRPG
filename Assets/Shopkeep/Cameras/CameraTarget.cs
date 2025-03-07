using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    private UInt32 layerMask;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = 0;
        AddLayer(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Layer index between 0 and 32
    public void AddLayer(int layer)
    {
        layerMask |= 1u << layer;
    }

    public void RemoveLayer(int layer)
    {
        layerMask ^= 1u << layer;
    }

    public bool CheckLayer(int layer)
    {
        return ((layerMask & (1u << layer)) > 0);
    }

    public UInt32 GetLayerMask()
    {
        return layerMask;
    }
}
