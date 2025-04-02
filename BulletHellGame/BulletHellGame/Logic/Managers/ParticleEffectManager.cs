using BulletHellGame.Logic.Utilities.ParticleEffects;

namespace BulletHellGame.Logic.Managers
{
    public class ParticleEffectManager
    {
        private static ParticleEffectManager _instance;
        public static ParticleEffectManager Instance => _instance ??= new ParticleEffectManager();

        private List<ParticleEffect> _particleEffects;

        private ParticleEffectManager()
        {
            _particleEffects = new List<ParticleEffect>();
        }

        public void SpawnEffect(Vector2 position, List<Texture2D> frames, float lifeTime, float frameDuration)
        {
            var effect = new ParticleEffect(position, frames, lifeTime, frameDuration);
            _particleEffects.Add(effect);
        }

        // Update all effects
        public void Update(GameTime gameTime)
        {
            for (int i = _particleEffects.Count - 1; i >= 0; i--)
            {
                var effect = _particleEffects[i];
                effect.Update(gameTime);

                if (effect.IsComplete)
                {
                    _particleEffects.RemoveAt(i); // Remove the effect if it's done
                }
            }
        }

        // Draw all effects
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var effect in _particleEffects)
            {
                effect.Draw(spriteBatch);
            }
        }

        public void SpawnDamageNumber(Vector2 position, int damage, bool isCritical = false)
        {
            var font = FontManager.Instance.GetFont("Arial");
            var color = isCritical ? Color.Red : Color.White;
            var lifetime = 1f;

            // Random offsets for position, rotation, and scale variation
            Random random = new Random();
            Vector2 randomOffset = new Vector2(random.Next(-10, 10), random.Next(-10, 10)); // Slight position variation
            float randomRotation = MathHelper.ToRadians(random.Next(-15, 15)); // Slight rotation variation
            float randomScale = 0.5f + (float)random.NextDouble() * 0.5f; // Scale between 0.5x and 1.0x

            var damageEffect = new DamageNumber(position + randomOffset, damage, font, color, lifetime, randomRotation, randomScale);
            _particleEffects.Add(damageEffect);
        }

    }
}
