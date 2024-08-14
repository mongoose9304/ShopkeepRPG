using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The absolute data of a monster. This does not inlcude any stats that are modified by a player. These are the monsters default data
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Curse", order = 1)]
public class BasicCurse : ScriptableObject
{
    public string name;
    public Sprite icon;
    public string description;
    public int severity;
}
