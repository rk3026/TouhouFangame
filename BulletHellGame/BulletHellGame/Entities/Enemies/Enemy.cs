using BulletHellGame.Components;

namespace BulletHellGame.Entities.Enemies
{
    public abstract class Enemy : Entity
    {
        public Enemy(Texture2D texture, Vector2 position) : base(texture, position)
        {
            AddComponent(new HealthComponent(100)); // Attach health
            AddComponent(new MovementComponent(this, new Vector2(0,0)));
        }
    }
}