using BulletHellGame.Entities.Characters.Enemies;
using BulletHellGame.Managers;

namespace BulletHellGame.Factories
{
    public class EnemyFactory
    {
        public EnemyFactory() { }

        public Enemy CreateEnemy(EnemyType type, Vector2 position, Vector2 velocity)
        {
            switch (type)
            {
                case EnemyType.Generic1:
                    return new GenericEnemy(TextureManager.Instance.GetTexture("Sprites/Enemies/GenericEnemy1Idle1"), position, velocity);
                case EnemyType.Generic2:
                    return new GenericEnemy(TextureManager.Instance.GetTexture("Sprites/Enemies/GenericEnemy1Idle1"), position, velocity);
                case EnemyType.Generic3:
                    return new GenericEnemy(TextureManager.Instance.GetTexture("Sprites/Enemies/GenericEnemy1Idle1"), position, velocity);
                case EnemyType.Generic4:
                    return new GenericEnemy(TextureManager.Instance.GetTexture("Sprites/Enemies/GenericEnemy1Idle1"), position, velocity);
                case EnemyType.Generic5:
                    return new GenericEnemy(TextureManager.Instance.GetTexture("Sprites/Enemies/GenericEnemy1Idle1"), position, velocity);
                default:
                    return new GenericEnemy(TextureManager.Instance.GetTexture("Sprites/Enemies/GenericEnemy1Idle1"), position, velocity);
            }
        }
    }
}
