namespace BulletHellGame.Presentation.UI.Menu
{
    public interface IMenuOptionStyle
    {
        void Draw(SpriteBatch spriteBatch, GameTime gameTime, string text, int index, Vector2 position, int selectedIndex);
    }

}
