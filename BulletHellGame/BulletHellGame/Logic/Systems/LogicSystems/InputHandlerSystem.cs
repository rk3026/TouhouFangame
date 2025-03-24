using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;
using BulletHellGame.Logic.Systems;

namespace BulletHellGame.Logic.Systems.LogicSystems
{
    public class InputHandlerSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (Entity entity in entityManager.GetEntitiesWithComponents(typeof(InputComponent)))
            {
                entity.GetComponent<InputComponent>().Controller.Update(entityManager, entity);

                // Now update all the things from the controller
                // if (Controller.IsShooting) spawn a bullet;
                // if (Controller.IsMoving) vc.Velocity = Controller.speed * controller.direction;
            }
        }
    }
}
