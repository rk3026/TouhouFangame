namespace BulletHellGame.Logic.Components
{
    public class DespawnComponent : IComponent
    {
        public bool DespawnWhenOffScreen { get; set; } = true;
        public Rectangle? CustomBoundary { get; set; } = null;

        public DespawnComponent(bool despawnWhenOffScreen = true, Rectangle? customBoundary = null)
        {
            DespawnWhenOffScreen = despawnWhenOffScreen;
            CustomBoundary = customBoundary;
        }
    }
}
