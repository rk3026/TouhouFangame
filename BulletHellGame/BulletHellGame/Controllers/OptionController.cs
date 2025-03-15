using BulletHellGame.Components;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Controllers
{
    public class OptionController : IController
    {
        public void Update(EntityManager entityManager, Entity entity)
        {
            // Grab components
            if (!(entity.TryGetComponent<ShootingComponent>(out var sc) &&
                entity.TryGetComponent<OwnerComponent>(out var oc)))
            {
                return;
            }
            sc.IsShooting = oc.Owner.GetComponent<ShootingComponent>().IsShooting; // Only able to shoot if its owner is
        }
    }
}
