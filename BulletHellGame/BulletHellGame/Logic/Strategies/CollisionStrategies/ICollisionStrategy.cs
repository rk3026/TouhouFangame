using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Strategies.CollisionStrategies
{
    public interface ICollisionStrategy
    {
        public void ApplyCollision(EntityManager entityManager, Entity owner, Entity other);
    }
}
