using UnityEngine;
namespace Shopkeeper
{
    public abstract class SkillEffect : ScriptableObject
    {
        public abstract void ApplyEffect(Player player, int level);
    }
}