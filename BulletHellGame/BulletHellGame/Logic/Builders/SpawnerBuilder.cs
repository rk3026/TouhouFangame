using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Components;

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
    }
}
