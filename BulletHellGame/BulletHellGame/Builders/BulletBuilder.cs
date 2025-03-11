using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Builders
{
    public class BulletBuilder : EntityBuilder<BulletData>
    {
        public BulletBuilder(BulletData data) : base(data) { }

        public override void SetSprite()
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(_entityData.SpriteName);
            SpriteComponent spriteComponent = new SpriteComponent(spriteData)
            {
                SpriteData = { Origin = new Vector2(spriteData.Texture.Width / 2, spriteData.Texture.Height / 2) },
                RotationSpeed = _entityData.RotationSpeed
            };
            _entity.AddComponent(spriteComponent);
        }

        public override void SetPosition()
        {
            _entity.AddComponent(new PositionComponent());
        }

        public override void SetVelocity()
        {
            _entity.AddComponent(new VelocityComponent());
        }

        public override void SetDamage()
        {
            _entity.AddComponent(new DamageComponent(_entityData.Damage));
        }

        public override void SetHoming()
        {
            if (_entityData.BulletType == BulletType.Homing)
            {
                _entity.AddComponent(new HomingComponent());
            }
        }

        public override void SetHitbox()
        {
            SpriteComponent sprite = _entity.GetComponent<SpriteComponent>();
            if (sprite != null)
            {
                HitboxComponent hitbox = new HitboxComponent(_entity, 3)
                {
                    Hitbox = new Vector2(sprite.CurrentFrame.Width, sprite.CurrentFrame.Height)
                };
                _entity.AddComponent(hitbox);
            }
        }
    }
}
