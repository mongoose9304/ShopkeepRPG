using Shopkeeper;
using UnityEngine;
namespace Shopkeeper
{
    [CreateAssetMenu(fileName = "MaxHP", menuName = "Skills/MaxHP")]
    public class MaxHP : SkillEffect
    {
        public int healthPerLevel = 10;

        public override void ApplyEffect(Player player, int level)
        {
            player.GetComponent<CharacterHealth>().maxHealth += level * healthPerLevel;
        }
    }
}