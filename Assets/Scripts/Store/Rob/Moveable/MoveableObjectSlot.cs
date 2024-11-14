using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObjectSlot : MonoBehaviour
{
    public bool hasObject;
    public bool isWindow;
    public MoveableObject placedObject;
    public GameObject worldObject;

    public void PickUpObject()
    {
        if(worldObject.GetComponentInChildren<Pedestal>())
        {
            Pedestal p = worldObject.GetComponentInChildren<Pedestal>();
            if (p.amount>0&&p.myItem!=null)
            {
                ShopManager.instance.invScreen.AddItemToInventory(p.myItem, p.amount);
            }
        }
        if (worldObject.GetComponentInChildren<BarginBin>())
        {
            BarginBin b = worldObject.GetComponentInChildren<BarginBin>();
            foreach(BarginBinSlot slot in b.binSlotsWithItems)
            {
                if(slot.amount>0&&slot.myItem)
                {
                    ShopManager.instance.invScreen.AddItemToInventory(slot.myItem, slot.amount);
                }
            }
        }
        if (worldObject)
        {
            Destroy(worldObject);
        }
       


        placedObject = null;
    }
    public void PlaceObject(MoveableObject object_)
    {
        if (object_)
        {
            placedObject = object_;
            SpawnPlacedObject();
        }
    }
    private void SpawnPlacedObject()
    {
        if(placedObject)
        {
            worldObject = GameObject.Instantiate(placedObject.myPrefab, transform.position, transform.rotation);
        }
    }
    public bool CheckForObject()
    {
        if(worldObject)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void InitObject(MoveableObject object_)
    {
        if(worldObject)
        {
            Destroy(worldObject);
        }
        PlaceObject(object_);

    }

}
