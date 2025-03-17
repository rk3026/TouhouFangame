using BulletHellGame.Managers;
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
        private ShaderManager _shaderManager;

        // Graphics:
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Effect _shader;
        private Texture2D _whitePixel; // White pixel for fullscreen shader
        private RenderTarget2D _renderTarget; // Render target for applying shader

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
            _shaderManager = ShaderManager.Instance;
        }

        protected override void Initialize()
        {
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = Globals.WindowSize.X;
            _graphics.PreferredBackBufferHeight = Globals.WindowSize.Y;
            _graphics.ApplyChanges();

            // Allow the window to be resized
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += UpdateWindowSize;

            // Calculate initial scaling factors
            _scaleX = (float)_graphics.PreferredBackBufferWidth / Globals.WindowSize.X;
            _scaleY = (float)_graphics.PreferredBackBufferHeight / Globals.WindowSize.Y;
            _scaleMatrix = Matrix.CreateScale(_scaleX, _scaleY, 1f);

            _spriteBatch = new SpriteBatch(this.GraphicsDevice);
            Globals.GraphicsDevice = this.GraphicsDevice;
            Globals.SpriteBatch = this._spriteBatch;

            // Create render target
            _renderTarget = new RenderTarget2D(GraphicsDevice, Globals.WindowSize.X, Globals.WindowSize.Y);

            base.Initialize();
        }

        private void UpdateWindowSize(object sender, EventArgs e)
        {
            _graphics.PreferredBackBufferWidth = this.Window.ClientBounds.Width;
            _graphics.PreferredBackBufferHeight = this.Window.ClientBounds.Height;
            _graphics.ApplyChanges();

            _scaleX = (float)this.Window.ClientBounds.Width / Globals.WindowSize.X;
            _scaleY = (float)this.Window.ClientBounds.Height / Globals.WindowSize.Y;
            _scaleMatrix = Matrix.CreateScale(_scaleX, _scaleY, 1f);

            // Recreate the render target when the window size changes
            _renderTarget.Dispose();
            _renderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            if (_shaderManager.ActiveShader != null)
            {
                _shaderManager.ActiveShader.Parameters["textureSize"].SetValue(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
                _shaderManager.ActiveShader.Parameters["videoSize"].SetValue(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
                _shaderManager.ActiveShader.Parameters["outputSize"].SetValue(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ContentManager content = Content;

            // Load fonts and textures
            FontManager.Instance.LoadFont(content, "DFPPOPCorn-W12");
            FontManager.Instance.LoadFont(content, "Arial");

            // Loading all textures via spritesheets
            TextureManager.Instance.LoadSpriteSheetData(content, "Data/SpriteSheets/Characters.json");
            TextureManager.Instance.LoadSpriteSheetData(content, "Data/SpriteSheets/EnemiesAndBosses.json");
            TextureManager.Instance.LoadSpriteSheetData(content, "Data/SpriteSheets/MenuAndOtherScreens.json");
            TextureManager.Instance.LoadSpriteSheetData(content, "Data/SpriteSheets/ProjectilesAndObjects.json");
            TextureManager.Instance.LoadSpriteSheetData(content, "Data/SpriteSheets/StagesTilesAndBackgrounds.json");
            TextureManager.Instance.LoadSpriteSheetData(content, "Data/SpriteSheets/Fonts.json");
            TextureManager.Instance.LoadSpriteSheetData(content, "Data/SpriteSheets/SidebarLoadAndPauseScreens.json");

            // Create a 1x1 white pixel texture for fullscreen shader
            _whitePixel = new Texture2D(GraphicsDevice, 1, 1);
            _whitePixel.SetData(new[] { Color.White });

            // Loading shaders
            var _shader = Content.Load<Effect>("Shaders/CRT_Shader");
            _shader.Parameters["textureSize"].SetValue(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)); // Size of the rendered texture (screen or render target)
            _shader.Parameters["videoSize"].SetValue(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));   // Original video size (for scaling effects)
            _shader.Parameters["outputSize"].SetValue(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));  // Output size after processing
            _shader.Parameters["hardScan"].SetValue(-4.0f);  // Increase = sharper scanlines, more defined. Decrease = softer scanlines, blurrier.
            _shader.Parameters["hardPix"].SetValue(-1.0f);   // Increase = sharper pixels, more crisp. Decrease = softer pixels, more blur.
            _shader.Parameters["warpX"].SetValue(0.002f);    // Increase = more horizontal screen warping (curvature). Decrease = flatter screen.
            _shader.Parameters["warpY"].SetValue(0.0025f);   // Increase = more vertical screen warping (curvature). Decrease = flatter screen.
            _shader.Parameters["maskDark"].SetValue(0.6f);   // Increase = darker shadow mask, more contrast. Decrease = lighter shadow mask, less contrast.
            _shader.Parameters["maskLight"].SetValue(1.4f);  // Increase = brighter highlights. Decrease = dimmer highlights.
            _shader.Parameters["brightboost"].SetValue(0.8f); // Increase = boosts overall brightness. Decrease = darkens the screen.
            _shader.Parameters["shadowMask"].SetValue(1.0f); // Increase = more visible shadow mask pattern. Decrease = less visible pattern.
            _shader.Parameters["bloomAmount"].SetValue(0.1f); // Increase = more glow/bloom effect, softer visuals. Decrease = less glow, sharper visuals.
            _shader.Parameters["shape"].SetValue(0.9f);      // Increase = more screen curvature. Decrease = flatter screen.
            ShaderManager.Instance.StoreShader("CRT", _shader);

            _shader = Content.Load<Effect>("Shaders/Glitch_Shader");
            _shader.Parameters["textureSize"].SetValue(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)); // Size of the rendered texture (screen or render target)
            ShaderManager.Instance.StoreShader("Glitch", _shader);

            _shader = Content.Load<Effect>("Shaders/MagicAura_Shader");
            ShaderManager.Instance.StoreShader("MagicAura", _shader);

            // Adding the mainmenu scene to begin the game
            _sceneManager.AddScene(new MainMenuScene(this.Content, this.GraphicsDevice));
        }

        protected override void Update(GameTime gameTime)
        {
            _shaderManager.Update(gameTime);
            _sceneManager.Update(gameTime);
            _inputManager.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.SetRenderTarget(_renderTarget);

            _spriteBatch.Begin(transformMatrix: _scaleMatrix);
            _sceneManager.Draw(_spriteBatch);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            if (ShaderManager.Instance.ShaderEnabled)
            {
                var shader = _shaderManager.ActiveShader;
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, shader);
            }
            else
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            }
            _spriteBatch.Draw(_renderTarget, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}