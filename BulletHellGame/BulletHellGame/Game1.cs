using BulletHellGame.Entities;
using BulletHellGame.Managers;
using BulletHellGame.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BulletHellGame
{
    public class Game1 : Game
    {
        // Managers:
        private SceneManager _sceneManager;
        private TextureManager _textureManager;

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
        }

        protected override void Initialize()
        {
            // Initialize globals and managers
            Globals.GraphicsDevice = this.GraphicsDevice;
            _sceneManager = SceneManager.Instance;
            _textureManager = TextureManager.Instance;

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

            base.Initialize();
        }

        private void UpdateWindowSize(object sender, EventArgs e)
        {
            // Update the global window size
            //Globals.WindowSize = new Point(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);

            // Update graphics device settings based on the new window size
            _graphics.PreferredBackBufferWidth = this.Window.ClientBounds.Width;
            _graphics.PreferredBackBufferHeight = this.Window.ClientBounds.Height;
            _graphics.ApplyChanges();

            // Recalculate the independent scaling factors and matrix
            _scaleX = (float)this.Window.ClientBounds.Width / Globals.WindowSize.X;  // Base width
            _scaleY = (float)this.Window.ClientBounds.Height / Globals.WindowSize.Y; // Base height
            _scaleMatrix = Matrix.CreateScale(_scaleX, _scaleY, 1f);

            //UpdateEntityPositions();
        }

        private void UpdateEntityPositions()
        {
            // Scale all entity positions based on the new independent scaling factors
            foreach (var entity in EntityManager.Instance.GetAllEntities())
            {
                entity.Position = new Vector2(entity.Position.X * _scaleX, entity.Position.Y * _scaleY);
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load content
            Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);
            ContentManager content = Content;
            string jsonPath = "Data/SpriteSheetData.json";

            TextureManager.Instance.LoadTexturesFromJson(content, jsonPath);

            // Example: Draw MainMenu from "MenuAndOtherScreens" sprite sheet
            string spriteSheet = "MenuAndOtherScreens";
            string spriteName = "MainMenu";

            Texture2D texture = TextureManager.Instance.GetTexture(spriteSheet);
            Rectangle sourceRectangle = TextureManager.Instance.GetSpriteRegion(spriteSheet, spriteName);

            // Create Player Character:
            Texture2D reimuTexture = Content.Load<Texture2D>("Sprites/ReimuIdle1");
            PlayableCharacter reimu = new PlayableCharacter(reimuTexture, new Vector2(Globals.WindowSize.X / 2, Globals.WindowSize.Y - reimuTexture.Height));
            EntityManager.Instance.AddEntity(reimu);

            TextureManager.Instance.LoadTexture(Content, "Sprites/ReimuBullet", "Sprites/ReimuBullet");
            TextureManager.Instance.LoadTexture(Content, "Sprites/ReimuPellet", "Sprites/ReimuPellet");
        }

        protected override void Update(GameTime gameTime)
        {
            // Exit game if input is detected
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Change the current scene based on input
            if (_sceneManager.Ready)
            {
                var newScene = _sceneManager.ActiveScene == SceneType.MainMenu ? SceneType.GameplayScene : SceneType.MainMenu;

                if (InputManager.KeyPressed(Keys.F1)) _sceneManager.SwitchScene(newScene, Transitions.Fade);
                else if (InputManager.KeyPressed(Keys.F2)) _sceneManager.SwitchScene(newScene, Transitions.Wipe);
                else if (InputManager.KeyPressed(Keys.F3)) _sceneManager.SwitchScene(newScene, Transitions.Push);
                else if (InputManager.KeyPressed(Keys.F4)) _sceneManager.SwitchScene(newScene, Transitions.Curtains);
                else if (InputManager.KeyPressed(Keys.F5)) _sceneManager.SwitchScene(newScene, Transitions.Rectangle);
                else if (InputManager.KeyPressed(Keys.F6)) _sceneManager.SwitchScene(newScene, Transitions.Checker);
            }
            _sceneManager.Update();

            // Update all entities
            EntityManager.Instance.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Begin sprite batch with the updated scale matrix
            Globals.SpriteBatch.Begin(transformMatrix: _scaleMatrix);

            // Draw all entities
            EntityManager.Instance.Draw(Globals.SpriteBatch);

            // End sprite batch
            Globals.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
