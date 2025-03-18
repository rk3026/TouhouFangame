using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cutsceneprototype.Content
{
    public  class TextboxManager
    {
        private SpriteFont font;
        private Texture2D texture;
        private Vector2 position;
        private string text;
        private bool isVisible;

        public TextboxManager(SpriteFont font, Texture2D texture, Vector2 position)
        {
            this.font = font;
            this.texture = texture;
            this.position = position;
            this.text = "";
            this.isVisible = false;
        }

        public void Show(string text)
        {
            this.text = text;
            this.isVisible = true;
        }

        public void Hide()
        {
            this.isVisible = false;
        }

        public void Update(GameTime gameTime)
        {
            // Handle input to advance or hide the textbox
            if (isVisible && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Hide();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isVisible) return;

            // Draw the textbox background
            spriteBatch.Draw(texture, position, Color.White);

            // Draw the text
            Vector2 textPosition = position + new Vector2(20, 20); // Add padding
            spriteBatch.DrawString(font, text, textPosition, Color.White);
        }
    }


}

