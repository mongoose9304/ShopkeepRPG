using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Shopkeeper
{
    public class Skill : MonoBehaviour
    {
        public string name;
        public TMP_Text levelText;
        public int maxLevel = 5;
        public int currentLevel = 0;

        public SkillTreeBranch branch;
        public SkillEffect effect;

        public void Increase()
        {
            if (currentLevel < maxLevel && branch.treeUI.SpendSP())
            {
                currentLevel++;
                effect?.ApplyEffect(branch.treeUI.player, currentLevel);
                levelText.text = $"{name} {currentLevel}/{maxLevel}";
                branch.OnIncrease();
            }
        }
    }
}
