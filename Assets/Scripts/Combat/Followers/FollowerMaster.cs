using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.AI;

public class FollowerMaster : MonoBehaviour
{
    public MMMiniObjectPooler deathEffectPool;
    public MMMiniObjectPooler spawnEffectPool;
    public GameObject followerPrefab;
    public int maxFollowers;
    public List<BasicFollower> myFollowers = new List<BasicFollower>();
    public List<GameObject> myTargets = new List<GameObject>();
    public float maxTimeBeforeTargetSearchs;
    public float currentTimeBeforeTargetSearchs;
    public float SearchRange;
    public List<string> searchableTags=new List<string>();
    public List<string> teams=new List<string>();
    public float maxTimeBetweenRespawns;
    public float currentTimeBetweenRespawns;
    bool isQuitting;
    public float maxHealth;
    public float regularDamage;
    public float specialDamage;
    public float physicalDef;
    public float mysticalDef;


    protected virtual void Awake()
    {
        if(deathEffectPool)
        {
            deathEffectPool.FillObjectPool();
        }
        if (spawnEffectPool)
        {
            spawnEffectPool.FillObjectPool();
        }
        Reset();
    }
    public virtual void Reset()
    {
        for (int i = 0; i < myFollowers.Count; i++)
        {
            Destroy(myFollowers[i].gameObject);
        }
        myFollowers.Clear();
            for (int i = 0; i < maxFollowers; i++)
        {
            GameObject obj = GameObject.Instantiate(followerPrefab);
            obj.SetActive(false);
            myFollowers.Add(obj.GetComponent<BasicFollower>());
            obj.GetComponent<BasicFollower>().myMaster = this;
        }
    }
    protected virtual void OnDisable()
    {
        if (isQuitting)
            return;
        foreach(BasicFollower follower in myFollowers)
        {
            follower.gameObject.SetActive(false);
        }
    }
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }
    protected virtual void Update()
    {
        SearchForTargetsUpdate();
        SpawnFollowersUpdate();
    }
    public virtual void SearchForTargetsUpdate()
    {
        if(currentTimeBeforeTargetSearchs>0)
        {
            currentTimeBeforeTargetSearchs -= Time.deltaTime;
            return;
        }
        currentTimeBeforeTargetSearchs = maxTimeBeforeTargetSearchs;
        SearchForTargets();
        
    }
    public virtual void SpawnFollowersUpdate()
    {
        if (currentTimeBetweenRespawns > 0)
        {
            currentTimeBetweenRespawns -= Time.deltaTime;
            return;
        }
        currentTimeBetweenRespawns = maxTimeBetweenRespawns;
        SpawnFollowers();

    }
    public virtual void SpawnFollowers()
    {
        for (int i = 0; i < myFollowers.Count; i++)
        {
            if(!myFollowers[i].gameObject.activeInHierarchy)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(transform.position + new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f)), out hit, 6.0f, NavMesh.AllAreas))
                {
                    myFollowers[i].transform.position = hit.position;
                }
                else
                {
                myFollowers[i].transform.position = transform.position;
                }
                myFollowers[i].SetStats(maxHealth, regularDamage, specialDamage,physicalDef,mysticalDef);
                myFollowers[i].gameObject.SetActive(true);
                if(spawnEffectPool)
                {
                    GameObject obj = spawnEffectPool.GetPooledGameObject();
                    obj.transform.position = myFollowers[i].transform.position;
                    obj.SetActive(true);
                }
                return;
            }
        }
    }
    public virtual void SearchForTargets()
    {
        myTargets.Clear();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, SearchRange);
        foreach (var hitCollider in hitColliders)
        {
            foreach(string s in searchableTags)
            {
                if (hitCollider.tag == s)
                {
                    myTargets.Add(hitCollider.gameObject);
                }
            }
        }
        for (int i = 0; i < myTargets.Count; i++)
        {
            if(myTargets[i].TryGetComponent<TeamUser>(out TeamUser t_))
            {
                foreach(string s_ in teams)
                {
                    if (t_.myTeam == s_)
                    {
                        myTargets.RemoveAt(i);
                    }
                }
            }
        }
        if(myTargets.Count>0)
        {
            for (int i = 0; i < myFollowers.Count; i++)
            {
                if (myFollowers[i].target==null)
                myFollowers[i].target = myTargets[Random.Range(0, myTargets.Count)];
            }
        }
    }
    public virtual void FollowerDeath(Transform location_)
    {
        if(deathEffectPool)
        {
            GameObject obj = deathEffectPool.GetPooledGameObject();
            obj.transform.position = location_.position;
            obj.SetActive(true);
        }
    }
    public virtual void SetStats(float health,float damage,float specialDamage_,float physicalDef_,float mysticalDef_)
    {
        
    }
    public GameObject TryToGetNewTarget()
    {
        if(myTargets.Count>0)
        {
            return myTargets[Random.Range(0, myTargets.Count)];
        }
        else
        {
            return null;
        }
    }

}
