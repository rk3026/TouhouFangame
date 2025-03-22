namespace BulletHellGame.Logic.Components
{
    public class AccelerationComponent : IComponent
    {
        public Vector2 Acceleration { get; set; } = Vector2.Zero;

        public AccelerationComponent(Vector2 acceleration) {
            Acceleration = acceleration;
        }
    }
}
