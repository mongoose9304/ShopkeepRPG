using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerMaster : MonoBehaviour
{
    public List<BasicFollower> myFollowers = new List<BasicFollower>();
    private void OnDisable()
    {
        foreach(BasicFollower follower in myFollowers)
        {
            follower.gameObject.SetActive(false);
        }
    }
    
}
