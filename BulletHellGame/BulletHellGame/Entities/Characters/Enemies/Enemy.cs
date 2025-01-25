using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;

namespace BulletHellGame.Entities.Characters.Enemies
{
    public class Enemy : Character
    {
        public Enemy(SpriteData spriteData) : base(spriteData)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Math.Abs(this.GetComponent<MovementComponent>().Velocity.X) > 0)
            {
                this.GetComponent<SpriteComponent>().SwitchAnimation("Moving");
            }
            else
            {
                this.GetComponent<SpriteComponent>().SwitchAnimation("Idle");
            }
        }
    }
}