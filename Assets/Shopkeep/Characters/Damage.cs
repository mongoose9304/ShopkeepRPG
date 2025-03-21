namespace Shopkeeper {

    public enum DamageType
    {
        NEUTRAL,
        FIRE,
        ICE,
        LIGHTNING
    }
    public struct Damage {
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
