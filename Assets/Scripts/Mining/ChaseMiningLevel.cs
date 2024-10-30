using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseMiningLevel : MiningLevel
{
    public Transform tunnelPos;
    public override void SetUpMiningLevel(float health_)
    {
        GetAllRocks();
        GetAllTreasureRocks();
        GetAllTiles();
        GetAllWalls();
        GetAllEnemies();
        SetMaterials();
        RandomizeAllObjects(health_);
        SetDeadTreasureRocks(health_);
        CreateTunnel(tunnelPos);
    }
   
}
