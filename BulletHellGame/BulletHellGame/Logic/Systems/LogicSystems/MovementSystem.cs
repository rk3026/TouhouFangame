using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Managers;
using BulletHellGame.Logic.Systems;

namespace BulletHellGame.Logic.Systems.LogicSystems
{
    public class MovementSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (var entity in entityManager.GetEntitiesWithComponents(typeof(VelocityComponent), typeof(PositionComponent)))
            {
                if (entity.TryGetComponent<VelocityComponent>(out var vc) &&
                    entity.TryGetComponent<PositionComponent>(out var pc))
                {
                    pc.Position += vc.Velocity;
                }
            }
        }
    }
}
