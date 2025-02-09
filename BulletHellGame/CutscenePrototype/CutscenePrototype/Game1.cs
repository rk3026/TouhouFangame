using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using cutsceneprototype.Content;
namespace cutsceneprototype
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private TextboxManager textbox;
        private SpriteFont font;
        private Texture2D textboxTexture;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the font and texture
            font = Content.Load<SpriteFont>("arial");
            textboxTexture = new Texture2D(GraphicsDevice, 1, 1);
            textboxTexture.SetData(new[] { Color.White });

            // Initialize the textbox
            Vector2 textboxPosition = new Vector2(0, GraphicsDevice.Viewport.Height - 150); // Bottom of the screen
            textbox = new TextboxManager(font, textboxTexture, textboxPosition);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update the textbox
            textbox.Update(gameTime);

            // Example: Show textbox when pressing a key
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                textbox.Show("This is a test message for the textbox!");
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            // Draw the textbox
            textbox.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}