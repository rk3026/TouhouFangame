using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Controllers
{
    public abstract class EntityController
    {
        public bool IsShooting { get; set; }
        public bool IsBombing { get; set; }
        public bool IsMoving { get; set; }
        public float Speed { get; set; }
        public float Direction { get; set; }

        public abstract void Update(EntityManager entityManager, Entity entity);
    }
}
