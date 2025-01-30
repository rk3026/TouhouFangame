using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.LogicSystems
{
    public class AnimationSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (Entity entity in entityManager.GetActiveEntities())
            {
                if (entity.TryGetComponent<SpriteComponent>(out var sc))
                {
                    sc.Update(gameTime);
                }
            }
        }
    }
}
