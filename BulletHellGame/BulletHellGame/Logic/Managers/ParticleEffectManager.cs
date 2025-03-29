using BulletHellGame.Logic.Utilities;

namespace BulletHellGame.Logic.Managers
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;

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
        }
    }

}
