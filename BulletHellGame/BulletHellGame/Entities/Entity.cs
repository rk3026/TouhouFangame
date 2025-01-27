using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using System.Linq;

namespace BulletHellGame.Entities
{
    public class Entity
    {
        private List<IComponent> _components = new();

        public bool IsActive { get; private set; } = false;

        public Entity(SpriteData spriteInfo)
        {
            if (spriteInfo == null)
            {
                throw new ArgumentNullException(nameof(spriteInfo), "SpriteData cannot be null.");
            }

            AddComponent(new SpriteComponent(spriteInfo));

            // Add other components (e.g., Hitbox, Movement)
            var sprite = GetComponent<SpriteComponent>();
            AddComponent(new HitboxComponent(this, sprite.CurrentFrame));
            AddComponent(new MovementComponent(this));
        }

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

            Position = position;
            GetComponent<MovementComponent>().Velocity = velocity;
        }

        public void AddComponent(IComponent component)
        {
            _components.Add(component);
        }

        public T GetComponent<T>() where T : class, IComponent
        {
            return _components.OfType<T>().FirstOrDefault();
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!IsActive) return; // Skip if not active

            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!IsActive) return; // Skip if not active

            var spriteComponent = GetComponent<SpriteComponent>();
            spriteComponent?.Draw(spriteBatch);
        }

        public Vector2 Position
        {
            get => GetComponent<SpriteComponent>()?.Position ?? Vector2.Zero;
            set
            {
                var spriteComponent = GetComponent<SpriteComponent>();
                if (spriteComponent != null)
                {
                    spriteComponent.Position = value;
                }
            }
        }

        public void SwitchAnimation(string animationName)
        {
            var spriteComponent = GetComponent<SpriteComponent>();
            spriteComponent?.SwitchAnimation(animationName);
        }
    }
}
