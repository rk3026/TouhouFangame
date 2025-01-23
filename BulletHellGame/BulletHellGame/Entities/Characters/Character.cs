using BulletHellGame.Components;
using BulletHellGame.Data;

namespace BulletHellGame.Entities.Characters
{
    public class Character : Entity
    {
        public Character(SpriteData spriteData, Vector2 position) : base(spriteData, position)
        {
            AddComponent(new HealthComponent(100));
            AddComponent(new SpriteEffectComponent());  // Make sure to add this component as well
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var spriteEffect = this.GetComponent<SpriteEffectComponent>();
            if (spriteEffect != null && spriteEffect.IsFlashing())
            {
                this.GetComponent<SpriteComponent>().Color = Color.Red;
            }
            else
            {
                this.GetComponent<SpriteComponent>().Color = Color.White;
            }
        }
    }
}
