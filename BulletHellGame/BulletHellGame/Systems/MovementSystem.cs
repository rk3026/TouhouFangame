using BulletHellGame.Components;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems
{
    public class MovementSystem : ISystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            List<Entity> entitiesToRemove = new List<Entity>();  // Collect entities to remove

            foreach (var entity in entityManager.GetActiveEntities())
            {
                if (entity.HasComponent<MovementComponent>())
                {
                    var movementComponent = entity.GetComponent<MovementComponent>();
                    var spriteComponent = entity.GetComponent<SpriteComponent>();

                    // Update movement
                    movementComponent.Position += movementComponent.Velocity;

                    // Bounce off walls if it has health
                    if (entity.HasComponent<HealthComponent>())
                    {
                        if (!entityManager.Bounds.Contains(movementComponent.Position))
                        {
                            if (movementComponent.Position.Y < entityManager.Bounds.Top || movementComponent.Position.Y > entityManager.Bounds.Bottom)
                            {
                                movementComponent.Velocity = new Vector2(movementComponent.Velocity.X, -movementComponent.Velocity.Y);
                            }
                            if (movementComponent.Position.X < entityManager.Bounds.Left || movementComponent.Position.X > entityManager.Bounds.Right)
                            {
                                movementComponent.Velocity = new Vector2(-movementComponent.Velocity.X, movementComponent.Velocity.Y);
                            }
                        }
                    }

                    // Delete entities that are off screen
                    if (movementComponent.Position.X < entityManager.Bounds.Left - spriteComponent.CurrentFrame.Width
                        || movementComponent.Position.X - spriteComponent.CurrentFrame.Width > entityManager.Bounds.Right
                        || movementComponent.Position.Y < entityManager.Bounds.Top - spriteComponent.CurrentFrame.Height
                        || movementComponent.Position.Y - spriteComponent.CurrentFrame.Height > entityManager.Bounds.Bottom)
                    {
                        entitiesToRemove.Add(entity);
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
