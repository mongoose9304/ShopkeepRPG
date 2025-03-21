using Shopkeeper;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //Actions
    [Header("Actions")]
    [SerializeField] CharacterAction walking;
    
    Shopkeeper.CharacterController controller;
    InputActionMap inputMap;
    
    private void Start() {
        controller = GetComponent<Shopkeeper.CharacterController>();
        inputMap = GetComponent<PlayerInput>().currentActionMap;
        MapInput();
    }

    void MapInput() {
        if(controller == null) return;
        if(inputMap == null) return;

        //map the input here
    }


    public void EnableInput(InputAction input, bool b) {
        if (b) { input.Enable(); } 
        else { input.Disable(); }
    }

    public void WalkAction(InputAction.CallbackContext context) {
        walking.inputContext = context;
        controller.ChangeAction(walking);
    }
}
