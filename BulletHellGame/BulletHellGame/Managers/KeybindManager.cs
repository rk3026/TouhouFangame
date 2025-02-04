using System.IO;
using System.Linq;
using System.Text.Json;

namespace BulletHellGame.Managers
{
    public enum GameAction
    {
        Shoot,
        Bomb,
        Slow,
        SkipText,
        Pause,
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Select,
        MenuUp,
        MenuDown,
        MenuLeft,
        MenuRight,
    }

    public class KeybindManager
    {
        private Dictionary<GameAction, Keys> _keybindings;
        private readonly string _configFilePath = "keybindings.json";

        public Dictionary<GameAction, Keys> Keybindings => _keybindings;

        public KeybindManager()
        {
            _keybindings = new Dictionary<GameAction, Keys>();
            LoadDefaults();
            LoadKeybindings();
        }

        private void LoadDefaults()
        {
            _keybindings[GameAction.Shoot] = Keys.Space;
            _keybindings[GameAction.Bomb] = Keys.K;
            _keybindings[GameAction.Slow] = Keys.LeftShift;
            _keybindings[GameAction.SkipText] = Keys.Tab;
            _keybindings[GameAction.Pause] = Keys.Escape;
            _keybindings[GameAction.MoveUp] = Keys.W;
            _keybindings[GameAction.MoveDown] = Keys.S;
            _keybindings[GameAction.MoveLeft] = Keys.A;
            _keybindings[GameAction.MoveRight] = Keys.D;
            _keybindings[GameAction.Select] = Keys.Enter;

            _keybindings[GameAction.MenuUp] = Keys.Up;
            _keybindings[GameAction.MenuDown] = Keys.Down;
            _keybindings[GameAction.MenuLeft] = Keys.Left;
            _keybindings[GameAction.MenuRight] = Keys.Right;
        }

        public void ResetToDefaults()
        {
            LoadDefaults();
            SaveKeybindings();
        }

        public void SetKeybind(GameAction action, Keys newKey)
        {
            if (!_keybindings.ContainsValue(newKey)) // Prevent duplicate bindings
            {
                _keybindings[action] = newKey;
                SaveKeybindings();
            }
        }

        private void SaveKeybindings()
        {
            var serializedData = _keybindings.ToDictionary(kv => kv.Key.ToString(), kv => kv.Value.ToString());
            File.WriteAllText(_configFilePath, JsonSerializer.Serialize(serializedData, new JsonSerializerOptions { WriteIndented = true }));
        }

        private void LoadKeybindings()
        {
            if (File.Exists(_configFilePath))
            {
                string json = File.ReadAllText(_configFilePath);
                var deserializedData = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

                if (deserializedData != null)
                {
                    _keybindings = deserializedData.ToDictionary(
                        kv => Enum.Parse<GameAction>(kv.Key),
                        kv => Enum.Parse<Keys>(kv.Value)
                    );
                }
            }
        }
    }
}
