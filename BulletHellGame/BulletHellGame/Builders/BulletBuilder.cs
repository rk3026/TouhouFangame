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

        /// <summary>
        /// Takes an already constructed bullet and changes the data (in the components) to match the new bullet data
        /// </summary>
        /// <param name="bullet"> The bullet entity to modify. </param>
        /// <param name="bulletData"> The data to apply to the entity. </param>
        public void ApplyBulletData(Entity bullet, BulletData bulletData)
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(bulletData.SpriteName);
            bullet.RemoveComponent<SpriteComponent>();
            bullet.AddComponent(new SpriteComponent(spriteData));
            bullet.GetComponent<SpriteComponent>().SpriteData.Origin = new Vector2(bullet.GetComponent<SpriteComponent>().CurrentFrame.Width / 2, bullet.GetComponent<SpriteComponent>().CurrentFrame.Height / 2);
            bullet.GetComponent<SpriteComponent>().RotationSpeed = bulletData.RotationSpeed;
            bullet.GetComponent<HitboxComponent>().Hitbox = new Vector2(bullet.GetComponent<SpriteComponent>().CurrentFrame.Width, bullet.GetComponent<SpriteComponent>().CurrentFrame.Height);
            bullet.GetComponent<DamageComponent>().BaseDamage = bulletData.Damage;
            if (bulletData.BulletType != BulletType.Homing)
            {
                if (bullet.HasComponent<HomingComponent>())
                {
                    bullet.RemoveComponent<HomingComponent>();
                }
            }
            else
            {
                if (!bullet.HasComponent<HomingComponent>())
                {
                    bullet.AddComponent(new HomingComponent());
                }
            }
        }

        public override void BuildSprite()
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(_entityData.SpriteName);
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2);
            spriteComponent.RotationSpeed = _entityData.RotationSpeed;
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

        public override void BuildDamage()
        {
            _entity.AddComponent(new DamageComponent(_entityData.Damage));
        }

        public override void BuildHoming()
        {
            if (_entityData.BulletType == BulletType.Homing)
            {
                _entity.AddComponent(new HomingComponent());
            }
        }

        public override void BuildHitbox()
        {
            HitboxComponent hc = new HitboxComponent(_entity, 3);
            hc.Hitbox = new Vector2(_entity.GetComponent<SpriteComponent>().CurrentFrame.Width, _entity.GetComponent<SpriteComponent>().CurrentFrame.Width);
            _entity.AddComponent(hc); // Layer 1 = enemies and their bullets
        }
    }
}
