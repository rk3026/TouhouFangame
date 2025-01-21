using BulletHellGame.Components;

namespace BulletHellGame.Entities.Characters
{
    public class Character : Entity
    {
        public Character(Texture2D texture, Vector2 position, Vector2 velocity) : base(texture, position, velocity)
        {
            AddComponent(new HealthComponent(100));
            AddComponent(new WeaponComponent(this));
            AddComponent(new SpriteEffectComponent());  // Make sure to add this component as well
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Access the SpriteEffectComponent and check if it's flashing
            var spriteEffect = this.GetComponent<SpriteEffectComponent>();
            if (spriteEffect != null && spriteEffect.IsFlashing())
            {
                // Draw the sprite with a red tint when flashing
                spriteBatch.Draw(this.Texture, this.Position, Color.Red);
            }
            else
            {
                // Otherwise, call the base Draw method
                base.Draw(spriteBatch);
            }
        }
    }
}
