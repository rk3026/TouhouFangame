using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Controllers
{
    public class EnemyController : EntityController
    {
        public override void Update(EntityManager entityManager, Entity entity)
        {
            this.IsShooting = true; // Constantly trying to shoot
        }
    }
}
