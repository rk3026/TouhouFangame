using BulletHellGame.Entities;

namespace BulletHellGame.Components
{
    public class HomingComponent : IComponent
    {
        public float HomingRange { get; set; } = 200f; // Detection range for homing
        public float HomingStrength { get; set; } = 5f; // How fast the bullet adjusts its direction
        public float MaxHomingSpeed { get; set; } = 5f; // Maximum speed of the bullet
        public float HomingTimeLeft = 5f;               // How much time before being removed
        public Entity CurrentTarget { get; set; }
    }
}
