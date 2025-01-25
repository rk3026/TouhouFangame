using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Managers;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace BulletHellGame.Scenes
{
    public class MainMenuScene : IScene
    {
        private SpriteData background;
        private ContentManager _contentManager;
        private int selectedIndex;
        private string[] menuOptions = { "Start Game", "Settings", "Exit" };

        public MainMenuScene(ContentManager contentManager)
        {
            this._contentManager = contentManager;
        }

        public void Load()
        {
            // Load with contentmanager
            background = TextureManager.Instance.GetSpriteData("MainMenu");
            FontManager.Instance.LoadFont(_contentManager, "DFPPOPCorn-W12");
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
                        /*
                        LevelData leveldata = LevelManager.LoadLevel("level1");
                        SceneManager.Instance.AddScene(new GameplayScene(this._contentManager, leveldata), Transitions.Fade);
                        */
                        SceneManager.Instance.AddScene(new TestScene(_contentManager));
                        break;
                    case 1:
                        // Handle Settings
                        //SceneManager.Instance.AddScene(new SettingsScene(this._contentManager), Transitions.Fade);
                        //SceneManager.Instance.Update();
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

            // Draw the menu options to the screen
            for (int i = 0; i < menuOptions.Length; i++)
            {
                Color color = (i == selectedIndex) ? Color.Red : Color.White;
                // Set up the font for drawing text
                spriteBatch.DrawString(FontManager.Instance.GetFont("DFPPOPCorn-W12"), menuOptions[i], new Vector2(100, 100 + i * 40), color);
            }
        }
    }
}