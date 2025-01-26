﻿using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Managers;
using BulletHellGame.Scenes;
using Microsoft.Xna.Framework.Content;
using System.Linq;

public class MainMenuScene : IScene
{
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
        // Update the menu logic (input handling, selection, etc.)
        if (InputManager.Instance.KeyPressed(Keys.W) && selectedIndex > 0)
            selectedIndex--;
        if (InputManager.Instance.KeyPressed(Keys.S) && selectedIndex < menuOptions.Length - 1)
            selectedIndex++;

        if (InputManager.Instance.KeyPressed(Keys.Enter))
        {
            // Handle selection
            switch (selectedIndex)
            {
                case 0:
                    // Change the current scene based on input
                    SceneManager.Instance.AddScene(new TestScene(_contentManager, _graphicsDevice));
                    break;
                case 1:
                    // Handle Settings
                    break;
                case 2:
                    // Exit the game
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
                // Draw a box around the selected option
                var textSize = FontManager.Instance.GetFont("DFPPOPCorn-W12").MeasureString(menuOptions[i]);
                var boxPosition = position - new Vector2(5, 5);
                var boxSize = new Vector2(textSize.X + 10, textSize.Y + 10);
                spriteBatch.Draw(whitePixel, new Rectangle((int)boxPosition.X, (int)boxPosition.Y, (int)boxSize.X, (int)boxSize.Y), Color.White);

                // Outline effect for selected option
                DrawOutlinedText(spriteBatch, menuOptions[i], position, Color.Black, 2);
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
