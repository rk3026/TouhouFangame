using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Components;
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
            _entity.AddComponent(new SpriteComponent(TextureManager.Instance.GetSpriteData(_entityData.SpriteName)));
        }
    }
}