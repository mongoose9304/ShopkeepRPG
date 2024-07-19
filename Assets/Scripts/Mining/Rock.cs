using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private bool holdingTunel=false;
    MiningLevel myLevel;
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
    }
}
