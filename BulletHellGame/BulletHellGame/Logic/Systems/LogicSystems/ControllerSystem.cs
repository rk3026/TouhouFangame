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
                entity.GetComponent<ControllerComponent>().Controller.Update(entityManager, entity);

                // Now update all the things from the controller
                // if (Controller.IsShooting) spawn a bullet;
                // if (Controller.IsMoving) vc.Velocity = Controller.speed * controller.direction;
            }
        }
    }
}
