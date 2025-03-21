 using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;


/// <summary>
/// "CharacterController" is already tacken
/// SO naming it "CharacterController_"
/// </summary>
/// 
namespace Shopkeeper {
    public class CharacterController : MonoBehaviour {
        //-Component attached to characters.
        //-Contains the current action.
        //-Contains event dispatched when the character starts and stops performing an action.
        //-Contains function to start executing an action.Higher priority actions interrupt lower priority actions.

        CharacterAction currentAction;
        Coroutine currentActionCoroutine;

        public void ChangeAction(CharacterAction newAction) {
            //if(newAction == currentAction) { return; }
            if (currentAction != null) {
                if (newAction.actionPriority < currentAction.actionPriority) {
                    return;
                }

                if (!currentAction.Exit(gameObject)) { return; }
                StopCoroutine(currentActionCoroutine);
            }

            currentAction = newAction;

            if (currentAction.Enter(gameObject)) {
                currentActionCoroutine = StartCoroutine(currentAction.actionDuration(gameObject));
            }

        }

        private void Update() {
            //Check if the current action has been completed and clears it
            if (currentAction != null) {
                if (currentAction.completed) {
                    currentAction = null;
                    currentActionCoroutine = null;
                }
            }
        }
    }
}