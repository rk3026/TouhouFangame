using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Managers;
using BulletHellGame.Presentation.Scenes;
using Microsoft.Xna.Framework.Content;
using System.Linq;

public class MainMenuScene : IScene
{
    private GameTime _gameTime;
    private Texture2D whitePixel;
    private SpriteData background;
    private ContentManager _contentManager;
    private GraphicsDevice _graphicsDevice;
    private int selectedIndex;
    private string[] menuOptions = { "Start Game", "Settings", "Exit" };

    public MainMenuScene(ContentManager contentManager, GraphicsDevice graphicsDevice)
    {
        this._contentManager = contentManager;
        this._graphicsDevice = graphicsDevice;
    }

    public void Load()
    {
        // Load with contentmanager
        background = TextureManager.Instance.GetSpriteData("MainMenu");
        FontManager.Instance.LoadFont(_contentManager, "DFPPOPCorn-W12");

        // Create a 1x1 white pixel texture for the box
        whitePixel = new Texture2D(_graphicsDevice, 1, 1);
        whitePixel.SetData(new Color[] { Color.White });
    }

    public void Update(GameTime gameTime)
    {
        _gameTime = gameTime;

        if (InputManager.Instance.ActionPressed(GameAction.MenuUp))
        {
            selectedIndex--;
            if (selectedIndex < 0)
                selectedIndex = menuOptions.Length - 1; // Wrap to last option
        }
        if (InputManager.Instance.ActionPressed(GameAction.MenuDown))
        {
            selectedIndex++;
            if (selectedIndex >= menuOptions.Length)
                selectedIndex = 0; // Wrap to first option
        }

        if (InputManager.Instance.ActionPressed(GameAction.Select))
        {
            switch (selectedIndex)
            {
                case 0:
                    SceneManager.Instance.AddScene(new TestScene(_contentManager, _graphicsDevice));
                    break;
                case 1:
                    SceneManager.Instance.AddScene(new SettingsScene(_contentManager, _graphicsDevice));
                    break;
                case 2:
                    Environment.Exit(0);
                    break;
            }
        }
    }


    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(background.Texture, Vector2.Zero, background.Animations.First().Value[0], Color.White);

        // Draw the menu options with outline and highlight box for the selected option
        for (int i = 0; i < menuOptions.Length; i++)
        {
            Vector2 position = new Vector2(100, 100 + i * 40);

            if (i == selectedIndex)
            {
                // Animate the box
                float time = (float)(_gameTime?.TotalGameTime.TotalSeconds ?? 0f);
                float offsetX = (float)Math.Sin(time * 5) * 3; // Oscillation effect
                float offsetY = (float)Math.Cos(time * 5) * 3; // Oscillation effect
                float rotation = (float)Math.Sin(time * 3) * 0.1f; // Small rotation effect

                // Draw a box around the selected option with the animation
                var textSize = FontManager.Instance.GetFont("DFPPOPCorn-W12").MeasureString(menuOptions[i]);
                var boxPosition = position - new Vector2(5, 5) + new Vector2(offsetX, offsetY);
                var boxSize = new Vector2(textSize.X + 10, textSize.Y + 10);

                // Draw the box with rotation
                spriteBatch.Draw(whitePixel, new Rectangle((int)boxPosition.X, (int)boxPosition.Y, (int)boxSize.X, (int)boxSize.Y), Color.White);

                // Outline effect for selected option
                DrawOutlinedText(spriteBatch, menuOptions[i], position + new Vector2(offsetX, offsetY), Color.Black, 2);
            }

            // Draw the actual menu option text
            Color textColor = (i == selectedIndex) ? Color.Red : Color.White;
            spriteBatch.DrawString(FontManager.Instance.GetFont("DFPPOPCorn-W12"), menuOptions[i], position, textColor);
        }
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
