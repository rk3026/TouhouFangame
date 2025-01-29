using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.RenderingSystems
{
    public class SpriteRenderingSystem : IRenderingSystem
    {
        public int DrawPriority => 1;

        private GraphicsDevice _graphicsDevice;
        public SpriteRenderingSystem(GraphicsDevice gd) {
            _graphicsDevice = gd;
        }
        public void Draw(EntityManager entityManager, SpriteBatch spriteBatch)
        {
            foreach (Entity entity in entityManager.GetActiveEntities())
            {
                if (entity.TryGetComponent<SpriteComponent>(out var sc) &&
                    entity.TryGetComponent<PositionComponent>(out var pc) &&
                    entity.TryGetComponent<VelocityComponent>(out var vc))
                {
                    SpriteData spriteData = sc.SpriteData;
                    if (vc.Velocity.X > 0)
                    {
                        sc.SpriteEffect = SpriteEffects.FlipHorizontally;
                    }
                    else if (vc.Velocity.X < 0)
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

                    Vector2 spritePosition = new Vector2(pc.Position.X - sc.CurrentFrame.Width / 2, pc.Position.Y - sc.CurrentFrame.Height / 2);

                    spriteBatch.Draw(
                        spriteData.Texture,
                        spritePosition,
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
}
