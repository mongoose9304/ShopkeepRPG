using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerMaster : MonoBehaviour
{
    public List<BasicFollower> myFollowers = new List<BasicFollower>();
    public List<GameObject> myTargets = new List<GameObject>();
    public float maxTimeBeforeTargetSearchs;
    public float currentTimeBeforeTargetSearchs;
    public float SearchRange;
    public List<string> searchableTags=new List<string>();
    public List<string> teams=new List<string>();
    protected virtual void OnDisable()
    {
        foreach(BasicFollower follower in myFollowers)
        {
            follower.gameObject.SetActive(false);
        }
    }
    protected virtual void Update()
    {
        SearchForTargetsUpdate();
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
    }


}
