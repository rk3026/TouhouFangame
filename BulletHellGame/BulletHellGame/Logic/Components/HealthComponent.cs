namespace BulletHellGame.Logic.Components
{
    public class HealthComponent : IComponent
    {
        public int MaxHealth { get; private set; } = 0;
        public int CurrentHealth { get; private set; } = 0;

        public HealthComponent(int health = 100) {
            MaxHealth = health;
            CurrentHealth = health;
        }

        public void TakeDamage(int amount)
        {
            CurrentHealth -= amount;
            if (CurrentHealth < 0) CurrentHealth = 0;
        }

        public void Heal(int amount)
        {
            CurrentHealth += amount;
            if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
        }

        public void Reset()
        {
            CurrentHealth = MaxHealth;
        }
    }

}
