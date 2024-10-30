using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPiece : MonoBehaviour
{
    bool canDrop;
    [SerializeField] bool isHead;
    [SerializeField] Material rockMat;
    [SerializeField] Material gemMat;
    [SerializeField] float damage;
    [SerializeField] bool isBoss;
    [SerializeField] GameObject mineableObject;
    [SerializeField] AudioClip attackAudio;
    private void OnTriggerEnter(Collider other)
    {
        if(isBoss)
        {
            if (other.tag == "Explosion")
            {
                gameObject.SetActive(false);
                GetComponentInParent<TumbleSection>().CheckObjects();

            }
            return;
        }
        if (!canDrop)
        {
            if (other.tag == "Explosion")
            {
                gameObject.SetActive(false);
                if(isHead)
                {
                    TumbleTowerEnemy enemy = GetComponentInParent<TumbleTowerEnemy>();
                    enemy.Death();
                  
                }
             
            }
        }
        else
        {
            if (other.tag == "Explosion")
            {
                gameObject.SetActive(false);
                if (isHead)
                {
                    GetComponentInParent<TumbleTowerEnemy>().TakeDamage(1);
                }
              
            }
            if (other.tag == "Pickaxe")
            {
                gameObject.SetActive(false);
             
            }
        }  
        if(other.tag=="Player")
        {
            other.gameObject.GetComponent<MiningPlayer>().TakeDamage(damage);
            if (attackAudio)
                MMSoundManager.Instance.PlaySound(attackAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
            false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
            1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
        }
    }
    public void SetTowerPiece(bool canDrop_)
    {
        if (isHead)
            return;
        canDrop = canDrop_;;
        if(canDrop)
        {
        GetComponent<MeshRenderer>().material = gemMat;
            if(mineableObject)
            mineableObject.SetActive(true);
        }
        else
        {
        GetComponent<MeshRenderer>().material = rockMat;
        gameObject.tag = "Untagged";
            if (mineableObject)
                mineableObject.SetActive(false);
        }
    }
}
