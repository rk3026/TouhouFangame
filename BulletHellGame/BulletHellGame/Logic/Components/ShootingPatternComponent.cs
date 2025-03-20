namespace BulletHellGame.Logic.Components
{
    public class ShootingPatternComponent : IComponent
    {
        public List<Vector2> FiringDirections { get; set; } // Firing Directions
        public float TimeBetweenShots { get; set; } // Time between shots
        public float LastShotTime { get; set; } // Last shot time (used for cooldown)
        public int ShotsPerPattern { get; set; } // Number of shots to fire in the pattern
        public float ShotAngleVariation { get; set; } // Angle variation for spread patterns
        public float ShotSpeed { get; set; } // Speed of the shot bullets

        public ShootingPatternComponent()
        {
        }
    }
}
