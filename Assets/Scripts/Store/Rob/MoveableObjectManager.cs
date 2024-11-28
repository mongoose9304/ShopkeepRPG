using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the objects that can be moved and saves them to json files to be loaded later
/// </summary>
public class MoveableObjectManager : MonoBehaviour
{
    [Tooltip("Singleton instance of the class")]
    public static MoveableObjectManager instance;
    [Tooltip("The list of items the player ha stored")]
    public List<MoveableObject> masterItemList = new List<MoveableObject>();
    [Tooltip("REFERENCE to all the slots where items can go")]
    public List<MoveableObjectSlot> allSlots = new List<MoveableObjectSlot>();
    [Tooltip("REFERENCE to all the slots where items can go in the human world")]
    public List<MoveableObjectSlot> humanSlots = new List<MoveableObjectSlot>();
    [Tooltip("REFERENCE to all the slots where items can go in the hell world")]
    public List<MoveableObjectSlot> hellSlots = new List<MoveableObjectSlot>();
    private void Awake()
    {
        instance = this;
        GetMasterList();
        LoadAllSlots();
    }
    /// <summary>
    /// Save every slot to a Json file
    /// </summary>
    public void SaveAllSlots()
    {
        List<string> masterItemList_ = new List<string>();
        for (int i = 0; i < allSlots.Count; i++)
        {
            if (allSlots[i].placedObject)
            {
                masterItemList_.Add(allSlots[i].placedObject.myName);
            }
            else
            {
                masterItemList_.Add("");
            }
           
        }
        FileHandler.SaveToJSON(masterItemList_, "MoveableObjectInventory");

    }
    /// <summary>
    /// Init all slots based on the master list
    /// </summary>
    private void LoadAllSlots()
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
    /// <summary>
    /// Load all the slots from Json files. If none are found it will use whatever is in the inspector 
    /// </summary>
    private void GetMasterList()
    {
        masterItemList.Clear();
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
