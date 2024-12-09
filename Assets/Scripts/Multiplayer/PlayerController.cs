using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Refernces for what a player object contains for other classes to use
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Tooltip("Refernce to the Player's controls")]
    public PlayerInput input;
    [Tooltip("Reference to the camera of this player")]
    public Camera myCam;
}
