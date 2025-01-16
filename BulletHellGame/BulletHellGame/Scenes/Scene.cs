public abstract class Scene
{
    protected readonly RenderTarget2D target;

    public Scene()
    {
        target = Globals.GetNewRenderTarget();
        Load();
    }

    protected abstract void Load();
    protected abstract void Draw();
    public abstract void Update();
    public abstract void Activate();

    public virtual RenderTarget2D GetFrame()
    {
        Globals.GraphicsDevice.SetRenderTarget(target);
        Globals.GraphicsDevice.Clear(Color.Black);

        Globals.SpriteBatch.Begin();
        Draw();
        Globals.SpriteBatch.End();

        Globals.GraphicsDevice.SetRenderTarget(null);
        return target;
    }
}
