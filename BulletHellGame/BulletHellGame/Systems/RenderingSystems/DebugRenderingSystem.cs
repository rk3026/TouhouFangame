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
            foreach (Entity entity in entityManager.GetEntitiesWithComponents(typeof(SpriteComponent)))
            {
                if (entity.TryGetComponent<PositionComponent>(out var pc) &&
                    entity.TryGetComponent<HitboxComponent>(out var hbc))
                {
                    // Get the width and height of the hitbox
                    int width = (int)hbc.Hitbox.X;
                    int height = (int)hbc.Hitbox.Y;

                    // Calculate the position (centered on the entity's position)
                    Vector2 position = new Vector2(pc.Position.X - width / 2, pc.Position.Y - height / 2);

                    // Increase the thickness of the outline by changing the width and height of the lines
                    int outlineThickness = 2; // Set the thickness of the outline

                    // DrawActiveShader the outline (four lines for the edges of the hitbox)
                    spriteBatch.Draw(_pixel, new Rectangle((int)position.X, (int)position.Y, width, outlineThickness), Color.Cyan); // Top
                    spriteBatch.Draw(_pixel, new Rectangle((int)position.X, (int)position.Y + height - outlineThickness, width, outlineThickness), Color.LightPink); // Bottom
                    spriteBatch.Draw(_pixel, new Rectangle((int)position.X, (int)position.Y, outlineThickness, height), Color.BlueViolet); // Left
                    spriteBatch.Draw(_pixel, new Rectangle((int)position.X + width - outlineThickness, (int)position.Y, outlineThickness, height), Color.Salmon); // Right
                }

                // DrawActiveShader Health Bar
                if (entity.TryGetComponent<HealthComponent>(out var hc) &&
                    entity.TryGetComponent<SpriteComponent>(out var sc))
                {
                    // Health Bar Dimensions
                    int healthBarWidth = (int)sc.CurrentFrame.Width; // Match hitbox width
                    int healthBarHeight = 7; // Fixed height for visibility

                    // Position the health bar just below the entity
                    Vector2 healthBarPosition = new Vector2(pc.Position.X - healthBarWidth / 2,
                                                            pc.Position.Y + sc.CurrentFrame.Height / 2 + 5);

                    // Calculate health percentage
                    float healthPercentage = (float)hc.CurrentHealth / hc.MaxHealth;
                    int filledWidth = (int)(healthBarWidth * healthPercentage);

                    // Background (Empty health bar)
                    spriteBatch.Draw(_pixel, new Rectangle((int)healthBarPosition.X,
                                                           (int)healthBarPosition.Y,
                                                           healthBarWidth,
                                                           healthBarHeight), Color.Red);

                    // Filled (Current health)
                    spriteBatch.Draw(_pixel, new Rectangle((int)healthBarPosition.X,
                                                           (int)healthBarPosition.Y,
                                                           filledWidth,
                                                           healthBarHeight), Color.Green);
                }
            }
        }
    }
}
