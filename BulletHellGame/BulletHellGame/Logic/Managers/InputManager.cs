using BulletHellGame.Presentation.Scenes;

namespace BulletHellGame.Logic.Managers
{
    public class InputManager
    {
        private static InputManager _instance;
        public static InputManager Instance => _instance ??= new InputManager();

        private static KeyboardState _lastKeyboard;
        private static KeyboardState _currentKeyboard;
        private Dictionary<GameAction, string> _soundMap = new(); // Map of GameAction to sound names

        private KeybindManager _keybindManager;

        public KeybindManager KeybindManager => _keybindManager;

        private InputManager()
        {
            _keybindManager = new KeybindManager();
            SceneManager.Instance.SceneChanged += SetSoundMapForCurrentScene;
            SetSoundMapForCurrentScene();
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
            bool pressed = _currentKeyboard.IsKeyDown(_keybindManager.Keybindings[action]) &&
                           _lastKeyboard.IsKeyUp(_keybindManager.Keybindings[action]);

            if (pressed) PlaySoundForAction(action);

            return pressed;
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

        // Maps GameActions to sounds for each scene
        private void SetSoundMapForCurrentScene()
        {
            IScene scene = SceneManager.Instance.GetCurrentScene();
            if (scene == null) return;

            if (scene.IsMenu)
            {
                _soundMap[GameAction.MenuUp] = "menu_move";
                _soundMap[GameAction.MenuDown] = "menu_move";
                _soundMap[GameAction.Select] = "menu_select";
                _soundMap[GameAction.Pause] = "menu_back";
            }
            else
            {
                _soundMap.Clear();
            }
        }

        // Plays the sound associated with the given action
        private void PlaySoundForAction(GameAction action)
        {
            if (_soundMap.TryGetValue(action, out string soundName))
            {
                SFXManager.Instance.PlaySound(soundName, varySound: false);
            }
        }
    }
}
