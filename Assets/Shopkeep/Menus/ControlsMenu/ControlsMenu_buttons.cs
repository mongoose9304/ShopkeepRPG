using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class ControlsMenu_buttons : MonoBehaviour
{
  
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Btn_Attack1() {
       
    }


    public void RemapButtonClicked(InputAction action) {
        Debug.Log(action.GetBindingDisplayString());
        var rebindoperation = action.PerformInteractiveRebinding().Start();
        Debug.Log(action.GetBindingDisplayString());
    }
}
