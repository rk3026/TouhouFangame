﻿namespace BulletHellGame.Logic.Components
{
    public class CollectorComponent : IComponent
    {
        public bool CanCollect { get; set; } = true;
        public CollectorComponent() { }
    }
}
