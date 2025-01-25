using BulletHellGame.Data.DataTransferObjects;
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
                case EnemyType.FairyBlue:
                    spriteData = TextureManager.Instance.GetSpriteData("FairyBlue");
                    return new Enemy(spriteData);
                case EnemyType.FairyWhite:
                    spriteData = TextureManager.Instance.GetSpriteData("FairyBlue");
                    return new Enemy(spriteData);
                case EnemyType.FairyPink:
                    spriteData = TextureManager.Instance.GetSpriteData("FairyBlue");
                    return new Enemy(spriteData);
                case EnemyType.FairyRed:
                    spriteData = TextureManager.Instance.GetSpriteData("FairyBlue");
                    return new Enemy(spriteData);
                case EnemyType.FairyOrange:
                    spriteData = TextureManager.Instance.GetSpriteData("FairyBlue");
                    return new Enemy(spriteData);
                case EnemyType.FairyGreen:
                    spriteData = TextureManager.Instance.GetSpriteData("FairyBlue");
                    return new Enemy(spriteData);
                default:
                    spriteData = TextureManager.Instance.GetSpriteData("FairyBlue");
                    return new Enemy(spriteData);
            }
        }
    }
}
