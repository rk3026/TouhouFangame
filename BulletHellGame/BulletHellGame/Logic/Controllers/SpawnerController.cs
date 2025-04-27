using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Controllers
{
    public class SpawnerController : EntityController
    {
        public override void Update(EntityManager entityManager, Entity entity)
        {
            // Grab components
            if (!entity.TryGetComponent<OwnerComponent>(out var oc))
            {
                return;
            }
            this.IsShooting = oc.Owner.GetComponent<ControllerComponent>().Controller.IsShooting; // Only able to shoot if its owner is

            var ownerComponent = entity.GetComponent<OwnerComponent>();
            var positionComponent = entity.GetComponent<PositionComponent>();

            if (ownerComponent.Owner.TryGetComponent<PositionComponent>(out var ownerPos))
            {
                positionComponent.Position = ownerPos.Position + ownerComponent.Offset;
            }
        }
    }
}
