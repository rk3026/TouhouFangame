using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.RenderingSystems
{
    public class SpriteRenderingSystem : IRenderingSystem
    {
        public int DrawPriority => 1;

        private readonly GraphicsDevice _graphicsDevice;
        private readonly Texture2D whitePixel;
        public SpriteRenderingSystem(GraphicsDevice gd) {
            _graphicsDevice = gd;
            whitePixel = new Texture2D(_graphicsDevice, 1, 1);
            whitePixel.SetData(new Color[] { Color.White });
        }
        public void Draw(EntityManager entityManager, SpriteBatch spriteBatch)
        {
            foreach (Entity entity in entityManager.GetEntitiesWithComponents(typeof(SpriteComponent)))
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

                    // Switch animations based on movement
                    if (vc.Velocity.LengthSquared() > 0)
                    {
                        sc.SwitchAnimation("MoveLeft", false);
                    }
                    else
                    {
                        sc.SwitchAnimation("Idle");
                    }

                    Vector2 spritePosition = new Vector2(pc.Position.X, pc.Position.Y);

                    spriteBatch.Draw(
                        spriteData.Texture,
                        spritePosition,
                        sc.CurrentFrame,
                        sc.Color,
                        sc.CurrentRotation,
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
