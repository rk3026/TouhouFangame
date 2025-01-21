using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BulletHellGame.Components
{
    public class SpriteEffectComponent : IComponent
    {
        private List<SpriteEffects> spriteEffects = new List<SpriteEffects>();
        private bool isFlashing;
        private float flashTime;
        private const float FlashDuration = 0.2f; // Duration for flashing red

        public SpriteEffectComponent()
        {
            isFlashing = false;
            flashTime = 0f;
        }

        // Method to trigger the flash effect
        public void FlashRed()
        {
            isFlashing = true; // Turn red immediately
            flashTime = FlashDuration; // Reset the flash duration
        }

        // Method to check if flashing is active
        public bool IsFlashing()
        {
            return isFlashing;
        }

        // Update the flash effect (time-based)
        public void Update(GameTime gameTime)
        {
            if (isFlashing)
            {
                flashTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (flashTime <= 0f)
                {
                    // Reset the flash effect once the duration is complete
                    isFlashing = false;
                }
            }
        }

        // Draw the entity with the flash effect applied if necessary
        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 position)
        {
            if (isFlashing)
            {
                // Apply a red tint to the texture while flashing
                spriteBatch.Draw(texture, position, Color.Red);
            }
            else
            {
                // Otherwise, draw the entity normally
                spriteBatch.Draw(texture, position, Color.White);
            }
        }

        public void Reset()
        {
            isFlashing = false;
        }
    }
}
