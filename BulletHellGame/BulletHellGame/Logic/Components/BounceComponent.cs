namespace BulletHellGame.Logic.Components
{
    public class BounceComponent : IComponent
    {
        public bool CanBounce { get; set; }
        public int BounceCount { get; set; }

        public BounceComponent(bool canBounce = true)
        {
            CanBounce = canBounce;
            BounceCount = 3;
        }
    }

}
