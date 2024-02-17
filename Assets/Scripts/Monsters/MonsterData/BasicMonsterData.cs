using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MonsterData", order = 1)]
public class BasicMonsterData : ScriptableObject
{
    public Sprite monsterSprite;
    public string originalName;
    public Element element;
    [Tooltip("int ranges from 1-5, 5 being an easier to please monster")]
    [SerializeField, Range(1, 5)] protected int attitude;
    [Tooltip("Set the bool to true if a monster can perform it. 0 advertising, 1 scouting, 2 fishing, 3 forestry, 4 mining")]
    public bool[] jobs = new bool[5];
    [Tooltip("Set the bool to true if a monster can perform it. 0 running shop, 1 fighting, 2 diving, 3 gathering, 4 digging")]
    public bool[] specializations = new bool[5];
    [Tooltip("background information on monster")]
    public string lore;
    //add battle stats
}
