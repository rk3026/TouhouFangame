using BulletHellGame.Systems;
using System.Linq;

namespace BulletHellGame.Managers
{
    public class SystemManager
    {
        private List<ISystem> _systems;

        public SystemManager()
        {
            _systems = new List<ISystem>();
        }

        public void AddSystem(ISystem system)
        {
            _systems.Add(system);
        }

        public void RemoveSystem(ISystem system)
        {
            _systems.Remove(system);
        }

        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (var system in _systems)
            {
                system.Update(entityManager, gameTime);
            }
        }

        public void Draw(EntityManager entityManager, SpriteBatch spriteBatch)
        {
            var drawingSystem = _systems.OfType<DrawingSystem>().FirstOrDefault();
            if (drawingSystem != null)
            {
                drawingSystem.Draw(entityManager, spriteBatch);
            }
        }
    }
}
