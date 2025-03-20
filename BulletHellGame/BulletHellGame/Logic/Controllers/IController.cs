using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Controllers
{
    public interface IController
    {
        public abstract void Update(EntityManager entityManager, Entity entity);
    }
}
