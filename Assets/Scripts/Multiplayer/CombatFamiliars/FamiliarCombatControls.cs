using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// The mostly virtual class for a familiars attacks and behavior that change between familiars
/// </summary>
public class FamiliarCombatControls : MonoBehaviour
{
    public bool damageImmune;
    public bool isBusy;
    public bool isControllingRotation;
    public bool bothPlayersBusy;
    public float meleeDamage;
    public float rangedDamage;
    public float specialADamage;
    public float specialBDamage;
    public float ultimateDamage;
    public virtual void EnableActions(InputActionMap playerActionMap)
    {
    }
    public virtual void CalculateDamage(float pAttack,float mAttack)
    {
    }
}
