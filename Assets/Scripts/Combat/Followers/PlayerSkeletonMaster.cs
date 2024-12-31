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
    [SerializeField] float skeletonHealthPerLevel;
    [SerializeField] float skeletonPhysicalDamagePerLevel;
    [SerializeField] float skeletonMysticalDamagePerLevel;
    [SerializeField] float skeletonPhysicalDefencePerLevel;
    [SerializeField] float skeletonMysticalDefencePerLevel;

    [SerializeField] float skeletonBaseHealth;
    [SerializeField] float skeletonBasePhysicalDamage;
    [SerializeField] float skeletonBaseMysticalDamage;
    [SerializeField] float skeletonBasePhysicalDefence;
    [SerializeField] float skeletonBaseMysticalDefence;
   
    public override void Reset()
    {
        Debug.Log("Skeleton Mater Reset");
        base.Reset();
        for (int i = 0; i < mySuperSkeletons.Count; i++)
        {
            Destroy(mySuperSkeletons[i].gameObject);
        }
        mySuperSkeletons.Clear();
        for (int i = 0; i < maxSuperFollowers; i++)
        {
            GameObject obj = GameObject.Instantiate(superSkeleton);
            obj.SetActive(false);
            mySuperSkeletons.Add(obj.GetComponent<BasicFollower>());
            obj.GetComponent<BasicFollower>().myMaster = this;
        }
        SpawnFollowers();
        SpawnFollowers();
        SpawnFollowers();
        SpawnFollowers();
        SpawnFollowers();
    }
   
    public void SetStatsBasedOnPlayerLevel(int level_)
    {
        if (level_ < 1)
            level_ = 1;
        maxHealth = (skeletonHealthPerLevel * level_)+skeletonBaseHealth;
        regularDamage = (skeletonPhysicalDamagePerLevel * level_)+ skeletonBasePhysicalDamage;
        specialDamage = (skeletonMysticalDamagePerLevel * level_)+skeletonBaseMysticalDamage;
        physicalDef = (skeletonPhysicalDefencePerLevel * level_)+skeletonBasePhysicalDefence;
        mysticalDef = (skeletonMysticalDefencePerLevel * level_)+skeletonBaseMysticalDefence;
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
                if (NavMesh.SamplePosition(transform.position + new Vector3(Random.Range(-1,1), 0, Random.Range(-1, 1)), out hit, 3.0f, NavMesh.AllAreas))
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
