using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Systems.LogicSystems
{
    public class MovementSystem : ILogicSystem
    {
        private float GAME_SPEED = 100f;

        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (var entity in entityManager.GetEntitiesWithComponents(typeof(VelocityComponent), typeof(PositionComponent)))
            {
                if (entity.TryGetComponent<VelocityComponent>(out var vc) &&
                    entity.TryGetComponent<PositionComponent>(out var pc))
                {
                    if (entity.TryGetComponent<OwnerComponent>(out var oc) && entity.HasComponent<ShootingComponent>())
                    {
                        pc.Position += oc.Owner.GetComponent<VelocityComponent>().Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * GAME_SPEED;
                    }
                    // Only update the position if the entity has an owner
                    pc.Position += vc.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * GAME_SPEED;
                }
            }
        }
    }
}
