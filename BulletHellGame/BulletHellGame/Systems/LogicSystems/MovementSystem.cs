using BulletHellGame.Components;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.LogicSystems
{
    public class MovementSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            List<Entity> entitiesToRemove = new List<Entity>();  // Collect entities to remove

            foreach (var entity in entityManager.GetEntitiesWithComponents(typeof(VelocityComponent)))
            {
                if (entity.TryGetComponent<VelocityComponent>(out var vc) &&
                entity.TryGetComponent<PositionComponent>(out var pc))
                {
                    // Update movement
                    pc.Position += vc.Velocity;

                    // Bounce off walls if it has health
                    if (entity.HasComponent<HealthComponent>() && !entity.HasComponent<PlayerInputComponent>())
                    {
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
                    if (entity.TryGetComponent<SpriteComponent>(out var sc))
                    {
                        // Delete entities that are off screen
                        if (pc.Position.X < entityManager.Bounds.Left - sc.CurrentFrame.Width
                            || pc.Position.X - sc.CurrentFrame.Width > entityManager.Bounds.Right
                            || pc.Position.Y < entityManager.Bounds.Top - sc.CurrentFrame.Height
                            || pc.Position.Y - sc.CurrentFrame.Height > entityManager.Bounds.Bottom)
                        {
                            entitiesToRemove.Add(entity);
                        }
                    }
                }
            }

            foreach (var entity in entitiesToRemove)
            {
                entityManager.QueueEntityForRemoval(entity);
            }
        }

    }
}
