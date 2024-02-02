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
   
    [Header("BaseStats")]
    [SerializeField] private string originalName;
    [SerializeField] private Element element;
        [Tooltip("int ranges from 1-5, 5 being an easier to please monster")]
    [SerializeField] private int attitude;
        [Tooltip("Set the bool to true if a monster can perform it. 0 advertising, 1 scouting, 2 fishing, 3 forestry, 4 mining")]
    [SerializeField] private bool[] jobs=new bool[5];
        [Tooltip("Set the bool to true if a monster can perform it. 0 running shop, 1 fighting, 2 diving, 3 gathering, 4 digging")]
    [SerializeField] private bool[] specializations = new bool[5];
}
