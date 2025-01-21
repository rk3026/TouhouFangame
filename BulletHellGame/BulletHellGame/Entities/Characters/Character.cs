using BulletHellGame.Components;

namespace BulletHellGame.Entities.Characters
{
    public class Character : Entity
    {
        public Character(Texture2D texture, Vector2 position, List<Rectangle> frameRects = null, double frameDuration = 0.1, bool isAnimating = false) : base(texture, position, frameRects, frameDuration, isAnimating)
        {
            AddComponent(new HealthComponent(100));
            AddComponent(new WeaponComponent(this));
            AddComponent(new SpriteEffectComponent());  // Make sure to add this component as well
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var spriteEffect = this.GetComponent<SpriteEffectComponent>();
            if (spriteEffect != null && spriteEffect.IsFlashing())
            {
                this.Color = Color.Red;
            }
            else
            {
                this.Color = Color.White;
            }
        }
    }
}
