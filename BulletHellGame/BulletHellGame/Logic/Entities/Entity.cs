using BulletHellGame.Logic.Components;
using System.Linq;

namespace BulletHellGame.Logic.Entities
{
    public class Entity
    {
        private Dictionary<Type, IComponent> _components = new();
        public event Action<IComponent> OnComponentAdded;
        public event Action<IComponent> OnComponentRemoved;

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
            foreach (var component in _components.Values)
            {
                component.Reset();
            }

            PositionComponent pc = GetComponent<PositionComponent>();
            VelocityComponent vc = GetComponent<VelocityComponent>();
            pc.Position = position;
            vc.Velocity = velocity;
        }

        public ISet<IComponent> GetComponents()
        {
            return _components.Values.ToHashSet();
        }

        public void AddComponent(IComponent component)
        {
            _components[component.GetType()] = component;
            OnComponentAdded?.Invoke(component);
        }

        // Probably not use this (removing components is costly, it has to loop through all the components)
        public void RemoveComponent<T>() where T : class, IComponent
        {
            if (_components.Remove(typeof(T), out var component))
                OnComponentRemoved?.Invoke(component);
        }


        public T GetComponent<T>() where T : class, IComponent
        {
            return (T)_components[typeof(T)];
        }

        public bool HasComponent<T>() where T : class, IComponent
        {
            return _components.ContainsKey(typeof(T));
        }

        public bool TryGetComponent<T>(out T component) where T : class, IComponent
        {
            IComponent test;
            bool worked = _components.TryGetValue(typeof(T), out test);
            component = (T)test;
            return worked;
        }
    }
}
