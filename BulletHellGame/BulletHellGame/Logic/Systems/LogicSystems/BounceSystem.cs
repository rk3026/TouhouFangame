using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Managers;
using BulletHellGame.Logic.Systems;

namespace BulletHellGame.Logic.Systems.LogicSystems
{
    public class BounceSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (var entity in entityManager.GetEntitiesWithComponents(typeof(BounceComponent)))
            {
                if (entity.TryGetComponent<BounceComponent>(out var bc) &&
                    entity.TryGetComponent<PositionComponent>(out var pc) &&
                    entity.TryGetComponent<VelocityComponent>(out var vc))
                {
                    bc.Lifetime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (bc.Lifetime <= 0)
                    {
                        entityManager.QueueEntityForRemoval(entity);
                        continue;
                    }

                    if (!bc.CanBounce) continue;
                    if (!entityManager.Bounds.Contains(pc.Position))
                    {
                        if (pc.Position.Y < entityManager.Bounds.Top || pc.Position.Y > entityManager.Bounds.Bottom)
                        {
                            vc.Velocity = new Vector2(vc.Velocity.X, -vc.Velocity.Y);
                        }
                        if (pc.Position.X < entityManager.Bounds.Left || pc.Position.X > entityManager.Bounds.Right)
                        {
                            vc.Velocity = new Vector2(-vc.Velocity.X, vc.Velocity.Y);
                        }
                    }
                }
            }
        }
    }
}