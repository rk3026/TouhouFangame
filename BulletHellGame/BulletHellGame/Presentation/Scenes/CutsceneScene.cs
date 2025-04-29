using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BulletHellGame.Presentation.Scenes
{
    public class CutsceneScene : IScene
    {
        private readonly ContentManager _contentManager;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly CutsceneData _cutsceneData;
        private readonly CharacterData _characterData;

        private SpriteFont _font;
        private Texture2D _backgroundTexture;
        private Texture2D _whitePixel;
        private Dictionary<string, SpriteData> _characterSprites;
        
        private int _currentDialogueIndex;
        private float _typeTimer;
        private string _visibleText;
        private bool _isTextComplete;
        private const float TYPE_DELAY = 0.03f;

        public bool IsOverlay => false;
        public bool IsMenu => false;

        public CutsceneScene(ContentManager contentManager, GraphicsDevice graphicsDevice, 
            CutsceneData cutsceneData, CharacterData characterData)
        {
            _contentManager = contentManager;
            _graphicsDevice = graphicsDevice;
            _cutsceneData = cutsceneData;
            _characterData = characterData;
            _currentDialogueIndex = 0;
            _visibleText = "";
            _isTextComplete = false;
            _characterSprites = new Dictionary<string, SpriteData>();
        }

        public void Load()
        {
            _font = FontManager.Instance.GetFont("DFPPOPCorn-W12");
            _backgroundTexture = _contentManager.Load<Texture2D>(_cutsceneData.BackgroundAsset);
            
            _whitePixel = new Texture2D(_graphicsDevice, 1, 1);
            _whitePixel.SetData(new[] { Color.White });

            // Load all unique character sprites
            var loadedSprites = new HashSet<string>();
            foreach (var line in _cutsceneData.Dialogue)
            {
                if (!loadedSprites.Contains(line.SpeakerName))
                {
                    var texture = _contentManager.Load<Texture2D>($"SpriteSheets/Cutscene{line.SpeakerName}");
                    _characterSprites[line.SpeakerName] = TextureManager.Instance.Create3x3SpriteSheet(
                        texture, $"{line.SpeakerName}Cutscene");
                    loadedSprites.Add(line.SpeakerName);
                }
            }

            if (!string.IsNullOrEmpty(_cutsceneData.MusicAsset))
            {
                BGMManager.Instance.PlayBGM(_contentManager, _cutsceneData.MusicAsset);
            }
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var currentLine = _cutsceneData.Dialogue[_currentDialogueIndex];

            if (!_isTextComplete)
            {
                _typeTimer += elapsed;
                if (_typeTimer >= TYPE_DELAY && _visibleText.Length < currentLine.Line.Length)
                {
                    _visibleText += currentLine.Line[_visibleText.Length];
                    _typeTimer = 0f;
                }
                
                if (_visibleText.Length == currentLine.Line.Length)
                {
                    _isTextComplete = true;
                }

                // Skip to end of current text
                if (InputManager.Instance.ActionPressed(GameAction.Select))
                {
                    _visibleText = currentLine.Line;
                    _isTextComplete = true;
                }
            }
            else if (InputManager.Instance.ActionPressed(GameAction.Select))
            {
                _currentDialogueIndex++;
                if (_currentDialogueIndex >= _cutsceneData.Dialogue.Count)
                {
                    SceneManager.Instance.RemoveScene();
                    SceneManager.Instance.AddScene(new StageIntroScene(_contentManager, _graphicsDevice, _characterData));
                }
                else
                {
                    _visibleText = "";
                    _isTextComplete = false;
                    _typeTimer = 0f;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _graphicsDevice.Clear(Color.Black);

            // Draw background
            spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, 
                _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.White);

            if (_currentDialogueIndex < _cutsceneData.Dialogue.Count)
            {
                var currentLine = _cutsceneData.Dialogue[_currentDialogueIndex];

                // Draw character sprite with expression
                if (_characterSprites.TryGetValue(currentLine.SpeakerName, out var spriteData) && 
                    spriteData.Animations.TryGetValue(currentLine.SpriteExpression, out var frames))
                {
                    Rectangle sourceRect = frames[0];
                    float scale = 1.0f;
                    Vector2 position = new Vector2(
                        _graphicsDevice.Viewport.Width / 2f - (sourceRect.Width * scale) / 2f,
                        _graphicsDevice.Viewport.Height / 2f - (sourceRect.Height * scale) / 2f - 50
                    );

                    spriteBatch.Draw(spriteData.Texture, position, sourceRect, Color.White);
                }

                // Draw dialogue box
                Rectangle textbox = new Rectangle(50, _graphicsDevice.Viewport.Height - 200, 
                    _graphicsDevice.Viewport.Width - 100, 150);
                spriteBatch.Draw(_whitePixel, textbox, Color.Black * 0.7f);

                // Draw speaker name and dialogue
                spriteBatch.DrawString(_font, $"{currentLine.SpeakerName}:", 
                    new Vector2(textbox.X + 20, textbox.Y + 20), Color.Yellow);
                spriteBatch.DrawString(_font, _visibleText,
                    new Vector2(textbox.X + 20, textbox.Y + 60), Color.White);

                // Draw continue indicator when text is complete
                if (_isTextComplete)
                {
                    var continueText = "▼";
                    var textSize = _font.MeasureString(continueText);
                    float alpha = 0.5f + (float)System.Math.Sin(gameTime.TotalGameTime.TotalSeconds * 4) * 0.25f;
                    spriteBatch.DrawString(_font, continueText,
                        new Vector2(textbox.Right - textSize.X - 20, textbox.Bottom - textSize.Y - 10),
                        Color.White * alpha);
                }
            }
        }
    }
}