using BulletHellGame.Components;
using System.Linq;

namespace BulletHellGame.Entities
{
    public abstract class Entity : Sprite
    {
        private List<IComponent> _components = new List<IComponent>();

        // When specifying the initialVelocity:
        public Entity(Texture2D texture, Vector2 position, Vector2 initialVelocity) : base(texture, position)
        {
            AddComponent(new SpriteEffectComponent());
            AddComponent(new HitboxComponent(this, new Rectangle(new Point((int)this.Position.X, (int)this.Position.Y), new Point(this.Texture.Width, this.Texture.Height))));
            AddComponent(new MovementComponent(this));

            // Set initial initialVelocity
            GetComponent<MovementComponent>().Velocity = initialVelocity;
        }

        public void AddComponent(IComponent component)
        {
            _components.Add(component);
        }

        public T GetComponent<T>() where T : class, IComponent
        {
            return _components.OfType<T>().FirstOrDefault();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
        }

        public void Reset(Vector2 position, Vector2 velocity)
        {
            foreach (IComponent component in _components)
            {
                component.Reset();
            }

            this.Position = position;
            this.GetComponent<MovementComponent>().Velocity = velocity;
        }
    }
}
