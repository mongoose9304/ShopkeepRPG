using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPickupManager : MonoBehaviour
{
    public static CombatPickupManager instance;
    public MMMiniObjectPooler healthPool;
    public MMMiniObjectPooler manaPool;
    //percent
    public float healAmount;
    public float manaAmount;
    public float healSpawnChance;
    public float manaSpawnChance;
    public float PlayerHeight;
    private void Start()
    {
        instance = this;
    }
    public void ClearPickups()
    {
        healthPool.ResetAllObjects();
        manaPool.ResetAllObjects();
    }
    public void TryForHealthPickup(Transform transform_)
    {
        if(Random.Range(0,100)<healSpawnChance)
        {
            CreateHealthPickup(transform_);
        }
    }
    public void TryForManaPickup(Transform transform_)
    {
        if (Random.Range(0, 100) < manaSpawnChance)
        {
            CreateManaPickup(transform_);
        }
    }
    public void CreateHealthPickup(Transform transform_)
    {
        GameObject obj = healthPool.GetPooledGameObject();
        obj.transform.position = new Vector3(transform_.position.x, PlayerHeight, transform_.position.z);
        obj.GetComponent<HealthPickup>().EnablePickUp(healAmount);
    }
    public void CreateManaPickup(Transform transform_)
    {
        GameObject obj = manaPool.GetPooledGameObject();
        obj.transform.position = new Vector3(transform_.position.x, PlayerHeight, transform_.position.z);
        obj.GetComponent<ManaPickup>().EnablePickUp(manaAmount);
    }
}
