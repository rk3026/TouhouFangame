using BulletHellGame.Components;
using System.Linq;

namespace BulletHellGame.Entities
{
    public class Entity
    {
        private List<IComponent> _components = new();
        public event Action<Entity> OnComponentsChanged; // Event for component changes

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

        public List<IComponent> GetComponents()
        {
            return _components;
        }

        public void AddComponent(IComponent component)
        {
            _components.Add(component);
            OnComponentsChanged?.Invoke(this); // Notify that components changed
        }

        // Probably not use this (removing components is costly, it has to loop through all the components)
        public void RemoveComponent<T>() where T : class, IComponent
        {
            _components.RemoveAll(c => c is T);
            OnComponentsChanged?.Invoke(this); // Notify that components changed
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
