using BulletHellGame.Logic.Components;
using System.Linq;

namespace BulletHellGame.Logic.Entities
{
    public class Entity
    {
        private List<IComponent> _components = new();
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
            OnComponentAdded?.Invoke(component);
        }

        // Probably not use this (removing components is costly, it has to loop through all the components)
        public void RemoveComponent<T>() where T : class, IComponent
        {
            // Find the component of id T
            var component = _components.OfType<T>().FirstOrDefault();

            if (component != null)
            {
                // Remove the component from the list
                _components.Remove(component);

                // Invoke the event notifying that the component was removed
                OnComponentRemoved?.Invoke(component);
            }
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
