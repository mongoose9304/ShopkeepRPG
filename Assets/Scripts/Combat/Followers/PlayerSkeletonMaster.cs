using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerSkeletonMaster : FollowerMaster
{
    public float skeletonExplosionDamage;
    [Tooltip("Multiplies the power of the basic skeleton to create a stronger one")]
    public float superSkeletonPower;
    public GameObject superSkeleton;
    public int maxSuperFollowers;
    public List<BasicFollower> mySuperSkeletons = new List<BasicFollower>();

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < maxSuperFollowers; i++)
        {
            GameObject obj = GameObject.Instantiate(superSkeleton);
            obj.SetActive(false);
            mySuperSkeletons.Add(obj.GetComponent<BasicFollower>());
            obj.GetComponent<BasicFollower>().myMaster = this;
        }
    }
    public override void SetStats(float health, float damage, float specialDamage_, float physicalDef_, float mysticalDef_)
    {
         maxHealth=health*0.2f;
     regularDamage=damage*0.2f;
    specialDamage=specialDamage_*0.2f;
        physicalDef = physicalDef_*0.2f;
        mysticalDef = mysticalDef_ * 0.2f;
        skeletonExplosionDamage = specialDamage * 3;
     }
    public override void FollowerDeath(Transform location_)
    {
        if (deathEffectPool)
        {
            GameObject obj = deathEffectPool.GetPooledGameObject();
            obj.transform.position = location_.position;
            obj.GetComponent<ProjectileExplosion>().damage = skeletonExplosionDamage;
            obj.SetActive(true);
        }
    }
    public override void SearchForTargets()
    {
        base.SearchForTargets();
        if (myTargets.Count > 0)
        {
            for (int i = 0; i < mySuperSkeletons.Count; i++)
            {
                if (mySuperSkeletons[i].target == null)
                    mySuperSkeletons[i].target = myTargets[Random.Range(0, myTargets.Count)];
            }
        }
    }
    public override void SpawnFollowers()
    {
        for (int i = 0; i < mySuperSkeletons.Count; i++)
        {
            if (!mySuperSkeletons[i].gameObject.activeInHierarchy)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(transform.position + new Vector3(2, 0, 0), out hit, 3.0f, NavMesh.AllAreas))
                {
                    mySuperSkeletons[i].transform.position = hit.position;
                }
                else
                {
                    mySuperSkeletons[i].transform.position = transform.position;
                }
                mySuperSkeletons[i].SetStats(maxHealth* superSkeletonPower, regularDamage* superSkeletonPower, specialDamage* superSkeletonPower, physicalDef* superSkeletonPower, mysticalDef* superSkeletonPower);
                mySuperSkeletons[i].gameObject.SetActive(true);
                if (spawnEffectPool)
                {
                    GameObject obj = spawnEffectPool.GetPooledGameObject();
                    obj.transform.position = mySuperSkeletons[i].transform.position;
                    obj.SetActive(true);
                }
                break;
            }
        }
        base.SpawnFollowers();
    }
}
