using BulletHellGame.Logic.Strategies.CollisionStrategies;

namespace BulletHellGame.Logic.Components
{
    public class CollisionStrategyComponent : IComponent
    {
        public ICollisionStrategy CollisionStrategy { get; set; }

        public CollisionStrategyComponent(ICollisionStrategy collisionStrategy)
        {
            CollisionStrategy = collisionStrategy;
        }
    }
}
