using BulletHellGame.Entities.Enemies;
using BulletHellGame.Managers;

namespace BulletHellGame.Factories
{
    public class EnemyFactory
    {
        public EnemyFactory() { }

        public Enemy CreateEnemy(EnemyType type, Vector2 position)
        {
            switch (type)
            {
                case EnemyType.Grunt:
                    return new Grunt(TextureManager.Instance.GetTexture("Grunt"), position);
                default:
                    return new Grunt(TextureManager.Instance.GetTexture("Grunt"), position);
            }
        }
    }
}
