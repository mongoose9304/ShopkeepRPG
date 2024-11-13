using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObjectManager : MonoBehaviour
{
    public static MoveableObjectManager instance;
    public List<MoveableObjectSlot> allSlots = new List<MoveableObjectSlot>();
    public List<MoveableObjectSlot> humanSlots = new List<MoveableObjectSlot>();
    public List<MoveableObjectSlot> hellSlots = new List<MoveableObjectSlot>();
    private void Awake()
    {
        instance = this;
        LoadAllSlots();
    }

    public void SaveAllSlots()
    {
        List<MoveableObject> masterItemList_ = new List<MoveableObject>();
        for (int i = 0; i < allSlots.Count; i++)
        {
            MoveableObject item_ = new MoveableObject();
            if (allSlots[i].placedObject)
            {
                item_ = allSlots[i].placedObject;
                masterItemList_.Add(item_);
            }
            else
            {
                masterItemList_.Add(item_);
            }
           
        }
        FileHandler.SaveToJSON(masterItemList_, "MoveableObjectInventory");
    }
    private void LoadAllSlots()
    {
        List<MoveableObject> masterItemList_ = FileHandler.ReadListFromJSON<MoveableObject>("MoveableObjectInventory");
        if (masterItemList_ != null)
        {
            for (int i = 0; i < masterItemList_.Count; i++)
            {
                if (masterItemList_[i]!=null)
                {
                    allSlots[i].InitObject(masterItemList_[i]);
                }
            }
        }
    }
}
