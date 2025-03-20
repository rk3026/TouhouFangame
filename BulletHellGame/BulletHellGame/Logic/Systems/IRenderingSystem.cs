using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Systems
{
    public interface IRenderingSystem
    {
        public int DrawPriority { get; } // lower draw first
        public void Draw(EntityManager entityManager, SpriteBatch spriteBatch);
    }
}
