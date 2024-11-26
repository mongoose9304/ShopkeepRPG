using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatCoopFamiliar : MonoBehaviour
{
    [Header("Inputs")]
    public InputActionMap playerActionMap;
    private InputAction movement;
    Vector3 moveInput;
    Vector3 newInput;
    public float moveSpeed;
    public float moveSpeedModifier;
    public LayerMask wallMask;
    public void SetUpControls(PlayerInput myInput)
    {
        playerActionMap = myInput.actions.FindActionMap("Player");
        movement = playerActionMap.FindAction("Movement");
    }
    private void OnDisable()
    {
        playerActionMap.Disable();
    }
    private void Update()
    {
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
}
