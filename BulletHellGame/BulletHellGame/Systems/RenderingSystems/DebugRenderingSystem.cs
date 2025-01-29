using BulletHellGame.Components;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.RenderingSystems
{
    public class DebugRenderingSystem : IRenderingSystem
    {
        public int DrawPriority => 2;

        private GraphicsDevice _graphicsDevice;
        private Texture2D _pixel;

        public DebugRenderingSystem(GraphicsDevice gd)
        {
            _graphicsDevice = gd;

            // Create a 1x1 pixel texture for drawing outlines
            _pixel = new Texture2D(_graphicsDevice, 1, 1);
            _pixel.SetData(new Color[] { Color.White });
        }

        public void Draw(EntityManager entityManager, SpriteBatch spriteBatch)
        {
            foreach (Entity entity in entityManager.GetActiveEntities())
            {
                if (entity.TryGetComponent<PositionComponent>(out var pc) &&
                    entity.TryGetComponent<HitboxComponent>(out var hc))
                {
                    // Get the width and height of the hitbox
                    int width = (int)hc.Hitbox.X;
                    int height = (int)hc.Hitbox.Y;

                    // Calculate the position (centered on the entity's position)
                    Vector2 position = new Vector2(pc.Position.X - hc.Hitbox.X / 2, pc.Position.Y - hc.Hitbox.Y / 2);

                    // Increase the thickness of the outline by changing the width and height of the lines
                    int outlineThickness = 2; // Set the thickness of the outline

                    // Draw the outline (four lines for the edges of the hitbox)
                    spriteBatch.Draw(_pixel, new Rectangle((int)position.X, (int)position.Y, width, outlineThickness), Color.Cyan); // Top
                    spriteBatch.Draw(_pixel, new Rectangle((int)position.X, (int)position.Y + height - outlineThickness, width, outlineThickness), Color.LightPink); // Bottom
                    spriteBatch.Draw(_pixel, new Rectangle((int)position.X, (int)position.Y, outlineThickness, height), Color.BlueViolet); // Left
                    spriteBatch.Draw(_pixel, new Rectangle((int)position.X + width - outlineThickness, (int)position.Y, outlineThickness, height), Color.Salmon); // Right
                }
            }
        }
    }
}
