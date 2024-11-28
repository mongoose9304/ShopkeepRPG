using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObjectSlot : MonoBehaviour
{
    public bool isWindow;
    public MoveableObject placedObject;
    public GameObject worldObject;
    public bool inHell;
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
    public virtual void PlaceObject(MoveableObject object_)
    {
        if (object_)
        {
            placedObject = object_;
            SpawnPlacedObject();
        }
    }
    protected void SpawnPlacedObject()
    {
        if(placedObject)
        {
            worldObject = GameObject.Instantiate(placedObject.myPrefab, transform.position, transform.rotation);
            if(worldObject.GetComponentInChildren<Pedestal>())
            {
                worldObject.GetComponentInChildren<Pedestal>().inHell = inHell;
            }
            if (worldObject.GetComponentInChildren<BarginBin>())
            {
                worldObject.GetComponentInChildren<BarginBin>().inHell = inHell;
            }
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
    public void ClearItem()
    {
        if (worldObject)
        {
            Destroy(worldObject);
        }
        worldObject = null;
        placedObject = null;
    }
   

}
