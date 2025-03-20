using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Controllers
{
    public class EnemyController : IController
    {
        public void Update(EntityManager entityManager, Entity entity)
        {
            // Grab components
            if (!entity.TryGetComponent<ShootingComponent>(out var sc))
            {
                return;
            }

            sc.IsShooting = true; // Constantly trying to shoot
            
        }
    }
}
