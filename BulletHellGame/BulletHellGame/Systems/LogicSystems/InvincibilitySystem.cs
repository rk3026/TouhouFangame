using BulletHellGame.Components;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.LogicSystems
{
    public class InvincibilitySystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (var entity in entityManager.GetEntitiesWithComponents(typeof(InvincibilityComponent)))
            {
                var invincibility = entity.GetComponent<InvincibilityComponent>();

                invincibility.RemainingTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (invincibility.RemainingTime <= 0)
                {
                    if (entity.TryGetComponent<SpriteComponent>(out var sprite))
                    {
                        //sprite.StopFlickerEffect(); // Stop flickering
                    }
                }
            }
        }
    }

}
