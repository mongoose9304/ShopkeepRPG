using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Talent
{
    public string name;
    public string flavorText;
    public string ID;
    public int levelInvested;
}
/// <summary>
/// The saved Talents of a player
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SavedTalents", order = 1)]
public class SavedTalents : ScriptableObject
{
    public List<Talent> talents = new List<Talent>();
    public int totalTalents;
    public int unspentTalents;
}
