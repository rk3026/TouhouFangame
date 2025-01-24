public static class Globals
{
    public static SpriteBatch SpriteBatch { get; set; }
    public static GraphicsDevice GraphicsDevice { get; set; }
    public static Point WindowSize { get; set; } = new(640, 480); // 640 x 480 is Retro/Arcade dimensions (like for Touhou 07 - Perfect Cherry Blossom)

    public static RenderTarget2D GetNewRenderTarget()
    {
        return new RenderTarget2D(GraphicsDevice, WindowSize.X, WindowSize.Y);
    }
}
