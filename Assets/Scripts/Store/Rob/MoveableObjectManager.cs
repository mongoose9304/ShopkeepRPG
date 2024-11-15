using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObjectManager : MonoBehaviour
{
    public static MoveableObjectManager instance;
    public List<MoveableObject> masterItemList = new List<MoveableObject>();
    public List<MoveableObjectSlot> allSlots = new List<MoveableObjectSlot>();
    public List<MoveableObjectSlot> humanSlots = new List<MoveableObjectSlot>();
    public List<MoveableObjectSlot> hellSlots = new List<MoveableObjectSlot>();
    private void Awake()
    {
        instance = this;
        GetMasterList();
        LoadAllSlots();
    }

    public void SaveAllSlots()
    {
        List<string> masterItemList_ = new List<string>();
        for (int i = 0; i < allSlots.Count; i++)
        {
            if (allSlots[i].placedObject)
            {
                masterItemList_.Add(MoveableObjectIndex.instance.GetItemIndex(allSlots[i].placedObject));
            }
            else
            {
                masterItemList_.Add("");
            }
           
        }
        FileHandler.SaveToJSON(masterItemList_, "MoveableObjectInventory");
    }
    private void LoadAllSlots()
    {
        
        if (masterItemList != null)
        {
            for (int i = 0; i < masterItemList.Count; i++)
            {
                if (masterItemList[i]!=null)
                {
                    Debug.Log("FoundMasterListItem");
                    allSlots[i].InitObject(masterItemList[i]);
                }
                else
                {
                    allSlots[i].ClearItem();
                }
            }
        }
       
    }
    private void GetMasterList()
    {
        List<string> masterItemList_ = FileHandler.ReadListFromJSON<string>("MoveableObjectInventory");
        if(masterItemList_!=null)
        {
            if (masterItemList_.Count == 0)
                return;
            for (int i = 0; i < masterItemList_.Count; i++)
            {
                if(masterItemList_[i]=="")
                {
                    masterItemList.Add(null);
                }
                else
                {

                    MoveableObject obj = MoveableObjectIndex.instance.GetItemFromIndex(masterItemList_[i]);
                    if(obj)
                    {
                        masterItemList.Add(obj);
                    }
                    else
                    {
                        masterItemList.Add(null);
                    }
                }
            }
        }
    }
}
