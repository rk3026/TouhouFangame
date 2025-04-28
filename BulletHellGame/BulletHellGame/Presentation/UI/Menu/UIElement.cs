namespace BulletHellGame.Presentation.UI.Menu
{
    public abstract class UIElement
    {
        public Vector2 Position { get; set; }
        public bool IsVisible { get; set; } = true;

        protected Texture2D _whitePixel;
        protected SpriteFont _font;

        public UIElement(Vector2 position, Texture2D whitePixel, SpriteFont font)
        {
            Position = position;
            _whitePixel = whitePixel;
            _font = font;
        }

        // Abstract methods for updating and drawing UI elements
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        // Optionally, you can add helper functions here for common behaviors
        protected void DrawOutlinedText(SpriteBatch spriteBatch, string text, Vector2 position, Color outlineColor, int outlineWidth)
        {
            for (int x = -outlineWidth; x <= outlineWidth; x++)
            {
                for (int y = -outlineWidth; y <= outlineWidth; y++)
                {
                    if (x == 0 && y == 0) continue;

                    spriteBatch.DrawString(_font, text, position + new Vector2(x, y), outlineColor);
                }
            }
        }
    }

}
