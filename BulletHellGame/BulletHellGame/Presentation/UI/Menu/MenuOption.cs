namespace BulletHellGame.Presentation.UI.Menu
{
    public class MenuOption
    {
        public string Text { get; set; }
        public Action OnSelect { get; set; }
        public IMenuOptionStyle Style { get; set; }

        public MenuOption(string text, Action onSelect, IMenuOptionStyle style)
        {
            Text = text;
            OnSelect = onSelect;
            Style = style;
        }

        public void ExecuteAction()
        {
            OnSelect?.Invoke();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, int index, Vector2 position, int selectedIndex)
        {
            Style.Draw(spriteBatch, gameTime, Text, index, position, selectedIndex);
        }
    }


}
