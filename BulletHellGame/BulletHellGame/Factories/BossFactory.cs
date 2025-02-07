using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Factories
{
    public class BossFactory
    {
        private Random _random = new Random();

        public BossFactory() { }

        public Entity CreateBoss(BossData bossData)
        {
            List<EnemyData> phases = bossData.Phases;
            if (phases == null || phases.Count == 0)
                throw new ArgumentException("Boss must have at least one phase.");

            EnemyData firstPhase = phases[0];

            Entity boss = new Entity();

            // Add the phase manager to track boss phases
            boss.AddComponent(new BossPhaseComponent(phases));

            // Initialize the boss with first phase stats
            boss.AddComponent(new HealthComponent(firstPhase.Health));
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(firstPhase.SpriteName);
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2);
            boss.AddComponent(spriteComponent);
            boss.AddComponent(new PositionComponent());
            boss.AddComponent(new VelocityComponent());
            boss.AddComponent(new MovementPatternComponent(firstPhase.MovementPattern));

            ShootingComponent wc = new ShootingComponent(firstPhase.BulletData);
            wc.FireRate = (float)_random.NextDouble() * 2f + 1f; // Fire rate between 1 and 3 shots per second

            // Circular bullet pattern
            int numBullets = _random.Next(8, 13); // Boss fires between 8-12 bullets per shot
            float angleStep = 360f / numBullets; // Evenly distribute bullets

            for (int i = 0; i < numBullets; i++)
            {
                float angle = (i * angleStep) * (MathF.PI / 180f); // Convert degrees to radians
                wc.FireDirections.Add(new Vector2(MathF.Cos(angle), MathF.Sin(angle))); // Circular pattern
            }

            boss.AddComponent(wc);

            // Set the hitbox:
            HitboxComponent hc = new HitboxComponent(boss, 1);
            SpriteComponent sc = boss.GetComponent<SpriteComponent>();
            hc.Hitbox = new Vector2(sc.CurrentFrame.Width, sc.CurrentFrame.Height);
            boss.AddComponent(hc);

            return boss;
        }
    }
}
