using BulletHellGame.Components;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.LogicSystems
{
    public class OffScreenDespawnSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            List<Entity> entitiesToRemove = new List<Entity>();

            foreach (var entity in entityManager.GetEntitiesWithComponents(typeof(PositionComponent), typeof(SpriteComponent)))
            {
                if (entity.TryGetComponent<PositionComponent>(out var pc) &&
                    entity.TryGetComponent<SpriteComponent>(out var sc))
                {
                    if (pc.Position.X < entityManager.Bounds.Left - sc.CurrentFrame.Width
                        || pc.Position.X - sc.CurrentFrame.Width > entityManager.Bounds.Right
                        || pc.Position.Y < entityManager.Bounds.Top - sc.CurrentFrame.Height
                        || pc.Position.Y - sc.CurrentFrame.Height > entityManager.Bounds.Bottom)
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
