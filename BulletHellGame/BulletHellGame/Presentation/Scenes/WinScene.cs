using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Managers;
using Microsoft.Xna.Framework.Content;

namespace BulletHellGame.Presentation.Scenes
{
    public class WinScene : IScene
    {
        private Texture2D whitePixel;
        private int selectedIndex;
        private string[] menuOptions = { "Play Again", "Settings", "Exit to Main Menu" };
        private ContentManager _contentManager;
        private GraphicsDevice _graphicsDevice;
        private CharacterData _characterData;

        public bool IsOverlay => true;

        public WinScene(ContentManager contentManager, GraphicsDevice graphicsDevice, CharacterData characterData)
        {
            _contentManager = contentManager;
            _graphicsDevice = graphicsDevice;
            _characterData = characterData;
        }

        public void Load()
        {
            FontManager.Instance.LoadFont(_contentManager, "DFPPOPCorn-W12");
            BGMManager.Instance.PlayBGM(_contentManager, "CallToAdventure");

            // Create a 1x1 white pixel texture for the _stageBackground
            whitePixel = new Texture2D(_graphicsDevice, 1, 1);
            whitePixel.SetData(new Color[] { Color.White });
        }

        public void Update(GameTime gameTime)
        {
            // Navigation
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
                        // Resume Game
                        SceneManager.Instance.RemoveScene();
                        SceneManager.Instance.RemoveScene();
                        SceneManager.Instance.AddScene(new TestLMScene(_contentManager, _graphicsDevice,_characterData));

                        break;
                    case 1:
                        // Go to settings
                        SceneManager.Instance.AddScene(new SettingsScene(_contentManager, _graphicsDevice));
                        break;
                    case 2:
                        // Exit to Main Menu
                        SceneManager.Instance.ClearScenes();
                        SceneManager.Instance.AddScene(new MainMenuScene(_contentManager, _graphicsDevice));
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Get window size
            int screenWidth = spriteBatch.GraphicsDevice.Viewport.Width;
            int screenHeight = spriteBatch.GraphicsDevice.Viewport.Height;

            // Semi-transparent overlay
            spriteBatch.Draw(whitePixel, new Rectangle(0, 0, screenWidth, screenHeight), Color.Black * 0.5f);

            // Define the menu box size
            int boxWidth = screenWidth / 2;
            int boxHeight = screenHeight / 2;
            int boxX = (screenWidth - boxWidth) / 2;
            int boxY = (screenHeight - boxHeight) / 2;

            // Draw _stageBackground box
            spriteBatch.Draw(whitePixel, new Rectangle(boxX, boxY, boxWidth, boxHeight), Color.Black * 0.9f);

            // Draw "Level Complete!" text
            string levelCompleteText = "Level Complete!";
            Vector2 levelCompleteSize = FontManager.Instance.GetFont("DFPPOPCorn-W12").MeasureString(levelCompleteText);
            Vector2 levelCompletePosition = new Vector2((screenWidth - levelCompleteSize.X) / 2, boxY + 20);
            DrawOutlinedText(spriteBatch, levelCompleteText, levelCompletePosition, Color.Black, 2);
            spriteBatch.DrawString(FontManager.Instance.GetFont("DFPPOPCorn-W12"), levelCompleteText, levelCompletePosition, Color.Yellow);

            // Draw menu options
            for (int i = 0; i < menuOptions.Length; i++)
            {
                Vector2 textSize = FontManager.Instance.GetFont("DFPPOPCorn-W12").MeasureString(menuOptions[i]);
                Vector2 position = new Vector2(boxX + (boxWidth - textSize.X) / 2, boxY + 100 + i * 50);

                if (i == selectedIndex)
                {
                    // Highlight box
                    spriteBatch.Draw(whitePixel, new Rectangle((int)position.X - 10, (int)position.Y - 5, (int)textSize.X + 20, (int)textSize.Y + 10), Color.White);

                    // Draw outline effect
                    DrawOutlinedText(spriteBatch, menuOptions[i], position, Color.Black, 2);
                }

                // Draw the text
                Color textColor = i == selectedIndex ? Color.Red : Color.White;
                spriteBatch.DrawString(FontManager.Instance.GetFont("DFPPOPCorn-W12"), menuOptions[i], position, textColor);
            }
        }


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
