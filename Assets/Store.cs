using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cost
{
    ItemData item;
    int amount;
}

public struct StoreItem
{
    ItemData item;
    List<Cost> costs;
}

public class Store : MonoBehaviour
{
    public List<StoreItem> recipes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
