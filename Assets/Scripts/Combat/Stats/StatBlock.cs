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
    public int totalSkillPoints;

    //StartStats;
    public int StartVitality;
    public int StartSoul;
    public int StartPhysicalProwess;
    public int StartMysticalProwess;
    public int StartPhysicalDefense;
    public int StartMysticalDefense;

    private int LevelFormula(int lv_)
    {
        int expNeeded= ((lv_ * ascension) * 10);
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
    public void ResetStats()
    {
        Vitality = StartVitality;
        Soul = StartSoul;
        PhysicalProwess = StartPhysicalProwess;
        MysticalProwess = StartMysticalProwess;
        PhysicalDefense = StartPhysicalDefense;
        MysticalDefense = StartMysticalDefense;
        remainingSkillPoints = totalSkillPoints;
    }
}
