using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;


/// <summary>
/// "CharacterController" is already tacken
/// SO naming it "CharacterController_"
/// </summary>
public class CharacterController_ : MonoBehaviour
{
    //-Component attached to characters.
    //-Contains the current action.
    //-Contains event dispatched when the character starts and stops performing an action.
    //-Contains function to start executing an action.Higher priority actions interrupt lower priority actions.

    CharacterAction currentAction;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
