﻿using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;
using BulletHellGame.Logic.Utilities.EntityDataGenerator;
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

        // Assets & UI
        private SpriteData _sidebarBackground, _stageBackground, _bush1Sprite, _bush2Sprite;
        private Texture2D whitePixel;
        private SpriteFont _font;
        private ParallaxBackground _parallaxBackground;
        private GameUI _gameUI;
        private EnemyIndicatorRenderer _enemyIndicatorRenderer;

        // Scene layout
        private Rectangle _playableArea;
        private Rectangle _uiArea;
        private const int _playableAreaOffset = 15;

        // Transition
        private enum TransitionState { None, WaveComplete, BossIncoming, LevelComplete }
        private TransitionState _transitionState = TransitionState.None;
        private float _transitionTimer = 0f;
        private const float TransitionDuration = 2f;
        private string _transitionMessage = "";

        // Countdown
        private bool _isCountdownActive = false;
        private float _countdownTimer = 3f;
        private const float CountdownStart = 3f;

        public TestLMScene(ContentManager contentManager, GraphicsDevice graphicsDevice)
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
            _enemyIndicatorRenderer = new EnemyIndicatorRenderer(graphicsDevice);
            _levelManager = new LevelManager(_entityManager, _playableArea);

            _entityManager.SpawnPlayer(EntityDataGenerator.CreateCharacterData());
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
            _parallaxBackground.AddLayer(_stageBackground.Texture, _stageBackground.Animations.First().Value.First(), _playableArea, 100f);

            int bushWidth = _bush1Sprite.Animations.First().Value.First().Width;
            Rectangle leftBushArea = new Rectangle(_playableArea.Left, _playableArea.Top, bushWidth, _playableArea.Height);
            Rectangle rightBushArea = new Rectangle(_playableArea.Right - bushWidth, _playableArea.Top, bushWidth, _playableArea.Height);
            _parallaxBackground.AddLayer(_bush1Sprite.Texture, _bush1Sprite.Animations.First().Value.First(), leftBushArea, 100f);
            _parallaxBackground.AddLayer(_bush2Sprite.Texture, _bush2Sprite.Animations.First().Value.First(), rightBushArea, 100f);

            _gameUI = new GameUI(_font, _uiArea, _entityManager);

            _levelManager.StartLevel(1);
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _systemManager.Update(_entityManager, gameTime);
            _levelManager.Update(gameTime);
            _parallaxBackground.Update(gameTime);
            _gameUI.Update(gameTime);

            if (InputManager.Instance.ActionPressed(GameAction.Pause))
                SceneManager.Instance.AddScene(new PausedScene(_contentManager, _graphicsDevice));

            if (_entityManager.GetEntityCount(EntityType.Player) == 0)
                SceneManager.Instance.AddScene(new RetryMenuScene(_font, whitePixel, _contentManager, _graphicsDevice));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _parallaxBackground.Draw(spriteBatch);
            _systemManager.Draw(_entityManager, spriteBatch);

            DrawSidebar(spriteBatch);
            _enemyIndicatorRenderer.Draw(_entityManager, spriteBatch);
            _gameUI.Draw(spriteBatch);

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
