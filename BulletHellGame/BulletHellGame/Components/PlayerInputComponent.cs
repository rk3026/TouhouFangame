namespace BulletHellGame.Components
{
    public class PlayerInputComponent : IComponent
    {
        // Movements
        public bool IsMovingUp { get; set; }
        public bool IsMovingDown { get; set; }
        public bool IsMovingLeft { get; set; }
        public bool IsMovingRight { get; set; }

        // Action states
        public bool IsShooting { get; set; }

        // Player Modes
        public bool IsFocused { get; set; }

        // Keyboard states
        public KeyboardState PreviousKeyboardState { get; set; }
        public KeyboardState CurrentKeyboardState { get; set; }

        // Input history (maybe useful later?)
        public List<Keys> PressedKeys { get; set; }

        public PlayerInputComponent()
        {
            PressedKeys = new List<Keys>();
        }
    }
}
