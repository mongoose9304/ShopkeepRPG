using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningLevel : MonoBehaviour
{
    public Transform startLocation;
    public MiningLevel nextLocation;
    public Rock tunnelHolder;
    public GameObject nextLevelTunnel;
    public List<Rock> allRocks = new List<Rock>();
    private void Start()
    {
        GetAllRocks();
        DecideTunnelHolder();
    }
    public void CreateTunnel(Transform location_)
    {
        nextLevelTunnel.transform.position = location_.position - new Vector3(0, 0.5f, 0) ;
        nextLevelTunnel.SetActive(true);
        if(nextLocation)
        nextLevelTunnel.GetComponent<Tunnel>().teleportLocation = nextLocation.startLocation;
        tunnelHolder = null;
    }
    public void DecideTunnelHolder()
    {
        foreach(Rock rock in allRocks)
        {
            rock.SetTunnelHolder(false);
        }
        RemoveInactiveRocks();
      tunnelHolder= allRocks[Random.Range(0, allRocks.Count)];
      tunnelHolder.SetTunnelHolder(true,this);
    }
    private void GetAllRocks()
    {
        allRocks.Clear();
        allRocks.AddRange(gameObject.GetComponentsInChildren<Rock>());
    }
    private void RemoveInactiveRocks()
    {
        for(int i=0;i<allRocks.Count;i++)
        {
            if (allRocks[i].gameObject.activeInHierarchy == false)
                allRocks.RemoveAt(i);
        }
    }
}
