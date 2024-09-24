using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
[System.Serializable]
/// <summary>
/// Used to pool enemies so we don't have to create them over and over
/// </summary>
public class EnemyItem : MonoBehaviour
{
    public string myname;
    public MMMiniObjectPooler pooler;
    
}
