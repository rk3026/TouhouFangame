namespace BulletHellGame.Components
{
    public class PositionComponent : IComponent
    {
        public Vector2 Position {  get; set; }
        public PositionComponent()
        {
            Position = Vector2.Zero;
        }
    }
}
