using BulletHellGame.Logic.Managers;
using BulletHellGame.Presentation.UI.Menu;
using Microsoft.Xna.Framework.Content;

namespace BulletHellGame.Presentation.Scenes
{
    public class PausedScene : IScene
    {
        private Texture2D whitePixel;
        private ContentManager _contentManager;
        private GraphicsDevice _graphicsDevice;
        private MenuNavigator menuNavigator;
        private List<MenuOption> menuOptions;
        private GameTime _gameTime;

        public bool IsOverlay => true;
        public bool IsMenu => true;

        public PausedScene(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            _contentManager = contentManager;
            _graphicsDevice = graphicsDevice;

            var style = new MenuOptionStyle1(contentManager, graphicsDevice);
            menuOptions = new List<MenuOption>
            {
                new MenuOption("Resume", () => SceneManager.Instance.RemoveScene(), style),
                new MenuOption("Settings", () => SceneManager.Instance.AddScene(new SettingsScene(_contentManager, _graphicsDevice)), style),
                new MenuOption("Exit to Main Menu", () =>
                {
                    SceneManager.Instance.ClearScenes();
                    SceneManager.Instance.AddScene(new MainMenuScene(_contentManager, _graphicsDevice));
                }, style)
            };

            menuNavigator = new MenuNavigator(menuOptions.Count);
        }

        public void Load()
        {
            FontManager.Instance.LoadFont(_contentManager, "DFPPOPCorn-W12");

            whitePixel = new Texture2D(_graphicsDevice, 1, 1);
            whitePixel.SetData(new Color[] { Color.White });

            BGMManager.Instance.PauseBGM();
        }

        public void Unload()
        {
            BGMManager.Instance.ResumeBGM();
        }

        public void Update(GameTime gameTime)
        {
            _gameTime = gameTime;

            menuNavigator.Update(gameTime);

            if (InputManager.Instance.ActionPressed(GameAction.Select))
            {
                menuOptions[menuNavigator.SelectedIndex].ExecuteAction();
            }

            if (InputManager.Instance.ActionPressed(GameAction.Pause))
            {
                SceneManager.Instance.RemoveScene();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int screenWidth = spriteBatch.GraphicsDevice.Viewport.Width;
            int screenHeight = spriteBatch.GraphicsDevice.Viewport.Height;

            // Dim the background
            spriteBatch.Draw(whitePixel, new Rectangle(0, 0, screenWidth, screenHeight), Color.Black * 0.5f);

            // Semi-transparent box in the center
            int boxWidth = screenWidth / 2;
            int boxHeight = screenHeight / 2;
            int boxX = (screenWidth - boxWidth) / 2;
            int boxY = (screenHeight - boxHeight) / 2;

            spriteBatch.Draw(whitePixel, new Rectangle(boxX, boxY, boxWidth, boxHeight), Color.Black * 0.9f);

            // Draw each menu option
            for (int i = 0; i < menuOptions.Count; i++)
            {
                // Center the text inside the box
                Vector2 textSize = FontManager.Instance.GetFont("DFPPOPCorn-W12").MeasureString(menuOptions[i].Text);
                Vector2 position = new Vector2(
                    boxX + (boxWidth - textSize.X) / 2,
                    boxY + 50 + i * 50
                );

                menuOptions[i].Draw(spriteBatch, _gameTime, i, position, menuNavigator.SelectedIndex);
            }
        }
    }
}
