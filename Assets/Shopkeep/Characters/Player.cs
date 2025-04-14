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
    [SerializeField] CharacterAction meleeAction;
    [SerializeField] CharacterAction spell1Action;
    [SerializeField] CharacterAction spell2Action;

    Shopkeeper.CharacterController controller;
    InputActionMap inputMap;

    //Spells
    [Header("Spell Reference")]
    public Melee melee;
    public Spell spell1;
    public Spell spell2;
    
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

    public void Melee(InputAction.CallbackContext context) {
        UseSpell spellAction = (UseSpell)meleeAction;

        if(spellAction == null) { return; }
        spellAction.spell = melee;
        spellAction.inputContext = context;

        controller.ChangeAction(spellAction);
    }

    public void Spell1Action(InputAction.CallbackContext context) {
        UseSpell spellAction = (UseSpell)spell1Action;
        if(spellAction == null) { return; }
        spellAction.spell = spell1;
        spellAction.inputContext = context;

        controller.ChangeAction(spellAction);
    }

    public void Spell2Action(InputAction.CallbackContext context) {
        UseSpell spellAction = (UseSpell)spell2Action;
        if (spellAction == null) { return; }
        spellAction.spell = spell2;
        spellAction.inputContext = context;

        controller.ChangeAction(spellAction);
    }
}
