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

            ShootingComponent shc = new ShootingComponent(firstPhase.Weapons);
            boss.AddComponent(shc);

            // Set the hitbox:
            HitboxComponent hc = new HitboxComponent(boss, 1);
            SpriteComponent sc = boss.GetComponent<SpriteComponent>();
            hc.Hitbox = new Vector2(sc.CurrentFrame.Width, sc.CurrentFrame.Height);
            boss.AddComponent(hc);

            return boss;
        }
    }
}
