using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BulletHellGame.Presentation.Scenes
{
    public class StageIntroScene : IScene
    {
        private readonly ContentManager _contentManager;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly CharacterData _characterData;
        
        private SpriteFont _font;
        private Texture2D _whitePixel;
        private float _timer;
        private float _alpha;
        private Vector2 _textPosition;
        private const float DISPLAY_DURATION = 2.5f;
        private const string STAGE_TEXT = "STAGE 1 START!";
        private Color _textColor;
        private Vector2[] _outlineOffsets;
        private const float FADE_DURATION = 0.5f;

        public bool IsOverlay => false;
        public bool IsMenu => false;

        public StageIntroScene(ContentManager contentManager, GraphicsDevice graphicsDevice, CharacterData characterData)
        {
            _contentManager = contentManager;
            _graphicsDevice = graphicsDevice;
            _characterData = characterData;
            _timer = 0f;
            _alpha = 0f;

            float outlineSize = 2f;
            _outlineOffsets = new[]
            {
                new Vector2(-outlineSize, -outlineSize),
                new Vector2(-outlineSize, outlineSize),
                new Vector2(outlineSize, -outlineSize),
                new Vector2(outlineSize, outlineSize)
            };
        }

        public void Load()
        {
            _font = FontManager.Instance.GetFont("DFPPOPCorn-W12");
            _whitePixel = new Texture2D(_graphicsDevice, 1, 1);
            _whitePixel.SetData(new[] { Color.White });

            // Calculate and store text position
            Vector2 textSize = _font.MeasureString(STAGE_TEXT);
            _textPosition = new Vector2(
                (_graphicsDevice.Viewport.Width - textSize.X) / 2,
                (_graphicsDevice.Viewport.Height - textSize.Y) / 2
            );
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _timer += deltaTime;

            // Calculate alpha for fade effects
            if (_timer < FADE_DURATION)
            {
                _alpha = _timer / FADE_DURATION;
            }
            else if (_timer > DISPLAY_DURATION - FADE_DURATION)
            {
                _alpha = (DISPLAY_DURATION - _timer) / FADE_DURATION;
            }
            else
            {
                _alpha = 1f;
            }

            _textColor = Color.White * _alpha;

            // Move to game scene when done or when player presses action button
            if (_timer >= DISPLAY_DURATION || InputManager.Instance.ActionPressed(GameAction.Select))
            {
                SceneManager.Instance.RemoveScene();
                SceneManager.Instance.AddScene(new TestLMScene(_contentManager, _graphicsDevice, _characterData));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.Black);

            // Draw text outline
            Color outlineColor = Color.Black * _alpha;
            foreach (var offset in _outlineOffsets)
            {
                spriteBatch.DrawString(_font, STAGE_TEXT, _textPosition + offset, outlineColor);
            }

            // Draw main text
            spriteBatch.DrawString(_font, STAGE_TEXT, _textPosition, _textColor);
        }

        public void Dispose()
        {
            _whitePixel?.Dispose();
        }
    }
}
