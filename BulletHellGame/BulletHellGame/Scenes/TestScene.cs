using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;
using BulletHellGame.UI;
using BulletHellGame.Utilities.EntityGenerator;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace BulletHellGame.Scenes
{
    public class TestScene : IScene
    {
        // Managers and Graphics:
        private EntityManager _entityManager;
        private WaveManager _waveManager;
        private SystemManager _systemManager;
        private ContentManager _contentManager;
        private GraphicsDevice _graphicsDevice;

        // Sprite/Texture Assets:
        private SpriteData _sidebarBackground;
        private SpriteData _stageBackground;
        private SpriteData _bush1Sprite;
        private SpriteData _bush2Sprite;
        private Texture2D whitePixel;
        private SpriteFont _font;

        // UI Elements:
        private ParallaxBackground _parallaxBackground;
        private GameUI _gameUI;
        private EnemyIndicatorRenderer _enemyIndicatorRenderer;

        // Scene Data:
        private Rectangle _playableArea;
        private Rectangle _uiArea;
        private const int _playableAreaOffset = 15; // Offset for the playable area on all sides

        public TestScene(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            this._playableArea = new Rectangle(
                _playableAreaOffset,
                _playableAreaOffset,
                graphicsDevice.Viewport.Width * 2 / 3 - 2 * _playableAreaOffset,
                graphicsDevice.Viewport.Height - 2 * _playableAreaOffset
            );
            this._uiArea = new Rectangle(_playableArea.Right, 0, graphicsDevice.Viewport.Width - _playableArea.Width, graphicsDevice.Viewport.Height);
            this._enemyIndicatorRenderer = new EnemyIndicatorRenderer(graphicsDevice);

            this._contentManager = contentManager;
            this._graphicsDevice = graphicsDevice;
            this._entityManager = new EntityManager(this._playableArea);
            this._waveManager = new WaveManager(_entityManager);
            this._systemManager = new SystemManager(this._graphicsDevice);

            // Load the player entity into the scene
            CharacterData cd = EntityDataGenerator.CreateCharacterData();
            this._entityManager.SpawnPlayer(cd);
        }

        public void Load()
        {
            // Load font and textures
            FontManager.Instance.LoadFont(_contentManager, "DFPPOPCorn-W12");
            _font = FontManager.Instance.GetFont("DFPPOPCorn-W12");
            _sidebarBackground = TextureManager.Instance.GetSpriteData("SidebarBackground");
            _stageBackground = TextureManager.Instance.GetSpriteData("Level1.Background");
            _bush1Sprite = TextureManager.Instance.GetSpriteData("Level1.Bush1");
            _bush2Sprite = TextureManager.Instance.GetSpriteData("Level1.Bush2");

            // Create a 1x1 white pixel texture
            whitePixel = new Texture2D(_graphicsDevice, 1, 1);
            whitePixel.SetData(new Color[] { Color.White });

            _parallaxBackground = new ParallaxBackground();

            // Add background layer (stretched to fill the playable area)
            _parallaxBackground.AddLayer(
                _stageBackground.Texture,
                _stageBackground.Animations.First().Value.First(),
                _playableArea,
                speed: 100f
            );

            // Determine bush width from sprite data
            int bushWidth = _bush1Sprite.Animations.First().Value.First().Width;

            // Define bush areas on the left and right sides
            Rectangle leftBushArea = new Rectangle(_playableArea.Left, _playableArea.Top, bushWidth, _playableArea.Height);
            Rectangle rightBushArea = new Rectangle(_playableArea.Right - bushWidth, _playableArea.Top, bushWidth, _playableArea.Height);

            // Add left and right bushes as parallax layers
            _parallaxBackground.AddLayer(_bush1Sprite.Texture, _bush1Sprite.Animations.First().Value.First(), leftBushArea, 100f);
            _parallaxBackground.AddLayer(_bush2Sprite.Texture, _bush2Sprite.Animations.First().Value.First(), rightBushArea, 100f);

            _gameUI = new GameUI(_font, _uiArea, _entityManager);
        }

        public void Update(GameTime gameTime)
        {
            // Update all entities
            _systemManager.Update(_entityManager, gameTime);
            _waveManager.Update(gameTime);

            if (InputManager.Instance.ActionPressed(GameAction.Pause))
            {
                SceneManager.Instance.AddScene(new PausedScene(this._contentManager, this._graphicsDevice));
                return;
            }

            if (_entityManager.GetEntityCount(EntityType.Player) == 0) // if (playerLives == 0) show retry menu
            {
                SceneManager.Instance.AddScene(new RetryMenuScene(_font, whitePixel, new Vector2(_playableArea.Width / 2, _playableArea.Height / 2), _contentManager, _graphicsDevice));
            }

            _parallaxBackground.Update(gameTime);
            _gameUI.Update(gameTime);

            if (InputManager.Instance.KeyPressed(Keys.F))
                SpawnEnemyGrid(5, 50);

            if (InputManager.Instance.KeyPressed(Keys.G))
                SpawnBoss();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //if (_waveManager.IsWaveComplete())
            //{
            //    Draw a "Wave Complete" message
            //}

            _parallaxBackground.Draw(spriteBatch);

            // DrawActiveShader all entities within the playable area
            _systemManager.Draw(_entityManager, spriteBatch);

            // Drawing the sidebar UI background
            Rectangle sourceRect = _sidebarBackground.Animations.First().Value.First();
            int tileWidth = sourceRect.Width;
            int tileHeight = sourceRect.Height;
            for (int y = 0; y < spriteBatch.GraphicsDevice.Viewport.Height; y += tileHeight)
                spriteBatch.Draw(_sidebarBackground.Texture, new Rectangle(0, y, _playableArea.Left, Math.Min(tileHeight, spriteBatch.GraphicsDevice.Viewport.Height - y)), sourceRect, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            for (int y = 0; y < spriteBatch.GraphicsDevice.Viewport.Height; y += tileHeight)
                spriteBatch.Draw(_sidebarBackground.Texture, new Rectangle(_playableArea.Right, y, spriteBatch.GraphicsDevice.Viewport.Width - _playableArea.Right, Math.Min(tileHeight, spriteBatch.GraphicsDevice.Viewport.Height - y)), sourceRect, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            for (int x = _playableArea.Left; x < _playableArea.Right; x += tileWidth)
                spriteBatch.Draw(_sidebarBackground.Texture, new Rectangle(x, 0, Math.Min(tileWidth, _playableArea.Right - x), _playableArea.Top), sourceRect, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            for (int x = _playableArea.Left; x < _playableArea.Right; x += tileWidth)
                spriteBatch.Draw(_sidebarBackground.Texture, new Rectangle(x, _playableArea.Bottom, Math.Min(tileWidth, _playableArea.Right - x), spriteBatch.GraphicsDevice.Viewport.Height - _playableArea.Bottom), sourceRect, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            
            _enemyIndicatorRenderer.Draw(_entityManager, spriteBatch);
            _gameUI.Draw(spriteBatch);
        }

        private void SpawnEnemyGrid(int gridSize, float offset)
        {
            float baseSpawnX = _playableArea.Center.X;
            float baseSpawnY = _playableArea.Top + offset;

            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    float spawnX = baseSpawnX + (col - (gridSize / 2)) * offset;
                    float spawnY = baseSpawnY + row * offset;

                    spawnX = MathHelper.Clamp(spawnX, _playableArea.Left, _playableArea.Right);
                    spawnY = MathHelper.Clamp(spawnY, _playableArea.Top, _playableArea.Bottom);

                    _entityManager.SpawnEnemy(EntityDataGenerator.CreateEnemyData(), new Vector2(spawnX, spawnY));
                }
            }
        }

        private void SpawnBoss()
        {
            BossData bossData = EntityDataGenerator.CreateBossData();
            Vector2 spawnPos = new Vector2(_entityManager.Bounds.Width / 2, _entityManager.Bounds.Height / 10);
            _entityManager.SpawnBoss(bossData, spawnPos);
        }
    }
}
