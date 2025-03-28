using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Controllers
{
    public class OptionController : EntityController
    {
        public override void Update(EntityManager entityManager, Entity entity)
        {
            // Grab components
            if (!(entity.TryGetComponent<ShootingComponent>(out var sc) &&
                entity.TryGetComponent<OwnerComponent>(out var oc)))
            {
                return;
            }
            this.IsShooting = oc.Owner.GetComponent<ControllerComponent>().Controller.IsShooting; // Only able to shoot if its owner is
        }
    }
}
