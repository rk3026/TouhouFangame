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
                if (entity.TryGetComponent<BulletContainerComponent>(out var bcc) &&
                    entity.TryGetComponent<PositionComponent>(out var pc) &&
                    entity.TryGetComponent<HitboxComponent>(out var hc) &&
                    entity.TryGetComponent<OwnerComponent>(out var oc)
                    )
                {
                    foreach (var bulletData in bcc.BulletsToSpawn.Keys)
                    {
                        for (int i = 0; i < bcc.BulletsToSpawn[bulletData]; i++)
                        {
                            // Assign random direction velocity
                            float angle = Random.Shared.NextSingle() * MathF.Tau; // 0 to 2π
                            var direction = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
                            Entity bullet = entityManager.SpawnBullet(bulletData, pc.Position, hc.Layer, direction, oc.Owner);
                            bullet.GetComponent<DespawnComponent>().DespawnWhenOffScreen = false;
                        }
                    }
                }

                entityManager.QueueEntityForRemoval(entity);
            }
        }
    }
}
