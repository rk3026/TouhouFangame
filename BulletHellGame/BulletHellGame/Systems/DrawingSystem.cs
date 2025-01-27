using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems
{
    public class DrawingSystem : ISystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (Entity entity in entityManager.GetActiveEntities())
            {
                SpriteComponent sc = entity.GetComponent<SpriteComponent>();
                sc.Update(gameTime);
            }
        }

        public void Draw(EntityManager entityManager, SpriteBatch spriteBatch)
        {
            foreach (Entity entity in entityManager.GetActiveEntities())
            {
                SpriteComponent sc = entity.GetComponent<SpriteComponent>();
                MovementComponent mc = entity.GetComponent<MovementComponent>();
                SpriteData spriteData = sc.SpriteData;
                if (mc.Velocity.X > 0)
                {
                    sc.SpriteEffect = SpriteEffects.FlipHorizontally;
                }
                else if (mc.Velocity.X < 0)
                {
                    sc.SpriteEffect = SpriteEffects.None;
                }

                // Switch animations if player is moving (definitely want to refactor to separate animations vs sprites)
                if (entity.HasComponent<PlayerInputComponent>())
                {
                    var playerInputComponent = entity.GetComponent<PlayerInputComponent>();
                    if (playerInputComponent.IsMovingLeft || playerInputComponent.IsMovingRight)
                    {
                        entity.GetComponent<SpriteComponent>().SwitchAnimation("MoveLeft", false);
                    }
                    else
                    {
                        entity.GetComponent<SpriteComponent>().SwitchAnimation("Idle");
                    }
                }

                spriteBatch.Draw(
                    spriteData.Texture,
                    mc.Position,
                    sc.CurrentFrame,
                    sc.Color,
                    sc.Rotation,
                    spriteData.Origin,
                    sc.Scale,
                    sc.SpriteEffect,
                    0f
                );
            }
        }
    }
}
