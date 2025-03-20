using BulletHellGame.Logic.Managers;
using System.IO;

namespace BulletHellGame
{
    public class Game1 : Game
    {
        // Graphics:
        private GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;
        private Texture2D _whitePixel; // White pixel for fullscreen shader
        private RenderTarget2D _renderTarget; // Render target for applying shader to the screen
        private static Point DefaultWindowSize { get; set; } = new(640, 480); // 640 x 480 is Retro/Arcade dimensions (like for Touhou 07 - Perfect Cherry Blossom)

        // Independent scaling factors and matrix
        private float _scaleX;
        private float _scaleY;
        private Matrix _scaleMatrix;

        public Game1()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphicsDeviceManager.IsFullScreen = false;
            _graphicsDeviceManager.PreferredBackBufferWidth = DefaultWindowSize.X;
            _graphicsDeviceManager.PreferredBackBufferHeight = DefaultWindowSize.Y;
            _graphicsDeviceManager.ApplyChanges();

            // Allow the window to be resized
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += UpdateWindowSize;

            // Calculate initial scaling factors
            _scaleX = (float)_graphicsDeviceManager.PreferredBackBufferWidth / DefaultWindowSize.X;
            _scaleY = (float)_graphicsDeviceManager.PreferredBackBufferHeight / DefaultWindowSize.Y;
            _scaleMatrix = Matrix.CreateScale(_scaleX, _scaleY, 1f);

            _spriteBatch = new SpriteBatch(this.GraphicsDevice);

            // Create render target
            _renderTarget = new RenderTarget2D(GraphicsDevice, DefaultWindowSize.X, DefaultWindowSize.Y);

            // Create a 1x1 white pixel texture for fullscreen shader
            _whitePixel = new Texture2D(GraphicsDevice, 1, 1);
            _whitePixel.SetData(new[] { Color.White });

            base.Initialize();
        }

        private void UpdateWindowSize(object sender, EventArgs e)
        {
            // Recalculate the independent scaling factors and matrix
            _scaleX = (float)this.Window.ClientBounds.Width / DefaultWindowSize.X;
            _scaleY = (float)this.Window.ClientBounds.Height / DefaultWindowSize.Y;
            _scaleMatrix = Matrix.CreateScale(_scaleX, _scaleY, 1f);

            if (ShaderManager.Instance.ActiveShader != null)
            {
                ShaderManager.Instance.ActiveShader.Parameters["textureSize"].SetValue(new Vector2(DefaultWindowSize.X, DefaultWindowSize.Y));
                ShaderManager.Instance.ActiveShader.Parameters["videoSize"].SetValue(new Vector2(DefaultWindowSize.X, DefaultWindowSize.Y));
                ShaderManager.Instance.ActiveShader.Parameters["outputSize"].SetValue(new Vector2(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height));
            }
        }


        protected override void LoadContent()
        {
            LoadFonts();
            LoadTextures();
            LoadShaders();
            LoadSFX();

            // Adding the mainmenu scene to begin the game
            SceneManager.Instance.AddScene(new MainMenuScene(this.Content, this.GraphicsDevice));
        }

        protected override void Update(GameTime gameTime)
        {
            ShaderManager.Instance.Update(gameTime);
            SceneManager.Instance.Update(gameTime);
            InputManager.Instance.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Draw to the fixed-size render target (640x480)
            GraphicsDevice.SetRenderTarget(_renderTarget);
            _spriteBatch.Begin();
            SceneManager.Instance.Draw(_spriteBatch);
            _spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            // Draw the render target to the screen, scaling to fit the window
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, ShaderManager.Instance.ShaderEnabled ? ShaderManager.Instance.ActiveShader : null);
            _spriteBatch.Draw(_renderTarget, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }


        private void LoadFonts()
        {
            // Loading fonts
            FontManager.Instance.LoadFont(Content, "DFPPOPCorn-W12");
            FontManager.Instance.LoadFont(Content, "Arial");
        }

        private void LoadTextures()
        {
            // Loading all textures via spritesheets
            TextureManager.Instance.LoadSpriteSheetData(Content, "Data/SpriteSheets/Characters.json");
            TextureManager.Instance.LoadSpriteSheetData(Content, "Data/SpriteSheets/EnemiesAndBosses.json");
            TextureManager.Instance.LoadSpriteSheetData(Content, "Data/SpriteSheets/MenuAndOtherScreens.json");
            TextureManager.Instance.LoadSpriteSheetData(Content, "Data/SpriteSheets/ProjectilesAndObjects.json");
            TextureManager.Instance.LoadSpriteSheetData(Content, "Data/SpriteSheets/StagesTilesAndBackgrounds.json");
            TextureManager.Instance.LoadSpriteSheetData(Content, "Data/SpriteSheets/Fonts.json");
            TextureManager.Instance.LoadSpriteSheetData(Content, "Data/SpriteSheets/SidebarLoadAndPauseScreens.json");
        }

        private void LoadShaders()
        {
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
        }

        private void LoadSFX()
        {
            string soundDirectory = Path.Combine(Content.RootDirectory, "Sounds");
            if (Directory.Exists(soundDirectory))
            {
                var soundFiles = Directory.GetFiles(soundDirectory, "*.xnb", SearchOption.AllDirectories);
                foreach (var file in soundFiles)
                {
                    string soundName = Path.GetFileNameWithoutExtension(file);
                    SFXManager.Instance.LoadSound(Content, soundName, Path.Combine("Sounds", soundName));
                }
            }
        }


    }
}