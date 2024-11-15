using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveableObjectReference
{
    public string myIndex;
    public MoveableObject myObject;
}
public class MoveableObjectIndex : MonoBehaviour
{
   public static MoveableObjectIndex instance;
    public List<MoveableObjectReference> allMoveableObjects = new List<MoveableObjectReference>();
    private void Awake()
    {
        if(!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public string GetItemIndex(MoveableObject obj_)
    {
        foreach(MoveableObjectReference obj in allMoveableObjects)
        {
            if (obj.myObject == obj_)
                return obj.myIndex;
        }

        return "";
    }
    public MoveableObject GetItemFromIndex(string index)
    {
        foreach (MoveableObjectReference obj in allMoveableObjects)
        {
            if (obj.myIndex == index)
                return obj.myObject;
        }
       return null;
    }
}
