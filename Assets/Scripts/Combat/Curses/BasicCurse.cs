using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///These are the curses and blessinsg that can be applied to a combat player. The functionality fo what each curse does must be defined in the Dungeon Manager
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Curse", order = 1)]
public class BasicCurse : ScriptableObject
{
    [Tooltip("The name of the curse, will be used to apply the effect of teh curse")]
    public string id;
    public Sprite icon;
    public string description;
    [Tooltip("Curses will be given out based on severity, higher should be considered more serious debuffs ")]
    public int severity;
}
