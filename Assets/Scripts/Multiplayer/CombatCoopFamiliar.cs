using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatCoopFamiliar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public StatBlock monsterStats;

    [Header("Interactions")]
    [Tooltip("All the objects the player is currently in range to interact with")]
    public List<GameObject> myInteractableObjects = new List<GameObject>();
    [Tooltip("The object the player is currently locked onto")]
    [SerializeField] GameObject interactableObjectTarget;
    [Tooltip("REFERENCE to gameobject used to show what you are locked onto")]
    [SerializeField] GameObject interactableObjectLockOnObject;

    [Header("Inputs")]
    public InputActionMap playerActionMap;
    private InputAction movement;
    Vector3 moveInput;
    Vector3 newInput;
    public float moveSpeed;
    public float moveSpeedModifier;
    public LayerMask wallMask;
    private bool InteractHeld;
    public void SetUpControls(PlayerInput myInput)
    {
        playerActionMap = myInput.actions.FindActionMap("Player");
        movement = playerActionMap.FindAction("Movement");
        playerActionMap.FindAction("YAction").performed += InteractPressed;
        playerActionMap.FindAction("YAction").canceled += InteractReleased;
    }
    private void OnDisable()
    {
        playerActionMap.Disable();
    }
    private void Update()
    {
        GetClosestInteractableObject();
        if (InteractHeld)
            InteractAction();
        moveInput = new Vector3(movement.ReadValue<Vector2>().x, 0, movement.ReadValue<Vector2>().y);
        transform.position = transform.position + PreventFalling() * moveSpeed * moveSpeedModifier * Time.deltaTime;
        if (moveInput != Vector3.zero)
            transform.forward = moveInput;
    }
    private Vector3 PreventGoingThroughWalls(Vector3 temp_)
    {

        var dir = transform.TransformDirection(Vector3.down);
        newInput = temp_;
        // Up

        if (Physics.Raycast(transform.position + new Vector3(0f, 5.0f, -0.5f), dir, 15, wallMask))
            if (newInput.z < 0)
                newInput.z = 0;
        // Down
        if (Physics.Raycast(transform.position + new Vector3(0f, 5.0f, .5f), dir, 15, wallMask))
            if (newInput.z > 0)
                newInput.z = 0;
        //Left
        if (Physics.Raycast(transform.position + new Vector3(0.5f, 5.0f, 0f), dir, 15, wallMask))
            if (newInput.x > 0)
                newInput.x = 0;
        //Right
        if (Physics.Raycast(transform.position + new Vector3(-0.5f, 5.0f, 0f), dir, 15, wallMask))
            if (newInput.x < 0)
                newInput.x = 0;
        return newInput;


    }
    private Vector3 PreventFalling()
    {
        // Stop walking
        var dir = transform.TransformDirection(Vector3.down);
        newInput = moveInput;
        // Up

        if (!Physics.Raycast(transform.position - new Vector3(0f, 0f, 1), dir, 10))
            if (newInput.z < 0)
                newInput.z = 0;
        // Down
        if (!Physics.Raycast(transform.position - new Vector3(0f, 0f, -1), dir, 10))
            if (newInput.z > 0)
                newInput.z = 0;
        //Left
        if (!Physics.Raycast(transform.position - new Vector3(-1, 0f, 0f), dir, 10))
            if (newInput.x > 0)
                newInput.x = 0;
        //Right
        if (!Physics.Raycast(transform.position - new Vector3(1, 0f, 0f), dir, 10))
            if (newInput.x < 0)
                newInput.x = 0;
        return newInput;
    }
    /// <summary>
    /// Calculates the nearest interactable object and sets that as the interactable target that will be used for lock ons
    /// </summary>
    private void GetClosestInteractableObject()
    {
        if (myInteractableObjects.Count == 0)
        {
            interactableObjectTarget = null;
            interactableObjectLockOnObject.SetActive(false);
            return;
        }
        for (int i = 0; i < myInteractableObjects.Count; i++)
        {
            if (!myInteractableObjects[i].activeInHierarchy)
            {
                if (interactableObjectTarget == myInteractableObjects[i])
                    interactableObjectTarget = null;
                myInteractableObjects.RemoveAt(i);
                continue;
            }
            if (!interactableObjectTarget)
            {
                interactableObjectTarget = myInteractableObjects[i];
            }
            if (Vector3.Distance(transform.position, myInteractableObjects[i].transform.position) < Vector3.Distance(transform.position, interactableObjectTarget.transform.position))
                interactableObjectTarget = myInteractableObjects[i];
            interactableObjectLockOnObject.SetActive(true);
            interactableObjectLockOnObject.transform.position = interactableObjectTarget.transform.position;
        }
        foreach (GameObject obj in myInteractableObjects)
        {
            if (Vector3.Distance(transform.position, obj.transform.position) < Vector3.Distance(transform.position, interactableObjectTarget.transform.position))
                interactableObjectTarget = obj;
        }
    }
    /// <summary>
    /// The actions taken when the player presses the interact button
    /// </summary>
    private void InteractAction()
    {
        if (interactableObjectTarget)
        {
            if (interactableObjectTarget.TryGetComponent<InteractableObject>(out InteractableObject obj))
            {
                obj.Interact();
            }
        }
    }
    public void RemoveInteractableObject(GameObject obj_)
    {
        myInteractableObjects.Remove(obj_);
        if (interactableObjectTarget = obj_)
        {
            interactableObjectTarget = null;
            interactableObjectLockOnObject.SetActive(false);
        }
    }
    private void InteractPressed(InputAction.CallbackContext objdd)
    {
        InteractHeld = true;
    }
    private void InteractReleased(InputAction.CallbackContext objdd)
    {
        InteractHeld = false;
    }

}
