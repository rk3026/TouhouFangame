namespace BulletHellGame.Components
{
    public class MovementComponent : IComponent
    {
        public Vector2 Position {  get; set; }
        public Vector2 Velocity { get; set; }

        public MovementComponent()
        {
            Position = Vector2.Zero;
            Velocity = Vector2.Zero;
        }
    }
}
