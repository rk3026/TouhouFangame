﻿using BulletHellGame.Components;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.LogicSystems
{
    public class MovementPatternSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (var entity in entityManager.GetEntitiesWithComponents(typeof(MovementPatternComponent)))
            {
                if (entity.TryGetComponent<VelocityComponent>(out var vc) &&
                    entity.TryGetComponent<MovementPatternComponent>(out var mpc) &&
                    entity.TryGetComponent<PositionComponent>(out var pc))
                {
                    // Update the movement pattern
                    mpc.TimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (mpc.CurrentStepIndex < mpc.PatternData.Count)
                    {
                        var currentStep = mpc.PatternData[mpc.CurrentStepIndex];
                        if (mpc.TimeElapsed >= currentStep.Time)
                        {
                            vc.Velocity = currentStep.Velocity;
                            mpc.TimeElapsed = 0f;
                            mpc.CurrentStepIndex++;
                        }
                    }

                    // Make enemy bounce off wall if not in bounds
                    if (!entityManager.Bounds.Contains(pc.Position))
                    {
                        if (pc.Position.Y < entityManager.Bounds.Top || pc.Position.Y > entityManager.Bounds.Bottom)
                        {
                            vc.Velocity = new Vector2(vc.Velocity.X, -vc.Velocity.Y);
                        }
                        if (pc.Position.X < entityManager.Bounds.Left || pc.Position.X > entityManager.Bounds.Right)
                        {
                            vc.Velocity = new Vector2(-vc.Velocity.X, vc.Velocity.Y);
                        }
                    }
                }
            }
        }
    }
}
