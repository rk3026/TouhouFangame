using BulletHellGame.Components;
using System.Linq;

namespace BulletHellGame.Entities
{
    public abstract class Entity : Sprite
    {
        private List<IComponent> _components = new List<IComponent>();

        public Entity(Texture2D texture, Vector2 position) : base(texture, position) { }

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
    }

}
