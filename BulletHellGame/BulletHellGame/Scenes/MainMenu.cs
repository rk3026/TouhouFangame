using BulletHellGame.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BulletHellGame.Scenes
{
    public class MainMenu : Scene
    {
        /*
        // References to textures
        private Texture2D _backgroundTexture;
        private Texture2D _logoTexture;

        // Button definitions (for simplicity, use rectangles)
        private Rectangle _startButtonRect;
        private Rectangle _exitButtonRect;

        // Mouse state for detecting clicks
        private MouseState _previousMouseState;

        public MainMenu() { }

        // Load all necessary textures and assets
        protected override void Load()
        {
            // Load textures from the TextureManager
            _backgroundTexture = TextureManager.Instance.GetTexture("MainMenuBackground");
            _logoTexture = TextureManager.Instance.GetTexture("GameLogo");

            // Define button positions and sizes
            _startButtonRect = new Rectangle(300, 300, 200, 50); // Example Start button
            _exitButtonRect = new Rectangle(300, 400, 200, 50);  // Example Exit button
        }

        // Handle logic that occurs when the scene becomes active
        public override void Activate()
        {
            // Reset mouse state or other scene-specific settings if necessary
            _previousMouseState = Mouse.GetState();
        }

        // Update logic for input handling
        public override void Update()
        {
            var mouseState = Mouse.GetState();

            // Check for button clicks
            if (mouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
            {
                // Start button clicked
                if (_startButtonRect.Contains(mouseState.Position))
                {
                    // Transition to the game scene
                    SceneManager.Instance.ChangeScene(new GameplayScene());
                }

                // Exit button clicked
                if (_exitButtonRect.Contains(mouseState.Position))
                {
                    // Exit the game
                    GameManager.Instance.Exit();
                }
            }

            _previousMouseState = mouseState;
        }

        // Draw the scene
        protected override void Draw()
        {
            var spriteBatch = Globals.SpriteBatch;

            spriteBatch.Begin();

            // Draw background
            spriteBatch.Draw(_backgroundTexture, Vector2.Zero, Color.White);

            // Draw logo
            spriteBatch.Draw(_logoTexture, new Vector2(300, 100), Color.White);

            // Draw buttons (placeholder rectangles for simplicity)
            spriteBatch.Draw(TextureManager.Instance.GetTexture("ButtonTexture"), _startButtonRect, Color.White);
            spriteBatch.Draw(TextureManager.Instance.GetTexture("ButtonTexture"), _exitButtonRect, Color.White);

            spriteBatch.End();
        }
        */
        public override void Activate()
        {
        }

        public override void Update()
        {
        }

        protected override void Draw()
        {
        }

        protected override void Load()
        {
        }
    }
}
