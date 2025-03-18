using BulletHellGame.Components;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.LogicSystems
{
    public class InvincibilitySystem : ILogicSystem
    {
        private const float FlickerInterval = 0.1f;  // Controls flicker speed (in seconds)

        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (var entity in entityManager.GetEntitiesWithComponents(typeof(InvincibilityComponent)))
            {
                var invincibility = entity.GetComponent<InvincibilityComponent>();
                invincibility.RemainingTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (entity.TryGetComponent<SpriteComponent>(out var sprite))
                {
                    if (invincibility.RemainingTime > 0)
                    {
                        // Flicker logic: toggle sprite color based on time
                        float flickerTime = (float)(invincibility.RemainingTime % (2 * FlickerInterval));
                        sprite.Color = flickerTime < FlickerInterval ? Color.Transparent : Color.White;
                    }
                    else
                    {
                        // Reset sprite color and remove invincibility
                        sprite.Color = Color.White;
                    }
                }
            }
        }
    }
}
