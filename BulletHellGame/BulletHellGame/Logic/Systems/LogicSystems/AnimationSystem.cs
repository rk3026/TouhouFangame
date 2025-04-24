using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Systems.LogicSystems
{
    public class AnimationSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (Entity entity in entityManager.GetEntitiesWithComponents(typeof(SpriteComponent)))
            {
                if (entity.TryGetComponent<SpriteComponent>(out var sc))
                {
                    // Rotate the sprite
                    sc.CurrentRotation += sc.RotationSpeed;

                    // Handle flashing logic
                    if (sc._isFlashing)
                    {
                        sc._flashDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                        if (sc._flashDuration <= 0)
                        {
                            sc.Color = Color.White;
                            sc._isFlashing = false;
                        }
                    }

                    if (sc._currentAnimation == null || !sc.SpriteData.Animations.ContainsKey(sc._currentAnimation))
                    {
                        // No animation to update
                        continue;
                    }

                    if (sc.SpriteData.Animations[sc._currentAnimation].Count > 1)
                    {
                        sc._isAnimating = true;
                    }

                    if (sc._isAnimating)
                    {
                        sc._timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;

                        if (sc._timeSinceLastFrame >= sc._frameDuration)
                        {
                            if (sc._isReversing)
                            {
                                sc._frameIndex--;
                                if (sc._frameIndex < 0)
                                {
                                    sc._frameIndex = 0;
                                    sc._isReversing = false;
                                }
                            }
                            else
                            {
                                sc._frameIndex++;
                                if (sc._frameIndex >= sc.SpriteData.Animations[sc._currentAnimation].Count)
                                {
                                    if (sc._loopAnimation)
                                    {
                                        sc._frameIndex = 0;
                                    }
                                    else
                                    {
                                        sc._frameIndex = sc.SpriteData.Animations[sc._currentAnimation].Count - 1;
                                        sc._isAnimating = false;
                                    }
                                }
                            }

                            sc._timeSinceLastFrame = 0;
                            sc._currentRect = sc.GetCurrentFrameRect();
                        }
                    }
                }
            }
        }
    }
}
