using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class MMFAutoPlayer : MonoBehaviour
{
    public bool playOnEnable;
    public bool playOnStart;
    public MMF_Player player;
    private void Start()
    {
        if(playOnStart)
        {
            player.PlayFeedbacks();
        }
    }
    private void OnEnable()
    {
        if(playOnEnable)
        {
            player.PlayFeedbacks();
        }
    }
}
