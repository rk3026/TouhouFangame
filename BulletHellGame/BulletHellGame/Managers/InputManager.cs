namespace BulletHellGame.Managers
{
    public class InputManager
    {
        private static InputManager _instance;
        public static InputManager Instance => _instance ??= new InputManager();

        private static KeyboardState _lastKeyboard;
        private static KeyboardState _currentKeyboard;

        private KeybindManager _keybindManager;

        public KeybindManager KeybindManager => _keybindManager;

        private InputManager()
        {
            _keybindManager = new KeybindManager();
        }

        public bool KeyPressed(Keys key)
        {
            return _currentKeyboard.IsKeyDown(key) && _lastKeyboard.IsKeyUp(key);
        }

        public bool KeyReleased(Keys key)
        {
            return _lastKeyboard.IsKeyDown(key) && _currentKeyboard.IsKeyUp(key);
        }
        public bool ActionPressed(GameAction action)
        {
            return _currentKeyboard.IsKeyDown(_keybindManager.Keybindings[action]) &&
                   _lastKeyboard.IsKeyUp(_keybindManager.Keybindings[action]);
        }

        public bool ActionReleased(GameAction action)
        {
            return _lastKeyboard.IsKeyDown(_keybindManager.Keybindings[action]) &&
                   _currentKeyboard.IsKeyUp(_keybindManager.Keybindings[action]);
        }

        public bool ActionDown(GameAction action)
        {
            return _currentKeyboard.IsKeyDown(_keybindManager.Keybindings[action]);
        }

        public void Update()
        {
            _lastKeyboard = _currentKeyboard;
            _currentKeyboard = Keyboard.GetState();
        }

        public void RebindKey(GameAction action, Keys newKey)
        {
            _keybindManager.SetKeybind(action, newKey);
        }
    }
}
