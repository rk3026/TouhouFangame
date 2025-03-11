using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Managers;

namespace BulletHellGame.Builders
{
    public class EnemyBuilder : EntityBuilder<EnemyData>
    {
        public EnemyBuilder() : base() { }
        public EnemyBuilder(EnemyData data) : base(data) { }

        public override void SetHealth()
        {
            _entity.AddComponent(new HealthComponent(_entityData.Health));
        }

        public override void SetSprite()
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(_entityData.SpriteName);
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2);
            _entity.AddComponent(spriteComponent);
        }

        public override void SetPosition() => _entity.AddComponent(new PositionComponent());
        public override void SetVelocity() => _entity.AddComponent(new VelocityComponent());

        public override void SetMovementPattern()
        {
            _entity.AddComponent(new MovementPatternComponent(_entityData.MovementPattern));
        }

        public override void SetShooting()
        {
            _entity.AddComponent(new ShootingComponent(_entityData.Weapons));
        }

        public override void SetLoot()
        {
            if (_entityData.Loot.Count > 0)
            {
                _entity.AddComponent(new LootComponent { Loot = _entityData.Loot });
            }
        }

        public override void SetHitbox()
        {
            HitboxComponent hc = new HitboxComponent(_entity, 1);
            float spriteWidth = _entity.GetComponent<SpriteComponent>().CurrentFrame.Width;
            float spriteHeight = _entity.GetComponent<SpriteComponent>().CurrentFrame.Height;
            hc.Hitbox = new Vector2(spriteWidth, spriteHeight);
            _entity.AddComponent(hc);
        }
    }
}