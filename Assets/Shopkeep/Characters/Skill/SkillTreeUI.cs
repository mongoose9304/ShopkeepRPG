using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shopkeeper
{
    public class SkillTreeUI : MonoBehaviour
    {
        public int availableSP = 10;

        public List<SkillTreeBranch> branches;
        public TMP_Text totalSPSpent;
        public TMP_Text availablePointsText;

        public Player player;

        void Start()
        {
            for (int i = 0; i < branches.Count; i++)
            {
                branches[i].treeUI = this;
                branches[i].branchIndex = i;
                if (i == 0)
                {
                    branches[i].gameObject.SetActive(true);
                }
                else
                {
                    branches[i].gameObject.SetActive(false);
                }
            }

            availablePointsText.text = $"SP Current Amount: {availableSP}";
        }

        public bool SpendSP()
        {
            if (availableSP > 0)
            {
                availableSP--;
                availablePointsText.text = $"SP Current Amount: {availableSP}";
                return true;
            }
            return false;
        }

        void UpdateSkillText(Skill skill)
        {
            skill.levelText.text = $"{skill.name} Lv {skill.currentLevel}/{skill.maxLevel}";
        }

        public int TotalPointsSpent()
        {
            return branches.Sum(branch => branch.TotalPointsSpent());
        }

        public void TryUnlockNextLayer(int currentLayer)
        {
            int totalPoints = TotalPointsSpent();

            for (int i = 0; i < branches.Count; i++)
            {
                if (!branches[i].gameObject.activeSelf && totalPoints >= branches[i].SPToUnlock)
                {
                    branches[i].gameObject.SetActive(true);
                }
            }

            totalSPSpent.text = "SP Spent: " + TotalPointsSpent();

        }
    }
}