namespace BulletHellGame.Logic.Components
{
    public class BombingComponent : IComponent
    {
        public int BombCount { get; set; }
        public float TimeSinceLastBomb { get; set; }
        public float BombCooldown { get; set; }
        public int BombDamage { get; set; }

        public BombingComponent(int initialBombs, float cooldown)
        {
            BombCount = initialBombs;
            TimeSinceLastBomb = cooldown;
            BombCooldown = cooldown;
            BombDamage = 500;
        }
    }
}
