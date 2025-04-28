using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Systems.LogicSystems
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
                    if (mpc.PatternData == null) continue;
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
                }
            }
        }
    }
}
