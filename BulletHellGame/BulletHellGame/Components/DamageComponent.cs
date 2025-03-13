namespace BulletHellGame.Components
{
    public class DamageComponent : IComponent
    {
        public int BaseDamage { get; set; }
        public float Multiplier { get; set; } = 1.0f; // For scaling damage
        public Func<int, int> DamageModifier { get; set; }

        public DamageComponent(int baseDamage)
        {
            BaseDamage = baseDamage;
        }

        public int CalculateDamage()
        {
            int damage = (int)(BaseDamage * Multiplier);
            return DamageModifier != null ? DamageModifier(damage) : damage;
        }
    }

}
