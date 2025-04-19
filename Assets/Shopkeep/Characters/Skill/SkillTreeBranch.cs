using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shopkeeper
{
    public class SkillTreeBranch : MonoBehaviour
    {
        public List<Skill> skills = new();
        public int SPToUnlock = 5;

        public SkillTreeUI treeUI;
        public int branchIndex;

        void Awake()
        {
            foreach (var skill in skills)
            {
                skill.branch = this;
            }
        }

        public int TotalPointsSpent()
        {
            return skills.Sum(skill => skill.currentLevel);
        }

        public void OnIncrease()
        {
            treeUI.TryUnlockNextLayer(branchIndex);
        }
    }
}
