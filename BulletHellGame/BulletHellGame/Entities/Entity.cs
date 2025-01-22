using BulletHellGame.Components;
using BulletHellGame.Data;
using System.Linq;

namespace BulletHellGame.Entities
{
    public class Entity
    {
        private List<IComponent> _components = new();

        public Entity(SpriteData spriteInfo, Vector2 position)
        {
            if (spriteInfo == null)
            {
                throw new ArgumentNullException(nameof(spriteInfo), "SpriteData cannot be null.");
            }

            // Add SpriteComponent with default frame duration and animation state
            AddComponent(new SpriteComponent(spriteInfo, position));

            // Add other components (e.g., Hitbox, Movement)
            var sprite = GetComponent<SpriteComponent>();
            AddComponent(new HitboxComponent(this, sprite.CurrentFrame));
            AddComponent(new MovementComponent(this));
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
            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            var spriteComponent = GetComponent<SpriteComponent>();
            spriteComponent?.Draw(spriteBatch);
        }

        public void Reset(Vector2 position, Vector2 velocity)
        {
            foreach (var component in _components)
            {
                component.Reset();
            }

            Position = position;
            GetComponent<MovementComponent>().Velocity = velocity;
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
