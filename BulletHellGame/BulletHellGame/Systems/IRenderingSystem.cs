using BulletHellGame.Managers;

namespace BulletHellGame.Systems
{
    public interface IRenderingSystem
    {
        public int DrawPriority { get; } // lower draw first
        public void Draw(EntityManager entityManager, SpriteBatch spriteBatch);
    }
}
