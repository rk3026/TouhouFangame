using BulletHellGame.Builders;
using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Managers;

namespace BulletHellGame.Builders
{
    public class EnemyBuilder : EntityBuilder<EnemyData>
    {
        public EnemyBuilder(EnemyData data) : base(data) { }

        public override void SetHealth()
        {
            _entity.AddComponent(new HealthComponent(_entityData.Health));
        }

        public override void SetSprite()
        {
            _entity.AddComponent(new SpriteComponent(TextureManager.Instance.GetSpriteData(_entityData.SpriteName)));
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
            SpriteComponent sprite = _entity.GetComponent<SpriteComponent>();
            if (sprite != null)
            {
                _entity.AddComponent(new HitboxComponent(_entity, 1)
                {
                    Hitbox = new Vector2(sprite.CurrentFrame.Width, sprite.CurrentFrame.Height)
                });
            }
        }
    }
}