using BulletHellGame.Logic.Managers;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace BulletHellGame.Presentation.Scenes
{
    public class ShaderConfigScene : IScene
    {
        private ContentManager _contentManager;
        private GraphicsDevice _graphicsDevice;
        private int selectedIndex = 0;
        private int shaderIndex = 0;
        private string[] shaderNames;

        public bool IsOverlay => false;

        public ShaderConfigScene(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            _contentManager = contentManager;
            _graphicsDevice = graphicsDevice;
            shaderNames = ShaderManager.Instance.ShaderNames.ToArray();

            // Set shaderIndex to match the currently active shader
            if (ShaderManager.Instance.ActiveShader != null)
            {
                string activeShaderName = ShaderManager.Instance.ShaderNames
                    .FirstOrDefault(name => ShaderManager.Instance.ActiveShader == ShaderManager.Instance.Shaders[name]);

                if (activeShaderName != null)
                {
                    shaderIndex = Array.IndexOf(shaderNames, activeShaderName);
                }
            }
        }

        public void Load() { }

        public void Update(GameTime gameTime)
        {
            if (InputManager.Instance.ActionPressed(GameAction.Pause))
                SceneManager.Instance.RemoveScene();

            if (InputManager.Instance.ActionPressed(GameAction.MenuUp))
            {
                selectedIndex = (selectedIndex - 1 + 3) % 3; // Three options: Shader selection, Shader toggle, and Back
            }
            else if (InputManager.Instance.ActionPressed(GameAction.MenuDown))
            {
                selectedIndex = (selectedIndex + 1) % 3;
            }

            if (selectedIndex == 0 && shaderNames.Length > 0) // Shader Selection
            {
                if (InputManager.Instance.ActionPressed(GameAction.MenuLeft))
                {
                    shaderIndex = (shaderIndex - 1 + shaderNames.Length) % shaderNames.Length;
                }
                else if (InputManager.Instance.ActionPressed(GameAction.MenuRight))
                {
                    shaderIndex = (shaderIndex + 1) % shaderNames.Length;
                }

                ShaderManager.Instance.SetActiveShader(shaderNames[shaderIndex]);
            }
            else if (selectedIndex == 1) // Shader Toggle
            {
                if (InputManager.Instance.ActionPressed(GameAction.Select))
                {
                    ShaderManager.Instance.ShaderEnabled = !ShaderManager.Instance.ShaderEnabled;
                }
            }
            else if (selectedIndex == 2) // Back
            {
                if (InputManager.Instance.ActionPressed(GameAction.Select))
                {
                    SceneManager.Instance.RemoveScene();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.Black);

            Vector2 titlePosition = new Vector2(100, 50);
            spriteBatch.DrawString(FontManager.Instance.GetFont("DFPPOPCorn-W12"), "Shader Switcher", titlePosition, Color.Yellow);

            // Draw Shader Selection
            Vector2 shaderPosition = new Vector2(100, 150);
            Color shaderTextColor = selectedIndex == 0 ? Color.Red : Color.White;
            string selectedShader = shaderNames.Length > 0 ? shaderNames[shaderIndex] : "None";
            spriteBatch.DrawString(FontManager.Instance.GetFont("DFPPOPCorn-W12"), $"Shader: {selectedShader}", shaderPosition, shaderTextColor);

            // Draw Shader Toggle
            string shaderStatus = ShaderManager.Instance.ShaderEnabled ? "ON" : "OFF";
            Vector2 statusPosition = new Vector2(100, 200);
            Color statusTextColor = selectedIndex == 1 ? Color.Red : Color.Cyan;
            spriteBatch.DrawString(FontManager.Instance.GetFont("DFPPOPCorn-W12"), $"Enabled: {shaderStatus}", statusPosition, statusTextColor);

            // Draw Back Option
            Vector2 backPosition = new Vector2(100, 250);
            Color backTextColor = selectedIndex == 2 ? Color.Red : Color.White;
            spriteBatch.DrawString(FontManager.Instance.GetFont("DFPPOPCorn-W12"), "Back", backPosition, backTextColor);
        }
    }
}
