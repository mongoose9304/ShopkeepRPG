using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonTotem : PlayerSpecialAttack
{
    public Totem myTotem;
    public float damageMulti=2;
    [SerializeField] private GameObject totemPrefab;
    public override void OnPress(GameObject obj_)
    {
        myTotem.gameObject.SetActive(false);
        myTotem.transform.position = transform.position;
        myTotem.gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        if(!myTotem)
        {
            myTotem= GameObject.Instantiate(totemPrefab).GetComponent<Totem>();
            myTotem.gameObject.SetActive(false);
        }
    }
    public override void CalculateDamage(float PATK, float MATK)
    {
        baseDamage = MATK * damageMulti;
        myTotem.damgeCollider.damage = baseDamage;
    }
}
