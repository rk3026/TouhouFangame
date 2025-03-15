using BulletHellGame.Controllers;

namespace BulletHellGame.Components
{
    public class InputComponent : IComponent
    {
        public IController Controller { get; set; } // The controller is who is controlling the entity that has this component

        public InputComponent(IController controller)
        {
            Controller = controller;
        }
    }
}
