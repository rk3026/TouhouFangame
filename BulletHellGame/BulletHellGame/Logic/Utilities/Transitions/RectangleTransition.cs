public class RectangleTransition() : Transition()
{
    public override void Draw(SpriteBatch spriteBatch)
    {
        Rectangle newR = new(newScene.Width / 2, newScene.Height / 2, 2, 2);
        newR.Inflate(newScene.Width * (1 - percentage) / 2, newScene.Height * (1 - percentage) / 2);

        spriteBatch.Draw(oldScene, Vector2.Zero, Color.White);
        spriteBatch.Draw(newScene, newR, newR, Color.White);
    }
}
