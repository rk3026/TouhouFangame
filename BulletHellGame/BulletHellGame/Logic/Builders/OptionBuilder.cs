using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Controllers;
using BulletHellGame.Logic.Managers;
using BulletHellGame.DataAccess.DataTransferObjects;

namespace BulletHellGame.Logic.Builders
{
    public class OptionBuilder : EntityBuilder<OptionData>
    {
        public OptionBuilder() : base() { }
        public OptionBuilder(OptionData data) : base(data) { }

        public override void BuildSprite()
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(_entityData.SpriteName);
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2);
            spriteComponent.RotationSpeed = 0.1f;
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

        public override void BuildShooting()
        {
            _entity.AddComponent(new ShootingComponent(_entityData.Weapons));
        }

        public override void BuildInput()
        {
            _entity.AddComponent(new InputComponent(new OptionController()));
        }
    }
}
