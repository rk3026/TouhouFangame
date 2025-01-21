namespace BulletHellGame.Entities.Characters.Enemies
{
    public class GenericEnemy : Enemy
    {
        public GenericEnemy(Texture2D texture, Vector2 position, Vector2 velocity) : base(texture, position, velocity)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
