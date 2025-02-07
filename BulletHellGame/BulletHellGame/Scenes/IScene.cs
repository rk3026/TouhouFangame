namespace BulletHellGame.Scenes
{
    public interface IScene
    {
        public void Load();
        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);

        public virtual RenderTarget2D GetFrame()
        {
            Globals.GraphicsDevice.Clear(Color.Black);

            Globals.SpriteBatch.Begin();
            Draw(Globals.SpriteBatch);
            Globals.SpriteBatch.End();

            return new RenderTarget2D(Globals.GraphicsDevice, Globals.WindowSize.X, Globals.WindowSize.Y);
        }
    }
}