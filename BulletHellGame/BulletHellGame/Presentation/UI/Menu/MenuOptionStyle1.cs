using BulletHellGame.Logic.Managers;
using Microsoft.Xna.Framework.Content;

namespace BulletHellGame.Presentation.UI.Menu
{
    public class MenuOptionStyle1 : IMenuOptionStyle
    {
        private Texture2D whitePixel;
        private ContentManager _contentManager;
        private GraphicsDevice _graphicsDevice;

        public MenuOptionStyle1(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            _contentManager = contentManager;
            _graphicsDevice = graphicsDevice;

            // Create a 1x1 white pixel texture for the box
            whitePixel = new Texture2D(_graphicsDevice, 1, 1);
            whitePixel.SetData(new Color[] { Color.White });
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, string text, int index, Vector2 position, int selectedIndex)
        {
            if (index == selectedIndex)
            {
                // Animate the box
                float time = (float)(gameTime?.TotalGameTime.TotalSeconds ?? 0f);
                float offsetX = (float)Math.Sin(time * 5) * 3; // Oscillation effect
                float offsetY = (float)Math.Cos(time * 5) * 3; // Oscillation effect

                // Draw a box around the selected option with the animation
                var textSize = FontManager.Instance.GetFont("DFPPOPCorn-W12").MeasureString(text);
                var boxPosition = position - new Vector2(5, 5) + new Vector2(offsetX, offsetY);
                var boxSize = new Vector2(textSize.X + 10, textSize.Y + 10);

                // Draw the box with rotation
                spriteBatch.Draw(whitePixel, new Rectangle((int)boxPosition.X, (int)boxPosition.Y, (int)boxSize.X, (int)boxSize.Y), Color.White);

                // Outline effect for selected option
                DrawOutlinedText(spriteBatch, text, position + new Vector2(offsetX, offsetY), Color.Black, 2);
            }

            // Draw the actual menu option text
            Color textColor = (index == selectedIndex) ? Color.Red : Color.White;
            spriteBatch.DrawString(FontManager.Instance.GetFont("DFPPOPCorn-W12"), text, position, textColor);
        }

        // Helper function to draw outlined text
        private void DrawOutlinedText(SpriteBatch spriteBatch, string text, Vector2 position, Color outlineColor, int outlineWidth)
        {
            for (int x = -outlineWidth; x <= outlineWidth; x++)
            {
                for (int y = -outlineWidth; y <= outlineWidth; y++)
                {
                    if (x == 0 && y == 0) continue;

                    spriteBatch.DrawString(FontManager.Instance.GetFont("DFPPOPCorn-W12"), text, position + new Vector2(x, y), outlineColor);
                }
            }
        }
    }

}
