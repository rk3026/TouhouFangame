using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Managers;

namespace BulletHellGame.Builders
{
    public class OptionBuilder : EntityBuilder<OptionData>
    {
        public OptionBuilder() : base() { }
        public OptionBuilder(OptionData data) : base(data) { }

        public override void SetSprite()
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(_entityData.SpriteName);
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2);
            spriteComponent.RotationSpeed = 0.1f;
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

        public override void SetShooting()
        {
            _entity.AddComponent(new ShootingComponent(_entityData.Weapons));
        }
    }
}
