using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Presentation.UI.Menu
{
    public class MenuNavigator
    {
        public int SelectedIndex { get; private set; }
        private readonly int menuOptionCount;

        public MenuNavigator(int menuOptionCount)
        {
            this.menuOptionCount = menuOptionCount;
            SelectedIndex = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (InputManager.Instance.ActionPressed(GameAction.MenuUp))
            {
                SelectedIndex--;
                if (SelectedIndex < 0)
                    SelectedIndex = menuOptionCount - 1; // Wrap to last option
            }
            if (InputManager.Instance.ActionPressed(GameAction.MenuDown))
            {
                SelectedIndex++;
                if (SelectedIndex >= menuOptionCount)
                    SelectedIndex = 0; // Wrap to first option
            }
        }
    }

}
