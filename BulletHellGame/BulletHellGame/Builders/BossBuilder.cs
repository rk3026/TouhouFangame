using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Managers;

namespace BulletHellGame.Builders
{
    public class BossBuilder : EntityBuilder<BossData>
    {
        public BossBuilder() : base() { }
        public BossBuilder(BossData data) : base(data) { }

        public override void SetPhases()
        {
            _entity.AddComponent(new BossPhaseComponent(_entityData.Phases));
        }

        public override void SetHealth()
        {
            _entity.AddComponent(new HealthComponent(_entityData.Phases[0].Health));
        }

        public override void SetSprite()
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(_entityData.Phases[0].SpriteName);
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2);
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

        public override void SetMovementPattern()
        {
            _entity.AddComponent(new MovementPatternComponent(_entityData.Phases[0].MovementPattern));
        }

        public override void SetShooting()
        {
            ShootingComponent shc = new ShootingComponent(_entityData.Phases[0].Weapons);
            _entity.AddComponent(shc);
        }

        public override void SetHitbox()
        {
            HitboxComponent hc = new HitboxComponent(_entity, 1);
            SpriteComponent sc = _entity.GetComponent<SpriteComponent>();
            hc.Hitbox = new Vector2(sc.CurrentFrame.Width, sc.CurrentFrame.Height);
            _entity.AddComponent(hc);
        }
    }
}
