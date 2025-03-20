namespace BulletHellGame.Logic.Components
{
    public class VelocityComponent : IComponent
    {
        public Vector2 Velocity { get; set; }
        public VelocityComponent() {
            Velocity = Vector2.Zero;
        }
    }
}
