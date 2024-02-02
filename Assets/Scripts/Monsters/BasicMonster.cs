using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{
    Fire,Water,Air,Earth,Neutral
};

public class BasicMonster : MonoBehaviour
{
    [Header("CurrentStats")]
        [Tooltip("Name Given by Player")]
    [SerializeField] private string name_;
        [Tooltip("Higher happiness means a more productive monster, maxes at 100")]
    [SerializeField, Range(0, 100)] private float happiness_;
    //add current battle stats

    [Header("BaseStats")]
    [SerializeField] protected BasicMonsterData myData_;
 
}
