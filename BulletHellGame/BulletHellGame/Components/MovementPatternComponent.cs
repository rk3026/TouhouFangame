using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Components
{
    public class MovementPatternComponent : IComponent
    {
        private Entity _owner;
        private string _patternName;
        private List<MovementData> _patternData;
        private int _currentStepIndex = 0;
        private float _timeElapsed = 0f;

        public MovementPatternComponent(Entity owner, string patternName)
        {
            _owner = owner;
            _patternName = patternName;

            // Retrieve the movement pattern from the manager
            _patternData = MovementPatternManager.Instance.GetPattern(patternName);
        }

        public void Update(GameTime gameTime)
        {
            // Convert ElapsedGameTime to seconds and accumulate
            _timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_currentStepIndex < _patternData.Count)
            {
                var currentStep = _patternData[_currentStepIndex];

                // Check if enough time has passed to move to the next step
                if (_timeElapsed >= currentStep.Time)
                {
                    // Apply the velocity for this step to the owner's movement component
                    _owner.GetComponent<MovementComponent>().Velocity = currentStep.Velocity;

                    // Move to the next step
                    _timeElapsed = 0f;
                    _currentStepIndex++;
                }
            }
        }
    }
}
