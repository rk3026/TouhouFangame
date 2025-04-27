using System.Linq;

namespace BulletHellGame.Logic.Managers
{
    public class ShaderManager
    {
        private static ShaderManager _instance;
        public static ShaderManager Instance => _instance ??= new ShaderManager();

        public bool ShaderEnabled { get; set; }
        public Effect ActiveShader { get; private set; }
        public Dictionary<string, Effect> Shaders { get; private set; }

        public IReadOnlyList<string> ShaderNames => Shaders.Keys.ToList();

        private ShaderManager()
        {
            Shaders = new Dictionary<string, Effect>();
            ShaderEnabled = false;
        }

        public void Update(GameTime gameTime)
        {
            if (ShaderEnabled)
            {
                if (ActiveShader != null && ActiveShader.Parameters["time"] != null)
                    ActiveShader.Parameters["time"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            }
        }

        public void StoreShader(string shaderName, Effect shader)
        {
            Shaders[shaderName] = shader;
        }

        public void SetActiveShader(string shaderName)
        {
            if (Shaders.TryGetValue(shaderName, out var shader))
            {
                ActiveShader = shader;
            }
        }
    }
}
