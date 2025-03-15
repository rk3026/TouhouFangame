using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Controllers
{
    public interface IController
    {
        public abstract void Update(EntityManager entityManager, Entity entity);
    }
}
