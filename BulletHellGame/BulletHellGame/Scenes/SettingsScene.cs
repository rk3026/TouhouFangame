﻿using BulletHellGame.Managers;
using BulletHellGame.Scenes;
using Microsoft.Xna.Framework.Content;

public class SettingsScene : IScene
{
    private GameTime _gameTime;
    private Texture2D whitePixel;
    private ContentManager _contentManager;
    private GraphicsDevice _graphicsDevice;
    private int selectedIndex;

    private string[] settingsOptions = {
        "Music Volume: ",
        "Sound Effects: ",
        "Debug Mode: ",
        "KeyConfig",
        "Back"
    };

    public SettingsScene(ContentManager contentManager, GraphicsDevice graphicsDevice)
    {
        this._contentManager = contentManager;
        this._graphicsDevice = graphicsDevice;
    }

    public void Load()
    {
        FontManager.Instance.LoadFont(_contentManager, "DFPPOPCorn-W12");

        whitePixel = new Texture2D(_graphicsDevice, 1, 1);
        whitePixel.SetData(new Color[] { Color.White });

        UpdateMenuText();
    }

    private void UpdateMenuText()
    {
        settingsOptions[0] = $"Music Volume: {Math.Round(SettingsManager.Instance.MusicVolume * 10)}/10";
        settingsOptions[1] = $"Sound Effects: {Math.Round(SettingsManager.Instance.SFXVolume * 10)}/10";
        settingsOptions[2] = $"Debug Mode: {(SettingsManager.Instance.Debugging ? "ON" : "OFF")}";
    }

    public void Update(GameTime gameTime)
    {
        _gameTime = gameTime;

        if (InputManager.Instance.ActionPressed(GameAction.Pause))
            SceneManager.Instance.RemoveScene();

        // Wrap-around navigation for Up/Down
        if (InputManager.Instance.ActionPressed(GameAction.Up))
        {
            selectedIndex--;
            if (selectedIndex < 0)
                selectedIndex = settingsOptions.Length - 1;
        }
        if (InputManager.Instance.ActionPressed(GameAction.Down))
        {
            selectedIndex++;
            if (selectedIndex >= settingsOptions.Length)
                selectedIndex = 0;
        }

        if (InputManager.Instance.ActionPressed(GameAction.Left) || InputManager.Instance.ActionPressed(GameAction.Right))
        {
            if (selectedIndex == 0) // Music Volume
            {
                float delta = InputManager.Instance.ActionPressed(GameAction.Left) ? -0.1f : 0.1f;
                SettingsManager.Instance.MusicVolume = Math.Clamp(SettingsManager.Instance.MusicVolume + delta, 0f, 1f);
            }
            else if (selectedIndex == 1) // SFX Volume
            {
                float delta = InputManager.Instance.ActionPressed(GameAction.Left) ? -0.1f : 0.1f;
                SettingsManager.Instance.SFXVolume = Math.Clamp(SettingsManager.Instance.SFXVolume + delta, 0f, 1f);
            }

            UpdateMenuText();
        }

        if (InputManager.Instance.ActionPressed(GameAction.Select))
        {
            if (selectedIndex == settingsOptions.Length - 1)
            {
                SceneManager.Instance.RemoveScene();
            }
            else if (selectedIndex == 2) // Toggle Debug Mode
            {
                SettingsManager.Instance.Debugging = !SettingsManager.Instance.Debugging;
                UpdateMenuText();
            }
            else if (selectedIndex == 3) // Rebind Keys
            {
                SceneManager.Instance.AddScene(new KeyConfigScene(this._contentManager, this._graphicsDevice));
            }
        }
    }


    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.GraphicsDevice.Clear(Color.Black);

        Vector2 titlePosition = new Vector2(100, 50);
        spriteBatch.DrawString(FontManager.Instance.GetFont("DFPPOPCorn-W12"), "Settings", titlePosition, Color.Yellow);

        for (int i = 0; i < settingsOptions.Length; i++)
        {
            Vector2 position = new Vector2(100, 150 + i * 40);
            Color textColor = (i == selectedIndex) ? Color.Red : Color.White;

            if (i == selectedIndex)
            {
                float time = (float)(_gameTime?.TotalGameTime.TotalSeconds ?? 0f);
                float offsetX = (float)Math.Sin(time * 5) * 3;
                float offsetY = (float)Math.Cos(time * 5) * 3;

                var textSize = FontManager.Instance.GetFont("DFPPOPCorn-W12").MeasureString(settingsOptions[i]);
                var boxPosition = position - new Vector2(5, 5) + new Vector2(offsetX, offsetY);
                var boxSize = new Vector2(textSize.X + 10, textSize.Y + 10);

                spriteBatch.Draw(whitePixel, new Rectangle((int)boxPosition.X, (int)boxPosition.Y, (int)boxSize.X, (int)boxSize.Y), Color.White);
                DrawOutlinedText(spriteBatch, settingsOptions[i], position + new Vector2(offsetX, offsetY), Color.Black, 2);
            }

            spriteBatch.DrawString(FontManager.Instance.GetFont("DFPPOPCorn-W12"), settingsOptions[i], position, textColor);
        }
    }

    private void DrawOutlinedText(SpriteBatch spriteBatch, string text, Vector2 position, Color outlineColor, int outlineWidth)
    {
        for (int x = -outlineWidth; x <= outlineWidth; x++)
        {
            for (int y = -outlineWidth; y <= outlineWidth; y++)
            {
                if (x == 0 && y == 0) continue;
                spriteBatch.DrawString(FontManager.Instance.GetFont("DFPPOPCorn-W12"), text, position + new Vector2(x, y), outlineColor);
            }
        }
    }
}
