using BulletHellGame.Managers;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace BulletHellGame.Scenes
{
    public class KeyConfigScene : IScene
    {
        private bool _waitingForKey;
        private bool _waitingForKeyRelease;
        private int _selectedIndex;
        private string[] actions;
        private SpriteFont font;
        private Texture2D whitePixel;
        private GraphicsDevice _graphicsDevice;
        private ContentManager _contentManager;

        public KeyConfigScene(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _contentManager = contentManager;
            actions = Enum.GetNames(typeof(GameAction)) // buttons for each control ("Left", "Down", "Pause", etc.)
                .Append("Reset to Defaults")            // Reset the key bindings
                .Append("Back")                         // Go back to previous scene
                .ToArray();
        }

        public void Load()
        {
            whitePixel = new Texture2D(_graphicsDevice, 1, 1);
            whitePixel.SetData(new Color[] { Color.White });
            font = FontManager.Instance.GetFont("DFPPOPCorn-W12");
        }

        public void Update(GameTime gameTime)
        {
            InputManager.Instance.Update();
            var keyboardState = Keyboard.GetState();

            if (!_waitingForKey)
            {
                if (InputManager.Instance.ActionPressed(GameAction.MenuUp))
                {
                    _selectedIndex--;
                    if (_selectedIndex < 0)
                        _selectedIndex = actions.Length - 1;
                }
                if (InputManager.Instance.ActionPressed(GameAction.MenuDown))
                {
                    _selectedIndex++;
                    if (_selectedIndex >= actions.Length)
                        _selectedIndex = 0;
                }

                if (InputManager.Instance.ActionPressed(GameAction.Pause))
                    SceneManager.Instance.RemoveScene();

                if (InputManager.Instance.ActionPressed(GameAction.Select))
                {
                    if (_selectedIndex == actions.Length - 2) // Reset to Defaults
                    {
                        InputManager.Instance.KeybindManager.ResetToDefaults();
                    }
                    else if (_selectedIndex == actions.Length - 1) // Back
                    {
                        SceneManager.Instance.RemoveScene();
                    }
                    else
                    {
                        _waitingForKey = true;
                        _waitingForKeyRelease = true; // Wait for all keys to be release
                    }
                }
            }

            if (_waitingForKey)
            {
                Keys[] pressedKeys = keyboardState.GetPressedKeys();

                if (_waitingForKeyRelease)
                {
                    if (pressedKeys.Length == 0)
                    {
                        _waitingForKeyRelease = false;
                    }
                }
                else
                {
                    // Only accept new key presses now that all keys were released
                    if (pressedKeys.Length > 0)
                    {
                        Keys newKey = pressedKeys[0];

                        if (Enum.TryParse(actions[_selectedIndex], out GameAction gameAction))
                        {
                            InputManager.Instance.RebindKey(gameAction, newKey);
                        }

                        _waitingForKey = false;
                    }
                }
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.Black);
            Vector2 position = new Vector2(100, 25);
            spriteBatch.DrawString(font, "Rebind Keys", position, Color.Yellow);

            for (int i = 0; i < actions.Length; i++)
            {
                bool isSelected = (i == _selectedIndex);
                Color color = isSelected ? Color.Red : Color.White;

                if (i == actions.Length - 2)
                {
                    spriteBatch.DrawString(font, actions[i], new Vector2(100, 75 + i * 25), color);
                }
                else if (i == actions.Length - 1)
                {
                    spriteBatch.DrawString(font, actions[i], new Vector2(100, 75 + i * 25), color);
                }
                else if (Enum.TryParse(actions[i], out GameAction gameAction))
                {
                    string keyText = InputManager.Instance.KeybindManager.Keybindings.ContainsKey(gameAction)
                        ? InputManager.Instance.KeybindManager.Keybindings[gameAction].ToString()
                        : "Not Bound";

                    spriteBatch.DrawString(font, $"{actions[i]}: {keyText}", new Vector2(100, 75 + i * 25), color);
                }
            }

            if (_waitingForKey)
            {
                spriteBatch.DrawString(font, $"Press a new key for {actions[_selectedIndex]}...", new Vector2(300, 75 + _selectedIndex*25), Color.Cyan);
            }
        }
    }
}
