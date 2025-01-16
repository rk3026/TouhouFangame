using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using System.IO;

namespace BulletHellGame.Managers
{
    public class TextureManager
    {
        private static TextureManager _instance;
        public static TextureManager Instance => _instance ??= new TextureManager();

        private ContentManager _contentManager;
        private readonly Dictionary<string, Texture2D> _textures = new();

        // Private constructor to prevent external instantiation
        private TextureManager() { }

        // Method to initialize the ContentManager (called during game initialization)
        public void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public void LoadTexturesFromJson(string jsonPath)
        {
            if (_contentManager == null)
            {
                throw new InvalidOperationException("TextureManager has not been initialized with a ContentManager.");
            }

            var json = File.ReadAllText(jsonPath);
            var assets = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);

            if (assets.TryGetValue("Textures", out var textures))
            {
                foreach (var asset in textures)
                {
                    if (!_textures.ContainsKey(asset.Key))
                    {
                        _textures[asset.Key] = _contentManager.Load<Texture2D>(asset.Value);
                    }
                }
            }
        }

        public Texture2D GetTexture(string key)
        {
            if (_textures.TryGetValue(key, out var texture))
            {
                return texture;
            }
            throw new KeyNotFoundException($"Texture with key '{key}' not found.");
        }
    }
}
