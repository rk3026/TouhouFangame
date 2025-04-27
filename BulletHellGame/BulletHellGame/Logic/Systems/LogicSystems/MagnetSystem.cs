using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Managers;
using BulletHellGame.Logic.Systems;

namespace BulletHellGame.Logic.Systems.LogicSystems
{
    public class MagnetSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (var entity in entityManager.GetEntitiesWithComponents(typeof(MagnetComponent), typeof(PositionComponent)))
            {
                if (!entity.TryGetComponent<MagnetComponent>(out var mc) ||
                    !entity.TryGetComponent<PositionComponent>(out var pc))
                {
                    continue;
                }

                if (entity.HasComponent<InvincibilityComponent>() && entity.GetComponent<InvincibilityComponent>().RemainingTime > 0) continue;

                // Find all attractable objects (e.g., collectibles, power-ups)
                foreach (var attractable in entityManager.GetEntitiesWithComponents(typeof(PositionComponent), typeof(VelocityComponent), typeof(AttractableComponent)))
                {
                    if (!attractable.TryGetComponent<PositionComponent>(out var attractablePosition) ||
                        !attractable.TryGetComponent<VelocityComponent>(out var velocity))
                    {
                        continue;
                    }

                    // Check if the attractable object is within range
                    float distanceSquared = Vector2.DistanceSquared(pc.Position, attractablePosition.Position);
                    if (distanceSquared <= mc.MagnetRange * mc.MagnetRange)
                    {
                        // Calculate attraction force
                        Vector2 directionToMagnet = pc.Position - attractablePosition.Position;
                        if (directionToMagnet.LengthSquared() > 0)
                            directionToMagnet.Normalize();

                        // Apply attraction
                        velocity.Velocity += directionToMagnet * mc.MagnetStrength * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }

                    float collectionThreshold = mc.SnapRange;
                    if (distanceSquared < collectionThreshold * collectionThreshold)
                    {
                        attractablePosition.Position = pc.Position;
                        velocity.Velocity = Vector2.Zero; // Stop movement
                    }
                }
            }
        }
    }
}