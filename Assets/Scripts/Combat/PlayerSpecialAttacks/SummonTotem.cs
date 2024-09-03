using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class SummonTotem : PlayerSpecialAttack
{
    public Totem myTotem;
    public float damageMulti=2;
    [SerializeField] MMMiniObjectPooler totemPool;
    public override void OnPress(GameObject obj_)
    {
        myTotem = totemPool.GetPooledGameObject().GetComponent<Totem>() ;
        myTotem.transform.position = transform.position;
        myTotem.damgeCollider.damage = baseDamage;
        myTotem.gameObject.SetActive(true);
        hiddenTimerCurrent = hiddenTimerMax;
        currentCharges -= 1;
    }
    public override void CalculateDamage(float PATK, float MATK)
    {
        baseDamage = MATK * damageMulti;
        
    }
    private void Update()
    {
        if(hiddenTimerCurrent>0)
        hiddenTimerCurrent -= Time.deltaTime;
    }
    public override bool CanBeUsed()
    {
        if (hiddenTimerCurrent <= 0)
            return true;
        else
            return false;
    }

}
