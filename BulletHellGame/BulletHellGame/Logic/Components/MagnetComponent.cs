namespace BulletHellGame.Logic.Components
{
    public class MagnetComponent : IComponent
    {
        public float MagnetRange { get; set; }  // Attraction range
        public float MagnetStrength { get; set; } // How fast objects are drawn in
        public float SnapRange { get; set; } // Range where objects snap to position

        public MagnetComponent(float range = 100f, float strength = 3f, float snapRange = 30f)
        {
            MagnetRange = range;
            MagnetStrength = strength;
            SnapRange = snapRange;
        }
    }
}
