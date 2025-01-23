namespace BulletHellGame.Components
{
    public class HealthComponent : IComponent
    {
        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }

        public HealthComponent(int maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
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
            this.CurrentHealth = this.MaxHealth;
        }

        public void Update(GameTime gameTime)
        {
            // Logic for health regeneration or effects could go here
        }
    }

}
