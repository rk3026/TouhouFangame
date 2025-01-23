using BulletHellGame.Data;
using BulletHellGame.Entities.Characters.Enemies;
using BulletHellGame.Managers;

namespace BulletHellGame.Factories
{
    public class EnemyFactory
    {
        public EnemyFactory() { }

        public Enemy CreateEnemy(EnemyType type)
        {
            SpriteData spriteData = null;
            switch (type)
            {
                case EnemyType.Generic1:
                    spriteData = TextureManager.Instance.GetSpriteData("Enemy1");
                    return new Enemy(spriteData);
                case EnemyType.Generic2:
                    spriteData = TextureManager.Instance.GetSpriteData("Enemy1");
                    return new Enemy(spriteData);
                case EnemyType.Generic3:
                    spriteData = TextureManager.Instance.GetSpriteData("Enemy1");
                    return new Enemy(spriteData);
                case EnemyType.Generic4:
                    spriteData = TextureManager.Instance.GetSpriteData("Enemy1");
                    return new Enemy(spriteData);
                case EnemyType.Generic5:
                    spriteData = TextureManager.Instance.GetSpriteData("Enemy1");
                    return new Enemy(spriteData);
                default:
                    spriteData = TextureManager.Instance.GetSpriteData("Enemy1");
                    return new Enemy(spriteData);
            }
        }
    }
}
