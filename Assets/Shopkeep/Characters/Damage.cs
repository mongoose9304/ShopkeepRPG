using System;

namespace Shopkeeper {

    public enum DamageType
    {
        NEUTRAL,
        FIRE,
        ICE,
        LIGHTNING
    }

    [Serializable]
    public class Damage {
        public Damage(float a)
        {
            amount = a;
            type = DamageType.NEUTRAL;
        }

        public Damage(float a, DamageType t)
        {
            amount = a;
            type = t;
        }
        public float amount;
        public DamageType type;
    }
}
