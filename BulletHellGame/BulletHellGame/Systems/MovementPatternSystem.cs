using BulletHellGame.Components;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems
{
    public class MovementPatternSystem : ISystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            // Iterate over all entities with a MovementPatternComponent
            foreach (var entity in entityManager.GetActiveEntities())
            {
                if (entity.HasComponent<MovementPatternComponent>())
                {
                    MovementPatternComponent mpc = entity.GetComponent<MovementPatternComponent>();

                    // Update the movement pattern
                    mpc.TimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (mpc.CurrentStepIndex < mpc.PatternData.Count)
                    {
                        var currentStep = mpc.PatternData[mpc.CurrentStepIndex];
                        if (mpc.TimeElapsed >= currentStep.Time)
                        {
                            entity.GetComponent<MovementComponent>().Velocity = currentStep.Velocity;
                            mpc.TimeElapsed = 0f;
                            mpc.CurrentStepIndex++;
                        }
                    }
                }
            }
        }
    }
}
