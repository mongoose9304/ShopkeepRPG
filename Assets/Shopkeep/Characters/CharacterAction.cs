using Shopkeeper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CharacterAction : ScriptableObject
{
    //-Scriptable object. [done]
    //-Contains priority of action. [done]
    //-Contains abstract functions that are executed when the action starts and stops.Returns true if action can be started or stopped. [done?]
    //-Contains abstract coroutine function that is for the duration of the action. [done]
    //ACTIONS: stunned, knocked back, attack, dash, run, walk
    //They all will have their own implementation in separate files and in THIS file we will just call them.

    public int actionPriority = 0;
    [HideInInspector] public InputAction.CallbackContext inputContext;
    [HideInInspector] public bool completed = false;

    public abstract bool Enter(GameObject character);
    public abstract bool Exit(GameObject character);
    public abstract IEnumerator actionDuration(GameObject character);
}


//Here we set the functioonality for each action, but we will be calling the co routine in the Character controller
//public class Attack : CharacterAction{
//    float update = 5.0f;
//    public override void Start()
//    { 
//        setActionPriority(1);
//        return true; 
//    }
//    public override void Exit()
//    {

//        return true;
//    }

//    //This is our Coroutine|Update
//    public override IEnumerator actionDuration()
//    {
//        //whatever we want the attack to do
//        yield return new WaitForSeconds(update);
//    }
//}

//public class Walking : CharacterAction
//{
//    Shopkeeper.CharacterWalking walkingAction;
//    Vector3 moveDirection = Vector3.zero;
//    float actionLength = 0.5f;

//    public override void Start()
//    {
//        walkingAction.moveDirection = moveDirection;
//        setActionPriority(0);
//        return true;
//    }
//    public override void Exit()
//    {
//        walkingAction.moveDirection = Vector3.zero;
//        return true;
//    }

//    //This is our Coroutine|Update
//    public override IEnumerator actionDuration()
//    {
//        //whatever we want the attack to do
//        yield return new WaitForSeconds(actionLength);
//    }
//}