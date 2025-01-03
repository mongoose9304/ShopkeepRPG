using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// The mostly virtual class for a familiars attacks and behavior that change between familiars
/// </summary>
public class FamiliarCombatControls : MonoBehaviour
{
    [Tooltip("Can the familliar take damage right now")]
    public bool damageImmune;
    [Tooltip("Is the familiar performing an action that would prevent them from acting or moving")]
    public bool isBusy;
    [Tooltip("Is my rotation being controlled by an action")]
    public bool isControllingRotation;
    [Tooltip("Is the main player and the familiar performing an action that would prevent them from acting or moving")]
    public bool bothPlayersBusy;
    [Tooltip("How much damage will my melee attack do")]
    public float meleeDamage;
    [Tooltip("How much damage will my ranged attack do")]
    public float rangedDamage;
    [Tooltip("How much damage will my special A attack do")]
    public float specialADamage;
    [Tooltip("How much damage will my special B attack do")]
    public float specialBDamage;
    [Tooltip("How much damage will my ultimate attack do")]
    public float ultimateDamage;
    [Tooltip("REFERENCE to the effects played when the player is hit")]
    [SerializeField]public MMF_Player hitEffects;
    /// <summary>
    /// Set up my controls to perform attack actions
    /// </summary>
    public virtual void EnableActions(InputActionMap playerActionMap)
    {
    }
    /// <summary>
    /// Calculate the damage each of my attacks can do
    /// </summary>
    public virtual void CalculateDamage(float pAttack,float mAttack)
    {
    }
}
