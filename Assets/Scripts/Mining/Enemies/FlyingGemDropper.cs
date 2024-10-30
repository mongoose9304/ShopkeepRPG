using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class FlyingGemDropper : FlyingBombDropper
{
    public List<MMMiniObjectPooler> myDropPools = new List<MMMiniObjectPooler>();
    public int maxDrops;
    int currentDrops;
    public GameObject deathEffect;
    protected override void DropBomb()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 10, tileLayer))
        {

            if (hit.collider.gameObject.TryGetComponent<Tile>(out Tile tile))
            {
                if(currentDrops>=maxDrops)
                {
                    attackTime = maxAttackCooldown;
                    GameObject.Instantiate(deathEffect,transform.position,transform.rotation);
                    Death();
                    return;
                }
              GameObject obj=  myDropPools[Random.Range(0, myDropPools.Count)].GetPooledGameObject();
                obj.transform.position = visualBomb.transform.position;
                obj.transform.rotation = visualBomb.transform.rotation;
                obj.SetActive(true);
                attackTime = maxAttackCooldown;
                currentDrops += 1;
            }
        }

    }
}
