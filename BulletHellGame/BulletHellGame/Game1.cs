using BulletHellGame.Managers;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BulletHellGame
{
    public class Game1 : Game
    {
        // Managers:
        private FontManager _fontManager;
        private SceneManager _sceneManager;
        private TextureManager _textureManager;
        private InputManager _inputManager;

        // Graphics:
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Independent scaling factors and matrix
        private float _scaleX;
        private float _scaleY;
        private Matrix _scaleMatrix;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Initialize globals and managers
            _sceneManager = SceneManager.Instance;
            _textureManager = TextureManager.Instance;
            _fontManager = FontManager.Instance;
            _inputManager = InputManager.Instance;
        }

        protected override void Initialize()
        {
            // Set initial graphics device settings
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = Globals.WindowSize.X;
            _graphics.PreferredBackBufferHeight = Globals.WindowSize.Y;
            _graphics.ApplyChanges();

            // Allow the window to be resized
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += UpdateWindowSize;

            // Calculate the initial scaling factors
            _scaleX = (float)_graphics.PreferredBackBufferWidth / Globals.WindowSize.X;  // Base width
            _scaleY = (float)_graphics.PreferredBackBufferHeight / Globals.WindowSize.Y; // Base height
            _scaleMatrix = Matrix.CreateScale(_scaleX, _scaleY, 1f);

            _spriteBatch = new SpriteBatch(this.GraphicsDevice);
            Globals.GraphicsDevice = this.GraphicsDevice;
            Globals.SpriteBatch = this._spriteBatch;

            base.Initialize();
        }

        private void UpdateWindowSize(object sender, EventArgs e)
        {
            // Update graphics device settings based on the new window size
            _graphics.PreferredBackBufferWidth = this.Window.ClientBounds.Width;
            _graphics.PreferredBackBufferHeight = this.Window.ClientBounds.Height;
            _graphics.ApplyChanges();

            // Recalculate the independent scaling factors and matrix
            _scaleX = (float)this.Window.ClientBounds.Width / Globals.WindowSize.X;  // Base width
            _scaleY = (float)this.Window.ClientBounds.Height / Globals.WindowSize.Y; // Base height
            _scaleMatrix = Matrix.CreateScale(_scaleX, _scaleY, 1f);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ContentManager content = Content;

            // Set up a default texture:
            Texture2D defaultTexture = new Texture2D(this.GraphicsDevice, 16, 16);
            Color[] colorData = new Color[16 * 16];
            int blockSize = 4;
            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    int blockX = x / blockSize;
                    int blockY = y / blockSize;
                    if ((blockX + blockY) % 2 == 0)
                    {
                        colorData[y * 16 + x] = Color.Black;
                    }
                    else
                    {
                        colorData[y * 16 + x] = Color.Pink;
                    }
                }
            }
            defaultTexture.SetData(colorData);
            TextureManager.Instance.SetDefaultTexture(defaultTexture);

            // Load all the TextureAtlases:
            TextureManager.Instance.LoadSpriteSheetData(content, "Data/SpriteSheets/Characters.json");
            TextureManager.Instance.LoadSpriteSheetData(content, "Data/SpriteSheets/EnemiesAndBosses.json");
            TextureManager.Instance.LoadSpriteSheetData(content, "Data/SpriteSheets/MenuAndOtherScreens.json");
            TextureManager.Instance.LoadSpriteSheetData(content, "Data/SpriteSheets/ProjectilesAndObjects.json");
            TextureManager.Instance.LoadSpriteSheetData(content, "Data/SpriteSheets/StagesTilesAndBackgrounds.json");
            TextureManager.Instance.LoadSpriteSheetData(content, "Data/SpriteSheets/Fonts.json");
            TextureManager.Instance.LoadSpriteSheetData(content, "Data/SpriteSheets/SidebarLoadAndPauseScreens.json");

            // Add the main menu scene to begin the game:
            _sceneManager.AddScene(new MainMenuScene(this.Content, this.GraphicsDevice));
        }

        protected override void Update(GameTime gameTime)
        {
            // Update managers:
            _sceneManager.Update(gameTime);
            _inputManager.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen
            GraphicsDevice.Clear(Color.Black);
            this._spriteBatch.Begin(transformMatrix: _scaleMatrix);
            this._sceneManager.Draw(this._spriteBatch);
            this._spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
