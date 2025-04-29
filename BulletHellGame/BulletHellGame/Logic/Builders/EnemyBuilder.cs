using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Controllers;
using BulletHellGame.Logic.Managers;
using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Strategies.CollisionStrategies;

namespace BulletHellGame.Logic.Builders
{
    public class EnemyBuilder : EntityBuilder<GruntData>
    {
        public EnemyBuilder() : base() { }
        public EnemyBuilder(GruntData data) : base(data) { }

        public override void BuildHealth()
        {
            _entity.AddComponent(new HealthComponent(_entityData.Health));
        }

        public override void BuildSprite()
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(_entityData.SpriteName);
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2);
            _entity.AddComponent(spriteComponent);
        }

        public override void BuildPosition() => _entity.AddComponent(new PositionComponent());
        public override void BuildVelocity() => _entity.AddComponent(new VelocityComponent());

        public override void BuildMovementPattern()
        {
            _entity.AddComponent(new MovementPatternComponent(_entityData.MovementPattern));
        }

        public override void BuildLoot()
        {
            if (_entityData.Loot.Count > 0)
            {
                _entity.AddComponent(new LootComponent { Loot = _entityData.Loot });
            }
        }

        public override void BuildHitbox()
        {
            HitboxComponent hc = new HitboxComponent(_entity, HitboxLayer.EnemiesAndEnemyBullets);
            float spriteWidth = _entity.GetComponent<SpriteComponent>().CurrentFrame.Width;
            float spriteHeight = _entity.GetComponent<SpriteComponent>().CurrentFrame.Height;
            hc.Hitbox = new Vector2(spriteWidth, spriteHeight);
            _entity.AddComponent(hc);
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
            // Enemies deal damage when colliding with player
            _entity.AddComponent(new DamageComponent(25));
        }
    }
}