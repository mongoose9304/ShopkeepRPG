using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    CharacterController controller;
    InputActionMap inputMap;

    private void Start() {
        controller = GetComponent<CharacterController>();
        inputMap = GetComponent<PlayerInput>().currentActionMap;
    }

    void MapInput() {
        if(controller == null) return;
        if(inputMap == null) return;

        //map the input here
        inputMap.FindAction("Action").performed += context => {
            
        };


    }


    public void EnableInput(InputAction input, bool b) {
        if (b) { input.Enable(); } 
        else { input.Disable(); }
    }
}
