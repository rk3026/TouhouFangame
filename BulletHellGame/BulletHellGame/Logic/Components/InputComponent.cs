using BulletHellGame.Logic.Controllers;

namespace BulletHellGame.Logic.Components
{
    public class InputComponent : IComponent
    {
        public EntityController Controller { get; set; } // The controller is who is controlling the entity that has this component

        public InputComponent(EntityController controller)
        {
            Controller = controller;
        }
    }
}
