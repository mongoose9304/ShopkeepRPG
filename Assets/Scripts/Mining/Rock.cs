using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The objects that can be destroyed by bombs, will drop stone upon destruction
/// </summary>
public class Rock : MonoBehaviour
{
    private bool holdingTunel=false;
    public int stoneAmount = 1;
    MiningLevel myLevel;
    /// <summary>
    /// Set this rock as the rock that holds the tunnel to the next level
    /// </summary>
    public void SetTunnelHolder(bool isHolder_,MiningLevel level_=null)
    {
        holdingTunel = isHolder_;
        myLevel = level_;
    }

    private void OnDisable()
    {
        if (holdingTunel)
        {
            myLevel.CreateTunnel(transform);
            holdingTunel = false;
        }
        if(MiningManager.instance)
        {
            MiningManager.instance.SpawnStone(transform);
        }
    }

}
