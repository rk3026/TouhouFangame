public abstract class Transition()
{
    protected RenderTarget2D oldScene;
    protected RenderTarget2D newScene;
    protected float duration;
    protected float durationLeft;
    protected float percentage;
    public void Start(RenderTarget2D oldS, RenderTarget2D newS, float length)
    {
        oldScene = oldS;
        newScene = newS;
        duration = length;
        durationLeft = duration;
    }

    public virtual bool Update(GameTime gameTime)
    {
        // Reduce the remaining duration by the elapsed time
        durationLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Calculate the percentage of completion, clamped between 0 and 1
        percentage = MathHelper.Clamp(1 - (durationLeft / duration), 0, 1);

        // Return true when the transition is complete
        return durationLeft <= 0f;
    }

    public abstract void Draw(SpriteBatch spriteBatch);
}
