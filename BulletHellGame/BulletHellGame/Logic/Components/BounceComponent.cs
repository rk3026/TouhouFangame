namespace BulletHellGame.Logic.Components
{
    public class BounceComponent : IComponent
    {
        public bool CanBounce { get; set; }
        public float Lifetime { get; set; }

        public BounceComponent(bool canBounce = true)
        {
            CanBounce = canBounce;
            Lifetime = 10f;
        }
    }

}
