using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Managers;
using BulletHellGame.Presentation.Scenes;
using BulletHellGame.Presentation.UI.Menu;
using Microsoft.Xna.Framework.Content;
using System.Linq;

public class MainMenuScene : IScene
{
    private GameTime _gameTime;
    private SpriteData background;
    private ContentManager _contentManager;
    private GraphicsDevice _graphicsDevice;
    private MenuNavigator menuNavigator;
    private List<MenuOption> menuOptions;

    public bool IsOverlay => false;
    public bool IsMenu => true;

    public MainMenuScene(ContentManager contentManager, GraphicsDevice graphicsDevice)
    {
        this._contentManager = contentManager;
        this._graphicsDevice = graphicsDevice;

        // Initialize the menu options with actions and a style
        var style = new MenuOptionStyle1(contentManager, graphicsDevice);
        menuOptions = new List<MenuOption>
        {
            new MenuOption("Start Game", () => SceneManager.Instance.AddScene(new CharacterSelectScene(_contentManager, _graphicsDevice)), style),
            new MenuOption("Settings", () => SceneManager.Instance.AddScene(new SettingsScene(_contentManager, _graphicsDevice)), style),
            new MenuOption("Exit", () => Environment.Exit(0), style)
        };

        // Initialize the menu navigator
        menuNavigator = new MenuNavigator(menuOptions.Count);
    }

    public void Load()
    {
        // Load content
        background = TextureManager.Instance.GetSpriteData("MainMenu");
        FontManager.Instance.LoadFont(_contentManager, "DFPPOPCorn-W12");

        BGMManager.Instance.PlayBGM(_contentManager, "01. Ghostly Dream ~ Snow or Cherry Petal");
    }

    public void Update(GameTime gameTime)
    {
        _gameTime = gameTime;

        // Update the menu navigator
        menuNavigator.Update(gameTime);

        if (InputManager.Instance.ActionPressed(GameAction.Select))
        {
            menuOptions[menuNavigator.SelectedIndex].ExecuteAction();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(background.Texture, Vector2.Zero, background.Animations.First().Value[0], Color.White);

        // Draw the menu options using their specific styles
        for (int i = 0; i < menuOptions.Count; i++)
        {
            Vector2 position = new Vector2(100, 100 + i * 40);
            menuOptions[i].Draw(spriteBatch, _gameTime, i, position, menuNavigator.SelectedIndex);
        }
    }
}
