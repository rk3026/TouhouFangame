namespace BulletHellGame.Presentation.Scenes
{
    public interface IScene
    {
        public bool IsOverlay { get; }
        public void Load();
        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);
    }
}