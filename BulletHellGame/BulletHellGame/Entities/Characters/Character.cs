using BulletHellGame.Components;
using BulletHellGame.Data;

namespace BulletHellGame.Entities.Characters
{
    public class Character : Entity
    {
        public Character(SpriteData spriteData) : base(spriteData)
        {
            AddComponent(new HealthComponent(100));
            AddComponent(new SpriteEffectComponent());
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
