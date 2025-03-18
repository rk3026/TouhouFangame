using BulletHellGame.Components;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Controllers
{
    public class EnemyController : IController
    {
        public void Update(EntityManager entityManager, Entity entity)
        {
            // Grab components
            if (!(entity.TryGetComponent<ShootingComponent>(out var sc)))
            {
                return;
            }

            sc.IsShooting = true; // Constantly trying to shoot
            
        }
    }
}
