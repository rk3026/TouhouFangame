using BulletHellGame.DataAccess.DataTransferObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using BulletHellGame.Logic.Managers;

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
        private Texture2D _characterSprite;
        private int _currentDialogueIndex = 0;

        public bool IsOverlay => false;

        public CutsceneScene(ContentManager contentManager, GraphicsDevice graphicsDevice, CutsceneData cutsceneData, CharacterData characterData)
        {
            _contentManager = contentManager;
            _graphicsDevice = graphicsDevice;
            _cutsceneData = cutsceneData;
            _characterData = characterData;
        }

        public void Load()
        {
            _font = _contentManager.Load<SpriteFont>("Fonts/DFPPOPCorn-W12");
            _backgroundTexture = _contentManager.Load<Texture2D>(_cutsceneData.BackgroundAsset);
            _characterSprite = _contentManager.Load<Texture2D>(_cutsceneData.SpriteAsset);
        }

        public void Update(GameTime gameTime)
        {
            if (InputManager.Instance.ActionPressed(GameAction.Select))
            {
                _currentDialogueIndex++;
                if (_currentDialogueIndex >= _cutsceneData.Dialogue.Count)
                {
                    SceneManager.Instance.RemoveScene();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Draw background
            spriteBatch.Draw(_backgroundTexture, Vector2.Zero, Color.White);

            // Draw character sprite
            if (_characterSprite != null)
            {
                Vector2 characterPosition = new Vector2(100, _graphicsDevice.Viewport.Height - 300);
                spriteBatch.Draw(_characterSprite, characterPosition, Color.White);
            }

            // Draw dialogue box
            if (_currentDialogueIndex < _cutsceneData.Dialogue.Count)
            {
                var dialogueLine = _cutsceneData.Dialogue[_currentDialogueIndex];
                string speaker = dialogueLine.SpeakerName;
                string line = dialogueLine.Line;

                Vector2 speakerPosition = new Vector2(50, _graphicsDevice.Viewport.Height - 150);
                Vector2 linePosition = new Vector2(50, _graphicsDevice.Viewport.Height - 100);

                spriteBatch.DrawString(_font, $"{speaker}:", speakerPosition, Color.Yellow);
                spriteBatch.DrawString(_font, line, linePosition, Color.White);
            }

            spriteBatch.End();
        }
    }
}
