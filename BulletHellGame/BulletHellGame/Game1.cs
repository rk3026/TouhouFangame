using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Managers;
using BulletHellGame.Scenes;
using Microsoft.Xna.Framework.Content;

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

            // Load content

            // Load all spritesheets:
            ContentManager content = Content;
            TextureManager.Instance.LoadTexturesFromJson(content, "Data/SpriteSheetData.json");

            // Create Player Character:
            SpriteData spriteData = TextureManager.Instance.GetSpriteData("Reimu");
            PlayableCharacter reimu = new PlayableCharacter(spriteData);
            EntityManager.Instance.SetPlayerCharacter(reimu);

            _sceneManager.AddScene(new MainMenuScene(this.Content));
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Begin sprite batch with the updated scale matrix
            this._spriteBatch.Begin(transformMatrix: _scaleMatrix);
            this._sceneManager.Draw(this._spriteBatch);
            this._spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
