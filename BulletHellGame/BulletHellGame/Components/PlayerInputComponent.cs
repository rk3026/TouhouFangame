namespace BulletHellGame.Components
{
    public class PlayerInputComponent : IComponent
    {
        // Store key states for movement
        public bool IsMovingUp { get; set; }
        public bool IsMovingDown { get; set; }
        public bool IsMovingLeft { get; set; }
        public bool IsMovingRight { get; set; }

        // Store action states (e.g., for shooting, using abilities)
        public bool IsShooting { get; set; }
        public bool IsJumping { get; set; }
        public bool IsInteracting { get; set; }

        // Store state for special modes (e.g., focused mode)
        public bool IsFocused { get; set; }

        // Store any other player input-related states you may need
        public KeyboardState PreviousKeyboardState { get; set; }
        public KeyboardState CurrentKeyboardState { get; set; }

        // Optionally, you can track input history for more advanced features
        public List<Keys> PressedKeys { get; set; }

        public PlayerInputComponent()
        {
            PressedKeys = new List<Keys>();
        }
    }
}
