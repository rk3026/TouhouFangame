public class PushTransition() : Transition()
{
    public override void Draw(SpriteBatch spriteBatch)
    {
        int mid = (int)(oldScene.Width * percentage);
        Vector2 oldPos = new(mid - oldScene.Width, 0);
        Vector2 newPos = new(mid, 0);

        spriteBatch.Draw(oldScene, oldPos, Color.White);
        spriteBatch.Draw(newScene, newPos, Color.White);
    }
}
