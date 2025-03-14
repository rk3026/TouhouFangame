using BulletHellGame.Components;
using BulletHellGame.Entities;
using BulletHellGame.Managers;
using BulletHellGame.Data.DataTransferObjects;
using System.Linq;

namespace BulletHellGame.Systems.RenderingSystems
{
    public class IndicatorSystem : IRenderingSystem
    {
        public int DrawPriority => 2;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly SpriteData _indicatorSprite;
        private readonly Rectangle _indicatorFrame;
        private int _frameCounter;
        private const int FlashInterval = 10; // Adjust for faster/slower flashing
        private const float MinOpacity = 0.5f; // Minimum opacity during flash

        public IndicatorSystem(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _indicatorSprite = TextureManager.Instance.GetSpriteData("EnemyIndicator");
            _indicatorFrame = _indicatorSprite.Animations.First().Value.First();
        }

        public void Draw(EntityManager entityManager, SpriteBatch spriteBatch)
        {
            int screenHeight = _graphicsDevice.Viewport.Height;
            Texture2D texture = _indicatorSprite.Texture;
            Vector2 origin = new Vector2(_indicatorFrame.Width / 2f, _indicatorFrame.Height / 2f);

            _frameCounter++;
            float opacity = MathHelper.Lerp(MinOpacity, 1f, (float)Math.Sin(_frameCounter * Math.PI / FlashInterval) * 0.5f + 0.5f);

            foreach (Entity entity in entityManager.GetEntitiesWithComponents(typeof(IndicatorComponent)))
            {
                if (entity.TryGetComponent<IndicatorComponent>(out var ic) &&
                    entity.TryGetComponent<PositionComponent>(out var pc))
                {
                    // Calculate the indicator's position, centered on X and at the bottom of the screen
                    Vector2 indicatorPosition = new Vector2(pc.Position.X, screenHeight - _indicatorFrame.Height / 2f);

                    // Draw the texture with opacity
                    spriteBatch.Draw(texture, indicatorPosition, _indicatorFrame, Color.White * opacity, 0f, origin, 1f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}