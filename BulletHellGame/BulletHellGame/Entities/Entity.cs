using BulletHellGame.Components;
using System.Collections.Generic;
using System.Linq;

namespace BulletHellGame.Entities
{
    public abstract class Entity : Sprite
    {
        private List<IComponent> _components = new List<IComponent>();

        public Entity(Texture2D texture, Vector2 position, List<Rectangle> frameRects = null, double frameDuration = 0.1, bool isAnimating = false) : base(texture, position, frameRects, frameDuration, isAnimating)
        {
            AddComponent(new SpriteEffectComponent());
            AddComponent(new HitboxComponent(this, new Rectangle(new Point((int)this.Position.X, (int)this.Position.Y), new Point(this.SourceRect.Value.Width, this.SourceRect.Value.Height))));
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
            base.Update(gameTime);
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
