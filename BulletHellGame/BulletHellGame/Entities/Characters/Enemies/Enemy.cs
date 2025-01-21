using BulletHellGame.Components;

namespace BulletHellGame.Entities.Characters.Enemies
{
    public abstract class Enemy : Character
    {
        public Enemy(Texture2D texture, Vector2 position, Vector2 velocity) : base(texture, position, velocity)
        {
        }
    }
}