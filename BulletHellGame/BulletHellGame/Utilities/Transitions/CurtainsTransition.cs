public class CurtainsTransition() : Transition()
{
    public override void Draw(SpriteBatch spriteBatch)
    {
        int amount = (int)(oldScene.Width * percentage / 2);
        Rectangle newR = new(amount, 0, oldScene.Width - 2 * amount, newScene.Height);

        spriteBatch.Draw(oldScene, Vector2.Zero, Color.White);
        spriteBatch.Draw(newScene, newR, newR, Color.White);
    }
}
