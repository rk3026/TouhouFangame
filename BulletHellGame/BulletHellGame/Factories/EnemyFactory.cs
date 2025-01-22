using BulletHellGame.Data;
using BulletHellGame.Entities.Characters.Enemies;
using BulletHellGame.Managers;

namespace BulletHellGame.Factories
{
    public class EnemyFactory
    {
        public EnemyFactory() { }

        public Enemy CreateEnemy(EnemyType type, Vector2 position)
        {
            SpriteData spriteData = null;
            switch (type)
            {
                case EnemyType.Generic1:
                    spriteData = TextureManager.Instance.GetSpriteInfo("Enemy1");
                    return new Enemy(spriteData, position);
                case EnemyType.Generic2:
                    spriteData = TextureManager.Instance.GetSpriteInfo("Enemy1");
                    return new Enemy(spriteData, position);
                case EnemyType.Generic3:
                    spriteData = TextureManager.Instance.GetSpriteInfo("Enemy1");
                    return new Enemy(spriteData, position);
                case EnemyType.Generic4:
                    spriteData = TextureManager.Instance.GetSpriteInfo("Enemy1");
                    return new Enemy(spriteData, position);
                case EnemyType.Generic5:
                    spriteData = TextureManager.Instance.GetSpriteInfo("Enemy1");
                    return new Enemy(spriteData, position);
                default:
                    spriteData = TextureManager.Instance.GetSpriteInfo("Enemy1");
                    return new Enemy(spriteData, position);
            }
        }
    }
}
