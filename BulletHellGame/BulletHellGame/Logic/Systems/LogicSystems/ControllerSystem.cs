using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Systems.LogicSystems
{
    public class ControllerSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (Entity entity in entityManager.GetEntitiesWithComponents(typeof(ControllerComponent)))
            {
                // Controller updates to determine what the entity is doing currently
                entity.GetComponent<ControllerComponent>().Controller.Update(entityManager, entity);

            }
        }
    }
}
