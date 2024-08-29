using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The mostly virtual class all player special attacks (interchangeable special moves)
/// </summary>
public class PlayerSpecialAttack : MonoBehaviour
{
    [Header("Referecnes")]
    public GameObject Player;
    public Element myElement;
    public bool isBusy;
    [Header("Stats")]
    public float manaCost;
    public float maxCoolDown;
    public float baseDamage;

    /// <summary>
    /// The behavior once the move is activated 
    /// </summary>
    public virtual void OnPress(GameObject obj_)
    {

    }
    public virtual void CalculateDamage(float PATK,float MATK)
    {

    }
    
}
