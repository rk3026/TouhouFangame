using BulletHellGame.Entities;

namespace BulletHellGame.Factories
{
    public abstract class EntityFactory
    {
        public EntityFactory() { }
        public abstract Entity CreateEntity();
    }
}
