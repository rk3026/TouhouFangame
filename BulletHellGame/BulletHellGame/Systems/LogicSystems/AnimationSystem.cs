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
                SpriteComponent sc = entity.GetComponent<SpriteComponent>();
                sc.Update(gameTime);
            }
        }
    }
}
