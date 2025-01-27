namespace BulletHellGame.Data.DataTransferObjects
{
    public class SpriteData
    {
        public string Name { get; set; } // Optional name for debugging or identification
        public Texture2D Texture { get; private set; } // Spritesheet texture
        public Dictionary<string, List<Rectangle>> Animations { get; private set; } = new(); // Animation name -> frames
        public Vector2 Origin { get; set; } = Vector2.Zero; // Default origin (for rotation and scaling)

        public SpriteData(Texture2D texture, Dictionary<string, List<Rectangle>> animations, string name = "")
        {
            Texture = texture ?? throw new ArgumentNullException(nameof(texture));
            Animations = animations ?? throw new ArgumentNullException(nameof(animations));
            Name = name;
        }

        // Get animation frames by name, throw exception if not found
        public List<Rectangle> GetAnimationFrames(string animationName)
        {
            if (Animations.TryGetValue(animationName, out var frames))
            {
                return frames;
            }

            throw new ArgumentException($"Animation '{animationName}' not found in SpriteData.");
        }

        // Check if an animation exists
        public bool HasAnimation(string animationName)
        {
            return Animations.ContainsKey(animationName);
        }
    }
}
