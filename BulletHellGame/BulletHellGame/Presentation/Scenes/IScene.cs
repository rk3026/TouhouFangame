namespace BulletHellGame.Presentation.Scenes
{
    public interface IScene
    {
        public bool IsOverlay { get; }
        public bool IsMenu { get; }
        public void Load();
        public virtual void Unload() { } // Not needed but can override
        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);
    }
}