using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;
using BulletHellGame.Presentation.UI;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace BulletHellGame.Presentation.Scenes
{
    public class TestLMScene : IScene
    {
        private EntityManager _entityManager;
        private LevelManager _levelManager;
        private SystemManager _systemManager;
        private ContentManager _contentManager;
        private GraphicsDevice _graphicsDevice;
        private ScreenFlipManager _screenFlipManager;

        // Assets & UI
        private SpriteData _sidebarBackground, _stageBackground, _bush1Sprite, _bush2Sprite;
        private Texture2D whitePixel;
        private SpriteFont _font;
        private ParallaxBackground _parallaxBackground;
        private GameUI _gameUI;
        private EnemyIndicatorRenderer _enemyIndicatorRenderer;
        private CharacterData _characterData;
        private Dictionary<string, string> phaseSongs;

        // Scene layout
        private Rectangle _playableArea;
        private Rectangle _uiArea;
        private const int _playableAreaOffset = 15;

        // Transition
        private enum TransitionState { None, WaveComplete, BossWave, LevelComplete }
        private TransitionState _transitionState = TransitionState.None;
        private float _transitionTimer = 0f;
        private const float TransitionDuration = 2f;
        private string _transitionMessage = "";

        public bool IsOverlay => false;
        public bool IsMenu => false;

        public TestLMScene(ContentManager contentManager, GraphicsDevice graphicsDevice, CharacterData characterData)
        {
            Rectangle sceneArea = new Rectangle(0, 0, 640, 480);
            int playableWidth = sceneArea.Width * 2 / 3;

            _playableArea = new Rectangle(_playableAreaOffset, _playableAreaOffset,
                playableWidth - 2 * _playableAreaOffset, sceneArea.Height - 2 * _playableAreaOffset);
            _uiArea = new Rectangle(playableWidth, 0, sceneArea.Width - playableWidth, sceneArea.Height);

            _contentManager = contentManager;
            _graphicsDevice = graphicsDevice;
            _entityManager = new EntityManager(_playableArea);
            _systemManager = new SystemManager(graphicsDevice);
            _screenFlipManager = new ScreenFlipManager(_playableArea);
            _enemyIndicatorRenderer = new EnemyIndicatorRenderer(graphicsDevice);
            _levelManager = new LevelManager(_entityManager, _playableArea);

            _characterData = characterData;
            _entityManager.SpawnPlayer(characterData);

            // Define phase songs
            phaseSongs = new Dictionary<string, string>
            {
                { "SubBoss", "激戦アレンジ 有頂天変  wonderful heaven 東方緋想天" },
                { "Boss", "激戦アレンジ U.N.オーエンは彼女なのか -11th- 東方紅魔郷" },
                { "Normal", "紅魔激走劇  Everlasting..."}
            };

            _levelManager.OnBossSpawned += () =>
            {
                
                var cutsceneData = CutsceneDataLoader.LoadCutsceneData("Level1Cutscenes");
                SceneManager.Instance.AddScene(new CutsceneScene(this._contentManager, this._graphicsDevice, cutsceneData, this._characterData));
                // Determine the current phase and play the corresponding song
                if (_levelManager.GetCurrentWaveType() == WaveType.SubBoss)
                {
                    BGMManager.Instance.PlayBGM(_contentManager, phaseSongs["SubBoss"]);
                }
                else if (_levelManager.GetCurrentWaveType() == WaveType.Boss)
                {
                    BGMManager.Instance.PlayBGM(_contentManager, phaseSongs["Boss"]);
                }

                _transitionState = TransitionState.BossWave;
                _transitionMessage = "Boss Incoming!";
                _transitionTimer = TransitionDuration;
            };

            _levelManager.OnBossDefeated += () =>
            {
                BGMManager.Instance.PlayBGM(_contentManager, phaseSongs["Normal"]);
            };
        }

        public void Load()
        {
            FontManager.Instance.LoadFont(_contentManager, "DFPPOPCorn-W12");
            _font = FontManager.Instance.GetFont("DFPPOPCorn-W12");

            _sidebarBackground = TextureManager.Instance.GetSpriteData("SidebarBackground");
            _stageBackground = TextureManager.Instance.GetSpriteData("Level1.Background");
            _bush1Sprite = TextureManager.Instance.GetSpriteData("Level1.Bush1");
            _bush2Sprite = TextureManager.Instance.GetSpriteData("Level1.Bush2");

            whitePixel = new Texture2D(_graphicsDevice, 1, 1);
            whitePixel.SetData(new Color[] { Color.White });

            _parallaxBackground = new ParallaxBackground();
            _parallaxBackground.AddLayer(_stageBackground.Texture, _stageBackground.Animations.First().Value.First(), _playableArea, -100f);

            int bushWidth = _bush1Sprite.Animations.First().Value.First().Width;
            Rectangle leftBushArea = new Rectangle(_playableArea.Left, _playableArea.Top, bushWidth, _playableArea.Height);
            Rectangle rightBushArea = new Rectangle(_playableArea.Right - bushWidth, _playableArea.Top, bushWidth, _playableArea.Height);
            _parallaxBackground.AddLayer(_bush1Sprite.Texture, _bush1Sprite.Animations.First().Value.First(), leftBushArea, -100f);
            _parallaxBackground.AddLayer(_bush2Sprite.Texture, _bush2Sprite.Animations.First().Value.First(), rightBushArea, -100f);

            _gameUI = new GameUI(_font, _uiArea, _entityManager);

            List<string> bgms = new()
            {
                "紅魔激走劇  Everlasting...",
                //"激戦アレンジ U.N.オーエンは彼女なのか -11th- 東方紅魔郷",
                //"広有射怪鳥事Till When原曲広有射怪鳥事Till When"
            };
            string bgm = bgms[new Random().Next(bgms.Count)];

            BGMManager.Instance.PlayBGM(_contentManager, bgm);
            _levelManager.StartLevel(1);
        }


        public void Update(GameTime gameTime)
        {
            
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            HandleTransitionStates(gameTime);

            _screenFlipManager.Update(_entityManager, gameTime);
            _systemManager.Update(_entityManager, gameTime);
            ParticleEffectManager.Instance.Update(gameTime);
            _levelManager.Update(gameTime);
            _parallaxBackground.Update(gameTime);
            _gameUI.Update(gameTime);

            if (InputManager.Instance.ActionPressed(GameAction.Pause))
                SceneManager.Instance.AddScene(new PausedScene(_contentManager, _graphicsDevice));

            if (_entityManager.GetEntityCount(EntityType.Player) == 0)
            {
                int menuPadding = 40;
                Rectangle menuLocation = new Rectangle(
                    _playableArea.X + menuPadding,
                    _playableArea.Y + menuPadding,
                    _playableArea.Width - menuPadding * 2,
                    _playableArea.Height - menuPadding * 2
                );

                SceneManager.Instance.AddScene(new RetryMenuScene(menuLocation, whitePixel, _contentManager, _graphicsDevice, _characterData));
            }

            if (_levelManager.LevelComplete() && !_levelManager.StartNextLevel())
            {
                Rectangle menuLocation = new Rectangle(
                    _playableArea.X + 40,
                    _playableArea.Y + 40,
                    _playableArea.Width - 80,
                    _playableArea.Height - 80
                );
                SceneManager.Instance.AddScene(new WinScene(menuLocation, _contentManager, _graphicsDevice, _characterData));
            }
        }

        private void HandleTransitionStates(GameTime gameTime)
        {
            // Check for wave transitions (e.g., new wave, wave complete, boss incoming)
            if (_levelManager.WaveJustCompleted)
            {
                _transitionState = TransitionState.WaveComplete;
                _transitionMessage = "Wave Complete!";
                _transitionTimer = TransitionDuration;
            }

            // Countdown for transition display
            if (_transitionTimer > 0)
            {
                _transitionTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _transitionState = TransitionState.None;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _screenFlipManager.Draw(spriteBatch, () =>
            {
                _systemManager.Draw(_entityManager, spriteBatch);
            }, (bgSpriteBatch) =>
            {
                _parallaxBackground.Draw(bgSpriteBatch, _screenFlipManager.CurrentFlip);
            });
            //_systemManager.Draw(_entityManager, spriteBatch);
            ParticleEffectManager.Instance.Draw(spriteBatch);

            DrawSidebar(spriteBatch);
            _enemyIndicatorRenderer.Draw(_entityManager, spriteBatch);
            _gameUI.Draw(spriteBatch);
            DrawTransitionMessage(spriteBatch);

            DrawWaveTimeLeft(spriteBatch);
        }


        private void DrawWaveTimeLeft(SpriteBatch spriteBatch)
        {
            // Get the current wave time left from the LevelManager
            float waveTimeLeft = _levelManager.GetCurrentWaveTimeLeft();

            string timeText = waveTimeLeft > 0
                ? $"{(int)(waveTimeLeft / 60)}:{waveTimeLeft % 60:00}"
                : "00:00";

            Vector2 position = new Vector2(_playableArea.Right - 55 , _playableArea.Top + 5);
            spriteBatch.DrawString(_font, timeText, position, Color.White);
        }

        private void DrawTransitionMessage(SpriteBatch spriteBatch)
        {
            if (_transitionState == TransitionState.None) return;

            Vector2 messagePosition = new Vector2(_playableArea.X + 100, _playableArea.Y + 50);

            // Calculate the alpha for the fade effect based on the transition timer
            // We want it to fade in and out once, so we just want a smooth 0 -> 1 -> 0 transition.
            float alpha = 1f - Math.Abs(2f * (_transitionTimer / TransitionDuration) - 1f);

            Color messageColor = Color.White * alpha;
            Color outlineColor = Color.Black * alpha;

            // Offset for the outline (you can adjust the offsets for thicker or thinner outline)
            float outlineOffset = 2f;

            // Draw the outline by rendering the text in different directions
            spriteBatch.DrawString(_font, _transitionMessage, messagePosition + new Vector2(-outlineOffset, -outlineOffset), outlineColor); // Top-Left
            spriteBatch.DrawString(_font, _transitionMessage, messagePosition + new Vector2(outlineOffset, -outlineOffset), outlineColor);  // Top-Right
            spriteBatch.DrawString(_font, _transitionMessage, messagePosition + new Vector2(-outlineOffset, outlineOffset), outlineColor); // Bottom-Left
            spriteBatch.DrawString(_font, _transitionMessage, messagePosition + new Vector2(outlineOffset, outlineOffset), outlineColor);  // Bottom-Right

            // Finally, draw the main message on top of the outline
            spriteBatch.DrawString(_font, _transitionMessage, messagePosition, messageColor);
        }

        private void DrawSidebar(SpriteBatch spriteBatch)
        {
            Rectangle sourceRect = _sidebarBackground.Animations.First().Value.First();
            int tileWidth = sourceRect.Width;
            int tileHeight = sourceRect.Height;

            for (int y = 0; y < spriteBatch.GraphicsDevice.Viewport.Height; y += tileHeight)
                spriteBatch.Draw(_sidebarBackground.Texture, new Rectangle(0, y, _playableArea.Left, Math.Min(tileHeight, spriteBatch.GraphicsDevice.Viewport.Height - y)), sourceRect, Color.White);

            for (int y = 0; y < spriteBatch.GraphicsDevice.Viewport.Height; y += tileHeight)
                spriteBatch.Draw(_sidebarBackground.Texture, new Rectangle(_playableArea.Right, y, spriteBatch.GraphicsDevice.Viewport.Width - _playableArea.Right, Math.Min(tileHeight, spriteBatch.GraphicsDevice.Viewport.Height - y)), sourceRect, Color.White);

            for (int x = _playableArea.Left; x < _playableArea.Right; x += tileWidth)
                spriteBatch.Draw(_sidebarBackground.Texture, new Rectangle(x, 0, Math.Min(tileWidth, _playableArea.Right - x), _playableArea.Top), sourceRect, Color.White);

            for (int x = _playableArea.Left; x < _playableArea.Right; x += tileWidth)
                spriteBatch.Draw(_sidebarBackground.Texture, new Rectangle(x, _playableArea.Bottom, Math.Min(tileWidth, _playableArea.Right - x), spriteBatch.GraphicsDevice.Viewport.Height - _playableArea.Bottom), sourceRect, Color.White);
        }
    }
}
