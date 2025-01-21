namespace BulletHellGame.Components
{
    public interface IComponent
    {
        void Update(GameTime gameTime);

        public void Reset()
        {
            // Default Implementation
        }
    }

}
