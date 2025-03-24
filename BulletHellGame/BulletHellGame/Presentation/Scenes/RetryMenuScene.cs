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
        private Vector2 _screenCenter;
        private ContentManager _contentManager;
        private GraphicsDevice _graphicsDevice;
        private CharacterData _characterData;

        public bool IsOverlay => true;

        public RetryMenuScene(SpriteFont font, Texture2D backgroundTexture, ContentManager contentManager, GraphicsDevice graphicsDevice, CharacterData characterData)
        {
            _font = font;
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
            // Recalculate screen center to handle window resizing
            _screenCenter = new Vector2(_graphicsDevice.Viewport.Width / 2, _graphicsDevice.Viewport.Height / 2);

            string message = "Retry?";
            Vector2 messageSize = _font.MeasureString(message);
            Vector2 messagePos = _screenCenter - messageSize / 2;

            Vector2 padding = new Vector2(40, 30);
            float maxWidth = messageSize.X;
            float totalHeight = messageSize.Y;

            Vector2[] optionPositions = new Vector2[_options.Length];

            for (int i = 0; i < _options.Length; i++)
            {
                Vector2 optionSize = _font.MeasureString(_options[i]);
                maxWidth = MathF.Max(maxWidth, optionSize.X);
                optionPositions[i] = _screenCenter + new Vector2(0, (i + 1) * 40) - optionSize / 2;
                totalHeight += optionSize.Y + 20;
            }

            Rectangle backgroundRect = new Rectangle(
                (int)(_screenCenter.X - maxWidth / 2 - padding.X / 2),
                (int)(messagePos.Y - padding.Y / 2),
                (int)(maxWidth + padding.X),
                (int)(totalHeight + padding.Y)
            );

            spriteBatch.Draw(_backgroundTexture, backgroundRect, Color.Black * 0.7f);
            spriteBatch.DrawString(_font, message, messagePos, Color.White);

            for (int i = 0; i < _options.Length; i++)
            {
                Color color = i == _selectedIndex ? Color.Yellow : Color.White;
                spriteBatch.DrawString(_font, _options[i], optionPositions[i], color);
            }
        }

    }
}
