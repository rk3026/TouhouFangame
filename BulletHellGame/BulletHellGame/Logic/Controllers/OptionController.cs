using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Controllers
{
    public class OptionController : EntityController
    {
        public override void Update(EntityManager entityManager, Entity entity)
        {
            var ownerComponent = entity.GetComponent<OwnerComponent>();
            var weaponPosition = entity.GetComponent<PositionComponent>();

            if (ownerComponent.Owner.TryGetComponent<PositionComponent>(out var ownerPos))
            {
                weaponPosition.Position = ownerPos.Position + ownerComponent.Offset;
            }
            this.IsShooting = ownerComponent.Owner.GetComponent<ControllerComponent>().Controller.IsShooting;
        }
    }
}
