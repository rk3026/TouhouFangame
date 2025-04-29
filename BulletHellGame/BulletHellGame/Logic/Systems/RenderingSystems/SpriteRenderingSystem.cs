using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Systems.RenderingSystems
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

                    // Switch animations based on movement
                    if (Math.Abs(vc.Velocity.X) > 1f)
                    {
                        sc.SwitchAnimation("MoveLeft", false);

                        if (vc.Velocity.X > 0)
                            sc.SpriteEffect = SpriteEffects.FlipHorizontally;
                        else if (vc.Velocity.X < 0)
                            sc.SpriteEffect = SpriteEffects.None;
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
