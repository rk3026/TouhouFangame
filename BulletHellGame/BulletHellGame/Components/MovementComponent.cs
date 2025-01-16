namespace BulletHellGame.Components
{
    public class MovementComponent : IComponent
    {
        private Sprite _owner;
        public Vector2 Velocity { get; set; }

        public MovementComponent(Sprite owner, Vector2 velocity)
        {
            _owner = owner;
            Velocity = velocity;
        }

        public void Update(GameTime gameTime)
        {
            _owner.Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }

}
