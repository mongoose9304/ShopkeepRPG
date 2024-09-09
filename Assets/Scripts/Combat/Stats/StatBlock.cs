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
    public int ascension=1;
    public int Vitality;
    public int Soul;
    public int PhysicalProwess;
    public int MysticalProwess;
    public int PhysicalDefense;
    public int MysticalDefense;
    public int Luck;
    public int savedExp;
    public int remainingSkillPoints;
    public int talentPoints;

    private int LevelFormula(int lv_)
    {
        int expNeeded= ((lv_ * ascension) * 10) + (lv_ * 200);
        if(lv_>50)
        {
            expNeeded *= 2;
        }
        if (lv_ > 90)
        {
            expNeeded *= 2;
        }
        return expNeeded;
    }
    public int GetEXPToLevelUp()
    {
        int temp = 0;
        for(int i=0;i<Level;i++)
        {
            temp += LevelFormula(i+1);
        }
        temp -= savedExp;
        return temp;
    }
}
