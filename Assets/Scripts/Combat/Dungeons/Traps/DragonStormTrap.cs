using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonStormTrap : BasicTrap
{
    public float dragonSpawnCooldown;
    float dragonSpawnCooldownCurrent;
    public List<Transform> dragonSpawnsA = new List<Transform>();
    public List<Transform> dragonSpawnsB = new List<Transform>();
    //flip between spawns to attack horizontally and vertically
    private bool lastSpawnedA;
    private void StartAttack()
    {
        if(lastSpawnedA)
        {
            for(int i=0;i<damageColliders.Length;i++)
            {
                damageColliders[i].transform.parent.transform.position = dragonSpawnsA[i].position;
                damageColliders[i].transform.parent.transform.rotation = dragonSpawnsA[i].rotation;
                damageColliders[i].transform.parent.gameObject.SetActive(true);
            }
            lastSpawnedA = false;
        }
        else
        {
            for (int i = 0; i < damageColliders.Length; i++)
            {
                damageColliders[i].transform.parent.transform.position = dragonSpawnsB[i].position;
                damageColliders[i].transform.parent.transform.rotation = dragonSpawnsB[i].rotation;
                damageColliders[i].transform.parent.gameObject.SetActive(true);
            }
            lastSpawnedA = true;
        }
        dragonSpawnCooldownCurrent = dragonSpawnCooldown;
    }
    private void Update()
    {
        dragonSpawnCooldownCurrent -= Time.deltaTime;
        if(dragonSpawnCooldownCurrent <= 0)
        {
            StartAttack();
        }
    }
    private void OnEnable()
    {
         dragonSpawnCooldownCurrent = dragonSpawnCooldown;
    }
}
