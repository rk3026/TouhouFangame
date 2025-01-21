using BulletHellGame.Entities;
using BulletHellGame.Entities.Characters.Enemies;
using BulletHellGame.Factories;
using BulletHellGame.Managers;
using BulletHellGame.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

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
            /*
            string spriteSheet = "MenuAndOtherScreens";
            string spriteName = "MainMenu";
            Texture2D texture = TextureManager.Instance.GetTexture(spriteSheet);
            Rectangle sourceRectangle = TextureManager.Instance.GetSpriteRegion(spriteSheet, spriteName);
            */

            // Create Player Character:
            Texture2D reimuTexture = Content.Load<Texture2D>("Sprites/Characters/Reimu/ReimuIdle1");
            PlayableCharacter reimu = new PlayableCharacter(reimuTexture, new Vector2(Globals.WindowSize.X / 2, Globals.WindowSize.Y - reimuTexture.Height), new Vector2(0,0));
            EntityManager.Instance.SetPlayerCharacter(reimu);

            // Load Bullet Textures:
            TextureManager.Instance.LoadTexture(Content, "Sprites/Bullets/ReimuBullet", "Sprites/Bullets/ReimuBullet");
            TextureManager.Instance.LoadTexture(Content, "Sprites/Bullets/ReimuPellet", "Sprites/Bullets/ReimuPellet");

            // Load Enemy Texture:
            TextureManager.Instance.LoadTexture(Content, "Sprites/Enemies/GenericEnemy1Idle1", "Sprites/Enemies/GenericEnemy1Idle1");
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

            // Spawn a batch of enemies on F key press:
            if (InputManager.KeyPressed(Keys.F))
            {
                // Check how many enemies are currently in the game
                int enemyCount = EntityManager.Instance.GetActiveEntities().OfType<Enemy>().Count();

                // Only spawn a new batch if there are no enemies currently in the game
                if (enemyCount == 0)
                {
                    // Define the grid size (5x5)
                    int gridSize = 5;

                    // Offset from the center to the left or right to create a 5x5 grid
                    float offset = 50;  // Adjust this to control the distance between enemies

                    // Calculate the base spawn position at the top middle of the screen
                    float baseSpawnX = Globals.WindowSize.X / 2;  // Start from the middle of the screen
                    float spawnY = 0;  // At the top of the screen

                    // Spawn the enemies in a 5x5 grid, centered on the screen
                    for (int row = 0; row < gridSize; row++)
                    {
                        for (int col = 0; col < gridSize; col++)
                        {
                            // Calculate the spawn position for each enemy
                            float spawnX = baseSpawnX + (col - 2) * offset;  // Offset left/right based on the column
                            float spawnOffsetY = (row + 2) * offset;  // Offset up/down based on the row

                            // Create the enemy at the calculated position
                            EntityManager.Instance.CreateEnemy(EnemyType.Generic1, new Vector2(spawnX, spawnY + spawnOffsetY));
                        }
                    }
                }
            }

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
