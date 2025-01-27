using BulletHellGame.Managers;

namespace BulletHellGame.Systems
{
    public interface ISystem
    {
        // Called every frame to update the system's logic
        void Update(EntityManager entityManager, GameTime gameTime);
    }
}
