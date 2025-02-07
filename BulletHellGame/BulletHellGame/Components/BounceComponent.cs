namespace BulletHellGame.Components
{
    public class BounceComponent : IComponent
    {
        public bool CanBounce { get; set; }

        public BounceComponent(bool canBounce = true)
        {
            CanBounce = canBounce;
        }
    }

}
