using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Managers;
using BulletHellGame.Presentation.UI.Menu;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace BulletHellGame.Presentation.Scenes
{
    public class RetryMenuScene : IScene
    {
        private SpriteFont _font;
        private Texture2D _backgroundTexture;
        private Rectangle _menuLocation;
        private ContentManager _contentManager;
        private GraphicsDevice _graphicsDevice;
        private CharacterData _characterData;
        private MenuNavigator menuNavigator;
        private List<MenuOption> menuOptions;
        private GameTime _gameTime;

        public bool IsOverlay => true;
        public bool IsMenu => true;

        public RetryMenuScene(Rectangle menuLocation, Texture2D backgroundTexture, ContentManager contentManager, GraphicsDevice graphicsDevice, CharacterData characterData)
        {
            _menuLocation = menuLocation;
            _backgroundTexture = backgroundTexture;
            _contentManager = contentManager;
            _graphicsDevice = graphicsDevice;
            _characterData = characterData;

            _font = FontManager.Instance.GetFont("DFPPOPCorn-W12");

            var style = new MenuOptionStyle1(contentManager, graphicsDevice);
            menuOptions = new List<MenuOption>
            {
                new MenuOption("Yes", () =>
                {
                    SceneManager.Instance.RemoveScene(); // Remove RetryMenu
                    SceneManager.Instance.RemoveScene(); // Remove Game Over or Pause?
                    SceneManager.Instance.AddScene(new TestLMScene(_contentManager, _graphicsDevice, _characterData));
                }, style),

                new MenuOption("No", () =>
                {
                    SceneManager.Instance.ClearScenes();
                    SceneManager.Instance.AddScene(new MainMenuScene(_contentManager, _graphicsDevice));
                }, style)
            };

            menuNavigator = new MenuNavigator(menuOptions.Count);
        }

        public void Load()
        {
            BGMManager.Instance.StopBGM();
        }

        public void Update(GameTime gameTime)
        {
            _gameTime = gameTime;

            menuNavigator.Update(gameTime);

            if (InputManager.Instance.ActionPressed(GameAction.Select))
            {
                menuOptions[menuNavigator.SelectedIndex].ExecuteAction();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw background overlay
            spriteBatch.Draw(_backgroundTexture, _menuLocation, Color.Black * 0.7f);

            // Center of the menu location
            Vector2 menuCenter = new Vector2(_menuLocation.X + _menuLocation.Width / 2, _menuLocation.Y + _menuLocation.Height / 2);

            // Draw "Retry?" message
            string message = "Retry?";
            Vector2 messageSize = _font.MeasureString(message);
            Vector2 messagePos = new Vector2(menuCenter.X - messageSize.X / 2, _menuLocation.Y + 20);
            spriteBatch.DrawString(_font, message, messagePos, Color.White);

            // Calculate vertical layout
            float optionSpacing = 40f;
            float totalOptionsHeight = menuOptions.Count * _font.LineSpacing + (menuOptions.Count - 1) * (optionSpacing - _font.LineSpacing);
            float startY = menuCenter.Y - totalOptionsHeight / 2;

            // Draw menu options
            for (int i = 0; i < menuOptions.Count; i++)
            {
                Vector2 optionSize = _font.MeasureString(menuOptions[i].Text);
                Vector2 optionPos = new Vector2(menuCenter.X - optionSize.X / 2, startY + i * optionSpacing);

                menuOptions[i].Draw(spriteBatch, _gameTime, i, optionPos, menuNavigator.SelectedIndex);
            }
        }
    }
}
