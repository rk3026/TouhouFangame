using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Systems.LogicSystems
{
    public class OffScreenDespawnSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            List<Entity> entitiesToRemove = new List<Entity>();

            foreach (var entity in entityManager.GetEntitiesWithComponents(
                typeof(PositionComponent),
                typeof(SpriteComponent),
                typeof(DespawnComponent)))
            {
                if (entity.TryGetComponent<PositionComponent>(out var pc) &&
                    entity.TryGetComponent<SpriteComponent>(out var sc) &&
                    entity.TryGetComponent<DespawnComponent>(out var dc))
                {
                    if (!dc.DespawnWhenOffScreen) continue;

                    Rectangle boundary = dc.CustomBoundary ?? entityManager.Bounds;

                    if (pc.Position.X < boundary.Left - sc.CurrentFrame.Width
                        || pc.Position.X - sc.CurrentFrame.Width > boundary.Right
                        || pc.Position.Y < boundary.Top - sc.CurrentFrame.Height
                        || pc.Position.Y - sc.CurrentFrame.Height > boundary.Bottom)
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
