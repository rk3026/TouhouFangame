using Newtonsoft.Json.Linq;
using Microsoft.Xna.Framework.Content;
using System.IO;
using BulletHellGame.DataAccess.DataTransferObjects;

namespace BulletHellGame.Logic.Managers
{
    public class TextureManager
    {
        private GraphicsDevice _graphicsDevice;

        private static TextureManager _instance;
        public static TextureManager Instance => _instance ??= new TextureManager();

        // Dictionary to store sprite information by name
        private readonly Dictionary<string, SpriteData> _sprites = new();

        private TextureManager() { }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            this._graphicsDevice = graphicsDevice;

            // Set up a default sprite:
            Texture2D texture = GetPixelTexture(Color.Blue, 10, 10);
            SpriteData sd = new SpriteData(texture, new Dictionary<string, List<Rectangle>>(), "Default");
            _sprites.Add("Default", sd);
        }

        /// <summary>
        /// Loads all the textures on a sprite sheet given the data json file for that sheet.
        /// </summary>
        public SpriteData Create3x3SpriteSheet(Texture2D texture, string spriteKey)
        {
            int frameWidth = texture.Width / 3;
            int frameHeight = texture.Height / 3;

            // Build the animations dictionary
            Dictionary<string, List<Rectangle>> animations = new Dictionary<string, List<Rectangle>>
            {
                ["Pose1"] = new List<Rectangle> { new Rectangle(0, 0, frameWidth, frameHeight) },
                ["Pose2"] = new List<Rectangle> { new Rectangle(frameWidth, 0, frameWidth, frameHeight) },
                ["Pose3"] = new List<Rectangle> { new Rectangle(frameWidth * 2, 0, frameWidth, frameHeight) },

                ["Pose4"] = new List<Rectangle> { new Rectangle(0, frameHeight, frameWidth, frameHeight) },
                ["Pose5"] = new List<Rectangle> { new Rectangle(frameWidth, frameHeight, frameWidth, frameHeight) },
                ["Pose6"] = new List<Rectangle> { new Rectangle(frameWidth * 2, frameHeight, frameWidth, frameHeight) },

                ["Pose7"] = new List<Rectangle> { new Rectangle(0, frameHeight * 2, frameWidth, frameHeight) },
                ["Pose8"] = new List<Rectangle> { new Rectangle(frameWidth, frameHeight * 2, frameWidth, frameHeight) },
                ["Pose9"] = new List<Rectangle> { new Rectangle(frameWidth * 2, frameHeight * 2, frameWidth, frameHeight) }
            };

            // Create and store a SpriteData
            SpriteData spriteData = new SpriteData(texture, animations, spriteKey);
            _sprites[spriteKey] = spriteData;
            return spriteData;
        }
        public void LoadSpriteSheetData(ContentManager content, string jsonFilePath)
        {
            // Read and parse the JSON file
            string jsonString = File.ReadAllText(jsonFilePath);
            JObject json = JObject.Parse(jsonString);

            // Use the file name (without extension) as the sprite sheet name
            string spriteSheetName = Path.GetFileNameWithoutExtension(jsonFilePath);
            string spriteSheetTexturePath = $"SpriteSheets/{spriteSheetName}";

            // Load the SpriteSheet texture
            Texture2D texture = content.Load<Texture2D>(spriteSheetTexturePath);

            // Iterate over each sprite in the JSON
            foreach (var sprite in json.Properties())
            {
                string spriteName = sprite.Name; // Entity name (e.g., "MainMenu", "Reimu")
                JObject animationsData = (JObject)sprite.Value;

                // Skip if no animations data is present
                if (animationsData == null) continue;

                // Create a dictionary to store the animations for the sprite
                Dictionary<string, List<Rectangle>> animations = new();

                // Check for multiple animations, or just use the "Default" animation if present
                foreach (var animation in animationsData)
                {
                    string animationName = animation.Key; // Animation name like "Idle", "MoveLeft", "Default", etc.
                    var framesData = animation.Value;

                    if (framesData != null)
                    {
                        // Create a list to store the frames (rectangles) for this animation
                        List<Rectangle> frames = new();

                        // Parse each frame
                        foreach (var frame in framesData)
                        {
                            // Directly access the frame data
                            int x = (int)frame["X"];
                            int y = (int)frame["Y"];
                            int width = (int)frame["Width"];
                            int height = (int)frame["Height"];

                            // Create a Rectangle from the frame data
                            Rectangle rect = new Rectangle(x, y, width, height);
                            frames.Add(rect);
                        }

                        // Store the animation frames under its name
                        animations[animationName] = frames;
                    }
                }

                // Store the sprite information (including animations) in the _sprites dictionary
                _sprites[spriteName] = new SpriteData(texture, animations, spriteName);
            }
        }

        /// <summary>
        /// Gets a SpriteName by name.
        /// </summary>
        public SpriteData GetSpriteData(string spriteName)
        {
            if (String.IsNullOrEmpty(spriteName)) return new SpriteData(new Texture2D(_graphicsDevice, 1, 1), new Dictionary<string, List<Rectangle>>(), string.Empty);
            return _sprites.TryGetValue(spriteName, out var spriteInfo) ? spriteInfo : GetDefaultSpriteData();
        }

        public SpriteData GetDefaultSpriteData()
        {
            return _sprites["Default"];
        }

        public Texture2D GetPixelTexture(Color color, int width, int height)
        {
            Texture2D pixel = new Texture2D(this._graphicsDevice, width, height);
            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = color;
            }
            pixel.SetData(data);

            return pixel;
        }

    }
}
