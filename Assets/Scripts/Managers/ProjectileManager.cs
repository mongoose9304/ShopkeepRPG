using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
[System.Serializable]
/// <summary>
/// A container for an projectilePool
/// </summary>
public class ProjectilePool
{
    public MMMiniObjectPooler myPool;
    public string nameOfProjectile;
}
public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager instance;
    public List<ProjectilePool> projectilePools = new List<ProjectilePool>();
    private void Awake()
    {
        instance = this;
    }
    public MMMiniObjectPooler GetPoolFromName(string name_)
    {
        foreach(ProjectilePool pool in projectilePools)
        {
            if(pool.nameOfProjectile==name_)
            {
                return pool.myPool;
            }
        }


        return null;
    }



}
