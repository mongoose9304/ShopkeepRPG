using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

class Spell {
    public void Cast() {

    }
}


public class Player : MonoBehaviour
{
    CharacterController controller;
    InputActionMap inputMap;

    Spell spellSlot1;
    Spell spellSlot2;

    private void Start() {
        controller = GetComponent<CharacterController>();
        inputMap = GetComponent<PlayerInput>().currentActionMap;

        inputMap.FindAction("XAction").performed += CastSpell1;
        inputMap.FindAction("YAction").performed += CastSpell2;
        inputMap.FindAction("AAction").performed += Attack;
    }

    void MapInput(string action, System.Action<UnityEngine.InputSystem.InputAction.CallbackContext> onPerformed) {
        if (controller == null) return;
        if (inputMap == null) return;

        //map the input here
        inputMap.FindAction(action).performed += onPerformed;
    }


    public void EnableInput(InputAction input, bool b) {
        if (b) { input.Enable(); } 
        else { input.Disable(); }
    }

    private void Attack(InputAction.CallbackContext cont)
    {

    }

    private void CastSpell1(InputAction.CallbackContext cont)
    {
        if (spellSlot1 != null)
        {
            spellSlot1.Cast();
        }
    }

    private void CastSpell2(InputAction.CallbackContext cont)
    {
        if (spellSlot2 != null)
        {
            spellSlot2.Cast();
        }
    }
}
