using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using System.IO;
namespace BulletHellGame.Managers
{
    public class TextureManager
    {
        private static TextureManager _instance;
        public static TextureManager Instance => _instance ??= new TextureManager();

        private readonly Dictionary<string, Texture2D> _textures = new();
        private readonly Dictionary<string, Dictionary<string, Rectangle>> _spriteRegions = new();

        private TextureManager() { }

        public void LoadTexturesFromJson(ContentManager contentManager, string jsonPath)
        {
            var json = File.ReadAllText(jsonPath);
            var spriteData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);

            foreach (var category in spriteData)
            {
                string spriteSheetName = category.Value.SpriteSheet;
                if (!HasTexture(spriteSheetName))
                {
                    // Load the texture if it's not already loaded
                    LoadTexture(contentManager, spriteSheetName, $"SpriteSheets/{spriteSheetName}");
                }

                // Parse sprite regions
                if (!_spriteRegions.ContainsKey(spriteSheetName))
                {
                    _spriteRegions[spriteSheetName] = new Dictionary<string, Rectangle>();
                }

                foreach (var sprite in category.Value.Sprites)
                {
                    string spriteName = sprite.Name;

                    int x = sprite.Value.X;
                    int y = sprite.Value.Y;
                    int width = sprite.Value.Width;
                    int height = sprite.Value.Height;

                    // Add the sprite region to the dictionary
                    _spriteRegions[spriteSheetName][spriteName] = new Rectangle(x, y, width, height);
                }
            }
        }

        public bool HasTexture(string key) => _textures.ContainsKey(key);

        public void LoadTexture(ContentManager contentManager, string key, string texturePath)
        {
            if (!_textures.ContainsKey(key))
            {
                _textures[key] = contentManager.Load<Texture2D>(texturePath);
            }
        }

        public Texture2D GetTexture(string key)
        {
            if (_textures.TryGetValue(key, out var texture))
            {
                return texture;
            }
            throw new KeyNotFoundException($"Texture '{key}' not found.");
        }

        public Rectangle GetSpriteRegion(string spriteSheetName, string spriteName)
        {
            if (_spriteRegions.TryGetValue(spriteSheetName, out var regions) &&
                regions.TryGetValue(spriteName, out var rectangle))
            {
                return rectangle;
            }
            throw new KeyNotFoundException($"Sprite region '{spriteName}' in '{spriteSheetName}' not found.");
        }
    }
}
