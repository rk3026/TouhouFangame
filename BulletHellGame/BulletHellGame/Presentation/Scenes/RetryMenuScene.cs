using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Managers;
using Microsoft.Xna.Framework.Content;

namespace BulletHellGame.Presentation.Scenes
{
    public class RetryMenuScene : IScene
    {
        private SpriteFont _font;
        private Texture2D _backgroundTexture;
        private int _selectedIndex = 0;
        private string[] _options = { "Yes", "No" };
        private Rectangle _menuLocation;
        private ContentManager _contentManager;
        private GraphicsDevice _graphicsDevice;
        private CharacterData _characterData;

        public bool IsOverlay => true;

        public RetryMenuScene(Rectangle menuLocation, Texture2D backgroundTexture, ContentManager contentManager, GraphicsDevice graphicsDevice, CharacterData characterData)
        {
            _menuLocation = menuLocation;
            _font = FontManager.Instance.GetFont("DFPPOPCorn-W12");
            _backgroundTexture = backgroundTexture;
            _contentManager = contentManager;
            _graphicsDevice = graphicsDevice;
            _characterData = characterData;
        }


        public void Load() { }

        public void Update(GameTime gameTime)
        {
            if (InputManager.Instance.ActionPressed(GameAction.MenuUp) && _selectedIndex > 0)
                _selectedIndex--;
            if (InputManager.Instance.ActionPressed(GameAction.MenuDown) && _selectedIndex < _options.Length - 1)
                _selectedIndex++;

            if (InputManager.Instance.ActionPressed(GameAction.Select))
            {
                if (_selectedIndex == 0)
                {
                    SceneManager.Instance.RemoveScene();
                    SceneManager.Instance.RemoveScene();
                    SceneManager.Instance.AddScene(new TestLMScene(_contentManager, _graphicsDevice, _characterData));
                }
                else
                {
                    SceneManager.Instance.ClearScenes();
                    SceneManager.Instance.AddScene(new MainMenuScene(_contentManager, _graphicsDevice));
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the background using _menuLocation
            spriteBatch.Draw(_backgroundTexture, _menuLocation, Color.Black * 0.7f);

            // Calculate the center of the menu location
            Vector2 menuCenter = new Vector2(_menuLocation.X + _menuLocation.Width / 2, _menuLocation.Y + _menuLocation.Height / 2);

            // Draw the "Retry?" message at the top of the menu
            string message = "Retry?";
            Vector2 messageSize = _font.MeasureString(message);
            Vector2 messagePos = new Vector2(menuCenter.X - messageSize.X / 2, _menuLocation.Y + 20);
            spriteBatch.DrawString(_font, message, messagePos, Color.White);

            // Calculate total height of options and spacing to center vertically
            float optionSpacing = 40f;
            float totalOptionsHeight = _options.Length * _font.LineSpacing + (_options.Length - 1) * (optionSpacing - _font.LineSpacing);
            float startY = menuCenter.Y - totalOptionsHeight / 2;

            // Draw each option centered within the menu location
            for (int i = 0; i < _options.Length; i++)
            {
                Vector2 optionSize = _font.MeasureString(_options[i]);
                Vector2 optionPos = new Vector2(menuCenter.X - optionSize.X / 2, startY + i * optionSpacing);

                Color color = i == _selectedIndex ? Color.Yellow : Color.White;
                spriteBatch.DrawString(_font, _options[i], optionPos, color);
            }
        }



    }
}
