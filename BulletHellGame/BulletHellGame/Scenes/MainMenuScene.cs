using BulletHellGame.Managers;
using Microsoft.Xna.Framework.Content;

namespace BulletHellGame.Scenes
{
    public class MainMenuScene : IScene
    {
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
                        SceneManager.Instance.AddScene(new GameplayScene(this._contentManager), Transitions.Fade);
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
            // Draw the menu options to the screen
            for (int i = 0; i < menuOptions.Length; i++)
            {
                Color color = (i == selectedIndex) ? Color.Red : Color.White;
                // Set up the font for drawing text
                spriteBatch.DrawString(FontManager.Instance.GetFont("Arial"), menuOptions[i], new Vector2(100, 100 + i * 40), color);
            }
        }
    }
}