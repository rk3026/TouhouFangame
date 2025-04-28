using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Controllers;
using BulletHellGame.Logic.Managers;
using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Strategies.CollisionStrategies;

namespace BulletHellGame.Logic.Builders
{
    public class BossBuilder : EntityBuilder<BossData>
    {
        public BossBuilder() : base() { }
        public BossBuilder(BossData data) : base(data) { }

        public override void BuildPhases()
        {
            _entity.AddComponent(new BossPhaseComponent(_entityData.Phases));
        }

        public override void BuildHealth()
        {
            _entity.AddComponent(new HealthComponent(_entityData.Phases[0].Health));
        }

        public override void BuildSprite()
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(_entityData.Phases[0].SpriteName);
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2);
            _entity.AddComponent(spriteComponent);
        }

        public override void BuildPosition()
        {
            _entity.AddComponent(new PositionComponent());
        }

        public override void BuildVelocity()
        {
            _entity.AddComponent(new VelocityComponent());
        }

        public override void BuildMovementPattern()
        {
            _entity.AddComponent(new MovementPatternComponent(_entityData.Phases[0].MovementPattern));
        }

        public override void BuildHitbox()
        {
            HitboxComponent hc = new HitboxComponent(_entity, HitboxLayer.EnemiesAndEnemyBullets);
            SpriteComponent sc = _entity.GetComponent<SpriteComponent>();
            hc.Hitbox = new Vector2(sc.CurrentFrame.Width, sc.CurrentFrame.Height);
            _entity.AddComponent(hc);
        }

        public override void BuildIndicator()
        {
            _entity.AddComponent(new IndicatorComponent());
        }

        public override void BuildController()
        {
            _entity.AddComponent(new ControllerComponent(new EnemyController()));
        }

        public override void BuildCollisionStrategy()
        {
            _entity.AddComponent(new CollisionStrategyComponent(new EnemyCollisionStrategy()));
        }

        public override void BuildDamage()
        {
            _entity.AddComponent(new DamageComponent(100));
        }
    }
}
