using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Systems
{
    public interface ILogicSystem
    {
        // Called every frame to update the system's logic
        void Update(EntityManager entityManager, GameTime gameTime);
    }
}
