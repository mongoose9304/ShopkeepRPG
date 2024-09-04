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
    public int totalEXP;

    private int LevelFormula(int lv_)
    {
        return (lv_ * lv_) * 10 + lv_ * 200;
    }
    public int GetEXPToLevelUp()
    {
        int temp = 0;
        for(int i=0;i<Level;i++)
        {
            temp += LevelFormula(i+1);
        }
        temp -= totalEXP;
        return temp;
    }
}
