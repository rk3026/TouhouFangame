using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Managers;

namespace BulletHellGame.Builders
{
    public class BossBuilder : EntityBuilder<BossData>
    {
        public BossBuilder(BossData data) : base(data) { }

        public override void SetPhases()
        {
            if (_entityData.Phases == null || _entityData.Phases.Count == 0)
                throw new ArgumentException("Boss must have at least one phase.");

            _entity.AddComponent(new BossPhaseComponent(_entityData.Phases));
        }

        public override void SetHealth()
        {
            if (_entityData.Phases.Count > 0)
            {
                _entity.AddComponent(new HealthComponent(_entityData.Phases[0].Health));
            }
        }

        public override void SetSprite()
        {
            if (_entityData.Phases.Count > 0)
            {
                SpriteData spriteData = TextureManager.Instance.GetSpriteData(_entityData.Phases[0].SpriteName);
                SpriteComponent spriteComponent = new SpriteComponent(spriteData)
                {
                    SpriteData = { Origin = new Vector2(spriteData.Texture.Width / 2, spriteData.Texture.Height / 2) }
                };
                _entity.AddComponent(spriteComponent);
            }
        }

        public override void SetPosition()
        {
            _entity.AddComponent(new PositionComponent());
        }

        public override void SetVelocity()
        {
            _entity.AddComponent(new VelocityComponent());
        }

        public override void SetMovementPattern()
        {
            if (_entityData.Phases.Count > 0)
            {
                _entity.AddComponent(new MovementPatternComponent(_entityData.Phases[0].MovementPattern));
            }
        }

        public override void SetShooting()
        {
            if (_entityData.Phases.Count > 0 && _entityData.Phases[0].Weapons != null)
            {
                _entity.AddComponent(new ShootingComponent(_entityData.Phases[0].Weapons));
            }
        }

        public override void SetHitbox()
        {
            SpriteComponent sprite = _entity.GetComponent<SpriteComponent>();
            if (sprite != null)
            {
                HitboxComponent hitbox = new HitboxComponent(_entity, 1)
                {
                    Hitbox = new Vector2(sprite.CurrentFrame.Width, sprite.CurrentFrame.Height)
                };
                _entity.AddComponent(hitbox);
            }
        }
    }
}
