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
            SpriteInfo si = null;
            switch (type)
            {
                case EnemyType.Generic1:
                    si = TextureManager.Instance.GetSpriteInfo("Enemy1.Idle");
                    return new Enemy(si.Texture, position, si.Rects, 0.1, true);
                case EnemyType.Generic2:
                    si = TextureManager.Instance.GetSpriteInfo("Enemy1.Idle");
                    return new Enemy(si.Texture, position, si.Rects, 0.1, true);
                case EnemyType.Generic3:
                    si = TextureManager.Instance.GetSpriteInfo("Enemy1.Idle");
                    return new Enemy(si.Texture, position, si.Rects, 0.1, true);
                case EnemyType.Generic4:
                    si = TextureManager.Instance.GetSpriteInfo("Enemy1.Idle");
                    return new Enemy(si.Texture, position, si.Rects, 0.1, true);
                case EnemyType.Generic5:
                    si = TextureManager.Instance.GetSpriteInfo("Enemy1.Idle");
                    return new Enemy(si.Texture, position, si.Rects, 0.1, true);
                default:
                    si = TextureManager.Instance.GetSpriteInfo("Enemy1.Idle");
                    return new Enemy(si.Texture, position, si.Rects, 0.1, true);
            }
        }
    }
}
