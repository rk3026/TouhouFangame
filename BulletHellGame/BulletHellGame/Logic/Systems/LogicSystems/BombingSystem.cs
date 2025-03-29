using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;
using BulletHellGame.Logic.Managers.BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Systems.LogicSystems
{
    public class BombingSystem : ILogicSystem
    {
        private List<Texture2D> _bombEffectFrames;
        public BombingSystem()
        {
            // Create a simple 32x32 explosion texture with varying colors (you can adjust size and colors)
            List<Texture2D> effectFrames = new List<Texture2D>();

            for (int i = 0; i < 5; i++) // 5 frames for the explosion
            {
                int size = 32 + i * 8; // Gradually increase the size of each frame
                Texture2D frameTexture = CreateExplosionTexture(size, i * 50); // Generate a frame with size and intensity
                effectFrames.Add(frameTexture);
            }

            _bombEffectFrames = effectFrames;
        }

        // Helper method to generate a simple explosion texture
        private Texture2D CreateExplosionTexture(int size, int alpha)
        {
            Color[] data = new Color[size * size];

            // Basic radial gradient explosion
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    // Calculate distance from the center of the texture (explosion effect)
                    float distance = Vector2.Distance(new Vector2(x, y), new Vector2(size / 2, size / 2));
                    float normalizedDistance = distance / (size / 2); // Normalize distance

                    // Create a gradient effect: Red in the center, fading out to transparent
                    Color color = Color.OrangeRed * (1 - normalizedDistance); // Decrease intensity with distance
                    color.A = (byte)MathHelper.Clamp(255 - alpha, 0, 255); // Fade alpha based on the frame number (intensity)

                    data[y * size + x] = color;
                }
            }

            // Create the texture and set the data
            Texture2D texture = TextureManager.Instance.GetPixelTexture(Color.White, size, size);
            texture.SetData(data);
            return texture;
        }

        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Entity entity in entityManager.GetEntitiesWithComponents(typeof(BombingComponent)))
            {
                if (!entity.TryGetComponent(out BombingComponent bombing) ||
                    !entity.TryGetComponent(out ControllerComponent controller))
                {
                    continue;
                }

                // Update bomb cooldown timer
                bombing.TimeSinceLastBomb += deltaTime;

                if (!controller.Controller.IsBombing)
                    continue;

                if (bombing.BombCount <= 0 || bombing.TimeSinceLastBomb < bombing.BombCooldown)
                    continue;

                // Perform Bomb: Clear all damageable entities
                foreach (Entity target in entityManager.GetEntitiesWithComponents(typeof(DamageComponent)))
                {
                    if (target.TryGetComponent<HealthComponent>(out var hc))
                        hc.TakeDamage(bombing.BombDamage);
                    else
                        entityManager.QueueEntityForRemoval(target);
                }
                SFXManager.Instance.PlaySound("se_gun00");

                // Particle effect:
                Vector2 entityPosition = entity.GetComponent<PositionComponent>().Position;
                int effectWidth = _bombEffectFrames[0].Width;
                int effectHeight = _bombEffectFrames[0].Height;
                Vector2 effectPosition = new Vector2(
                    entityPosition.X - (effectWidth / 2),
                    entityPosition.Y - (effectHeight / 2)
                );
                ParticleEffectManager.Instance.SpawnEffect(effectPosition, _bombEffectFrames, 2f, 0.1f);

                bombing.BombCount--;
                bombing.TimeSinceLastBomb = 0f;
            }
        }
    }
}
