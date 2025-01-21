namespace BulletHellGame.Entities.Characters.Enemies
{
    public class Enemy : Character
    {
        public Enemy(Texture2D texture, Vector2 position, List<Rectangle> frameRects = null, double frameDuration = 0.1, bool isAnimating = false) : base(texture, position, frameRects, frameDuration, isAnimating)
        {
        }
    }
}