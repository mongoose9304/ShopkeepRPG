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
    public List<Tile> allTiles = new List<Tile>();
    public List<Wall> allWalls = new List<Wall>();
    public Material wallMat;
    public Material tileMatA;
    public Material tileMatB;
    private void Start()
    {
        GetAllRocks();
        GetAllTiles();
        GetAllWalls();
        SetMaterials();
        DecideTunnelHolder();
    }
    public void CreateTunnel(Transform location_)
    {
        nextLevelTunnel.transform.position = location_.position - new Vector3(0, 0.5f, 0);
        nextLevelTunnel.SetActive(true);
        if (nextLocation)
        {
            nextLevelTunnel.GetComponent<Tunnel>().teleportLocation = nextLocation.startLocation;
            nextLevelTunnel.GetComponent<Tunnel>().objectToSetActive = nextLocation.gameObject;
        }
        if(!nextLevelTunnel.GetComponent<Tunnel>().objectToSetInactive)
            nextLevelTunnel.GetComponent<Tunnel>().objectToSetInactive = gameObject;
        tunnelHolder = null;
    }
    public void DecideTunnelHolder()
    {
        if (allRocks.Count == 0)
            return;
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
    private void GetAllTiles()
    {
        allTiles.Clear();
        allTiles.AddRange(gameObject.GetComponentsInChildren<Tile>());
    }
    private void GetAllWalls()
    {
        allWalls.Clear();
        allWalls.AddRange(gameObject.GetComponentsInChildren<Wall>());
    }
    private void RemoveInactiveRocks()
    {
        for(int i=0;i<allRocks.Count;i++)
        {
            if (allRocks[i].gameObject.activeInHierarchy == false)
                allRocks.RemoveAt(i);
        }
    }
    public void SetMaterials()
    {
        for (int i = 0; i < allWalls.Count; i++)
        {
            allWalls[i].gameObject.GetComponent<MeshRenderer>().material = wallMat;
        }
        for (int i = 0; i < allTiles.Count; i++)
        {
            if(allTiles[i].typeBFloor)
                allTiles[i].gameObject.GetComponent<MeshRenderer>().material = tileMatB;
            else
                allTiles[i].gameObject.GetComponent<MeshRenderer>().material = tileMatA;
        }
    }
}
