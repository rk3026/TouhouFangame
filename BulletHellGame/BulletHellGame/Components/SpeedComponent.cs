namespace BulletHellGame.Components
{
    public class SpeedComponent : IComponent
    {
        public SpeedComponent(float speed = 7, float focusedSpeed = 3)
        {
            Speed = speed;
            FocusedSpeed = focusedSpeed;
        }

        public float Speed { get; set; }
        public float FocusedSpeed { get; set; }
    }
}
