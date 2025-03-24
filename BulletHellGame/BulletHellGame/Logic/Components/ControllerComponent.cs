using BulletHellGame.Logic.Controllers;

namespace BulletHellGame.Logic.Components
{
    public class ControllerComponent : IComponent
    {
        public EntityController Controller { get; set; }

        public ControllerComponent(EntityController controller)
        {
            Controller = controller;
        }
    }
}
