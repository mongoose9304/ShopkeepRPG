using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SetInventorySelected : MonoBehaviour
{
    public InventoryUI inv;


    private void OnEnable()
    {
       if( inv.slots.Count>0)
        {
            if(UnityEngine.EventSystems.EventSystem.current)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(inv.slots[0].gameObject);
            }
        }
    }
}
