public class FadeTransition() : Transition()
{
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(oldScene, Vector2.Zero, Color.White * percentage);
        spriteBatch.Draw(newScene, Vector2.Zero, Color.White * (1 - percentage));
    }
}
