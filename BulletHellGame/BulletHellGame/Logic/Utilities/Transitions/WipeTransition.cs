public class WipeTransition() : Transition()
{
    public override void Draw(SpriteBatch spriteBatch)
    {
        int mid = (int)(oldScene.Width * percentage);
        Rectangle newR = new(mid, 0, newScene.Width, newScene.Height);

        spriteBatch.Draw(oldScene, Vector2.Zero, Color.White);
        spriteBatch.Draw(newScene, newR, newR, Color.White);
    }
}
