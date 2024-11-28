using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveableObjectIndex : MonoBehaviour
{
   public static MoveableObjectIndex instance;
    public List<MoveableObject> allMoveableObjects = new List<MoveableObject>();
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
  /*  public string GetItemIndex(MoveableObject obj_)
    {
        foreach(MoveableObjectReference obj in allMoveableObjects)
        {
            if (obj.myObject == obj_)
                return obj.myIndex;
        }

        return "";
    }
  */
    public MoveableObject GetItemFromIndex(string index)
    {
        foreach (MoveableObject obj in allMoveableObjects)
        {
            if (obj.myName == index)
                return obj;
        }
       return null;
    }
}
