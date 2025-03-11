using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Builders
{
    public class BulletBuilder : EntityBuilder<BulletData>
    {
        public BulletBuilder() : base() { }
        public BulletBuilder(BulletData data) : base(data) { }

        public override void SetSprite()
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(_entityData.SpriteName);
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2);
            spriteComponent.RotationSpeed = _entityData.RotationSpeed;
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
            HitboxComponent hc = new HitboxComponent(_entity, 3);
            hc.Hitbox = new Vector2(_entity.GetComponent<SpriteComponent>().CurrentFrame.Width, _entity.GetComponent<SpriteComponent>().CurrentFrame.Width);
            _entity.AddComponent(hc); // Layer 1 = enemies and their bullets
        }
    }
}
