using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.LogicSystems
{
    public class AnimationSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            var entitiesWithSpriteComponent = entityManager.GetEntitiesWithComponents(typeof(SpriteComponent));

            foreach (Entity entity in entitiesWithSpriteComponent)
            {
                if (entity.TryGetComponent<SpriteComponent>(out var sc))
                {
                    sc.Update(gameTime);
                }
            }
        }
    }
}
