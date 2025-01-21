using Newtonsoft.Json.Linq;
using Microsoft.Xna.Framework.Content;
using System.IO;
using BulletHellGame.Data;

namespace BulletHellGame.Managers
{
    public class TextureManager
    {
        private static TextureManager _instance;
        public static TextureManager Instance => _instance ??= new TextureManager();

        // Dictionary to store sprite information by name
        private readonly Dictionary<string, SpriteInfo> _sprites = new();

        private TextureManager() { }

        /// <summary>
        /// Loads textures and sprite data from a JSON file.
        /// </summary>
        public void LoadTexturesFromJson(ContentManager content, string jsonFilePath)
        {
            // Read and parse the JSON file
            string jsonString = File.ReadAllText(jsonFilePath);
            JObject json = JObject.Parse(jsonString);

            foreach (var category in json.Properties())
            {
                string spriteSheetName = category.Name; // Sprite Sheet name (e.g., "Characters", "MenuAndOtherScreens")
                string spriteSheetTexturePath = $"SpriteSheets/{spriteSheetName}";

                // Load the SpriteSheet
                Texture2D texture = content.Load<Texture2D>(spriteSheetTexturePath);

                // Parse sprite data
                var spriteData = category.Value["Sprites"];
                if (spriteData == null) continue;

                foreach (var sprite in spriteData.Children<JProperty>())
                {
                    string spriteName = sprite.Name; // Sprite name (e.g., "MainMenu", "GameLogo")
                    var framesData = sprite.Value["Frames"];
                    if (framesData == null) continue;

                    // Create a list to store the frames (rectangles)
                    List<Rectangle> frames = new();

                    foreach (var frame in framesData)
                    {
                        var rectData = frame["Rect"];
                        if (rectData != null)
                        {
                            // Create a Rectangle for each frame
                            Rectangle rect = new Rectangle(
                                (int)rectData["X"],
                                (int)rectData["Y"],
                                (int)rectData["Width"],
                                (int)rectData["Height"]
                            );

                            frames.Add(rect);
                        }
                    }

                    // Store the sprite information (now with multiple frames)
                    _sprites[spriteName] = new SpriteInfo
                    {
                        Texture = texture,
                        Rects = frames,  // Store the list of frames
                        Name = spriteName
                    };
                }
            }
        }

        /// <summary>
        /// Gets a SpriteInfo by name.
        /// </summary>
        public SpriteInfo GetSpriteInfo(string spriteName)
        {
            return _sprites.TryGetValue(spriteName, out var spriteInfo) ? spriteInfo : null;
        }
    }
}