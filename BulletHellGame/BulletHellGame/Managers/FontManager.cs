using Microsoft.Xna.Framework.Content;

namespace BulletHellGame.Managers
{
    public class FontManager
    {
        private static FontManager _instance;
        public static FontManager Instance => _instance ??= new FontManager();

        private Dictionary<string, SpriteFont> _fonts;

        private FontManager()
        {
            _fonts = new Dictionary<string, SpriteFont>();
        }

        // Loads a font and stores it in the dictionary
        public void LoadFont(ContentManager content, string fontName)
        {
            if (!_fonts.ContainsKey(fontName)) // Avoid reloading the same font
            {
                _fonts[fontName] = content.Load<SpriteFont>("Fonts/" + fontName);
            }
        }

        // Retrieves a loaded font by name
        public SpriteFont GetFont(string fontName)
        {
            if (_fonts.ContainsKey(fontName))
            {
                return _fonts[fontName];
            }
            else
            {
                throw new KeyNotFoundException($"Font '{fontName}' not loaded.");
            }
        }
    }
}
