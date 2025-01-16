using BulletHellGame.Managers;

public class InputManager
{
    private static InputManager _instance;
    public static InputManager Instance => _instance ??= new InputManager();

    private static KeyboardState _lastKeyboard;
    private static KeyboardState _currentKeyboard;

    private InputManager() { }

    public static bool KeyPressed(Keys key)
    {
        return _currentKeyboard.IsKeyDown(key) && _lastKeyboard.IsKeyUp(key);
    }

    public static bool KeyDown(Keys key)
    {
        return _currentKeyboard.IsKeyDown(key);
    }

    public static void Update()
    {
        _lastKeyboard = _currentKeyboard;
        _currentKeyboard = Keyboard.GetState();
    }
}
