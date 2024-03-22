using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialAttack : MonoBehaviour
{
    public float manaCost;
    public float maxCoolDown;
    public float baseDamage;
    public bool isBusy;
    public GameObject Player;

    public virtual void OnPress(GameObject obj_)
    {

    }
    
}
