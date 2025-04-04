﻿using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Components
{
    public class MovementPatternComponent : IComponent
    {
        public int CurrentStepIndex = 0;
        public float TimeElapsed = 0f;
        public List<MovementData> PatternData;
        private string _patternName;

        public MovementPatternComponent(string patternName)
        {
            _patternName = patternName;
            PatternData = MovementPatternManager.Instance.GetPattern(_patternName);
        }

        public void Reset()
        {
            TimeElapsed = 0f;
            CurrentStepIndex = 0;
        }
    }
}
