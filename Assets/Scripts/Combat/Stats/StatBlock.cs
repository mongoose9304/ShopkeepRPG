using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Basic stats of a Player or familiar
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StatBlock", order = 1)]
public class StatBlock : ScriptableObject
{
    public int Level;
    public int Vitality;
    public int Soul;
    public int PhysicalProwess;
    public int MysticalProwess;
    public int PhysicalDefense;
    public int MysticalDefense;
    public int Luck;
}
