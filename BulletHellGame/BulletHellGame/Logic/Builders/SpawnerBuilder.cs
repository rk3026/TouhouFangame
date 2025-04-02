using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Controllers;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Builders
{
    public class SpawnerBuilder: EntityBuilder<SpawnerData>
    {
        public SpawnerBuilder() : base() { }
        public SpawnerBuilder(SpawnerData data) : base(data) { }

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

        public override void BuildOwner()
        {
            _entity.AddComponent(new OwnerComponent(_entityData.Owner));
        }

        public override void BuildSprite()
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(_entityData.SpriteName);
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2);
            _entity.AddComponent(spriteComponent);
        }

        public override void BuildController()
        {
            _entity.AddComponent(new ControllerComponent(new SpawnerController()));
        }
    }
}