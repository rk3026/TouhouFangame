using BulletHellGame.Components;
using System.Linq;

namespace BulletHellGame.Entities
{
    public class Entity
    {
        private List<IComponent> _components = new();

        public bool IsActive { get; private set; } = false;

        public Entity() { }

        public void Activate(Vector2 position, Vector2 velocity)
        {
            IsActive = true;
            Reset(position, velocity);
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        private void Reset(Vector2 position, Vector2 velocity)
        {
            foreach (var component in _components)
            {
                component.Reset();
            }

            PositionComponent pc = GetComponent<PositionComponent>();
            VelocityComponent vc = GetComponent<VelocityComponent>();
            pc.Position = position;
            vc.Velocity = velocity;
        }

        public void AddComponent(IComponent component)
        {
            _components.Add(component);
        }

        public T GetComponent<T>() where T : class, IComponent
        {
            return _components.OfType<T>().FirstOrDefault();
        }

        public bool HasComponent<T>() where T : class, IComponent
        {
            return _components.OfType<T>().Any();
        }

        public bool TryGetComponent<T>(out T component) where T : class, IComponent
        {
            component = _components.OfType<T>().FirstOrDefault();
            return component != null;
        }
    }
}
