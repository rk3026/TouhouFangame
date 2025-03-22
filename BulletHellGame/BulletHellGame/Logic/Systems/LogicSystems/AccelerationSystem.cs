using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Systems.LogicSystems
{
    public class AccelerationSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (var entity in entityManager.GetEntitiesWithComponents(
                         typeof(VelocityComponent),
                         typeof(AccelerationComponent)))
            {
                if (entity.TryGetComponent<VelocityComponent>(out var velocity) &&
                    entity.TryGetComponent<AccelerationComponent>(out var acceleration))
                {
                    float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    velocity.Velocity += acceleration.Acceleration * deltaTime;
                }
            }
        }
    }
}
