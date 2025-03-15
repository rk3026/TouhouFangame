using BulletHellGame.Systems;
using BulletHellGame.Systems.RenderingSystems;
using System.Linq;
using System.Reflection;

namespace BulletHellGame.Managers
{
    public class SystemManager
    {
        private List<ILogicSystem> _logicSystems;
        private List<IRenderingSystem> _renderingSystems;

        public SystemManager(GraphicsDevice graphicsDevice)
        {
            _logicSystems = new List<ILogicSystem>();
            _renderingSystems = new List<IRenderingSystem>();

            // Dynamically load logic systems
            LoadSystems<ILogicSystem>("BulletHellGame.Systems.LogicSystems");

            // Dynamically load rendering systems
            LoadSystems<IRenderingSystem>("BulletHellGame.Systems.RenderingSystems", graphicsDevice);
        }

        private void LoadSystems<T>(string namespaceName, GraphicsDevice graphicsDevice = null)
        {
            // Use reflection to load the systems
            var systemTypes = Assembly.GetExecutingAssembly()
                                      .GetTypes()
                                      .Where(t => t.IsClass && !t.IsAbstract && typeof(T).IsAssignableFrom(t))
                                      .Where(t => t.Namespace == namespaceName)
                                      .ToList();

            List<T> tempList = new List<T>();

            foreach (var type in systemTypes)
            {
                if (typeof(T) == typeof(IRenderingSystem) && graphicsDevice != null)
                {
                    var system = Activator.CreateInstance(type, graphicsDevice) as IRenderingSystem;
                    if (system != null)
                    {
                        tempList.Add((T)system);
                    }
                }
                else if (typeof(T) == typeof(ILogicSystem))
                {
                    var system = Activator.CreateInstance(type) as ILogicSystem;
                    if (system != null)
                    {
                        tempList.Add((T)system);
                    }
                }
            }

            // Sort rendering systems by drawing priority (lower = draw first)
            if (typeof(T) == typeof(IRenderingSystem))
            {
                _renderingSystems = tempList.Cast<IRenderingSystem>()
                                            .OrderBy(s => s.DrawPriority)
                                            .ToList();
            }
            else
            {
                _logicSystems = tempList.Cast<ILogicSystem>().ToList();
            }
        }


        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (var system in _logicSystems)
            {
                system.Update(entityManager, gameTime);
            }
        }

        public void Draw(EntityManager entityManager, SpriteBatch spriteBatch)
            {
                foreach (var system in _renderingSystems.OrderBy(system => system.DrawPriority))
                {
                    if (system is DebugRenderingSystem && !SettingsManager.Instance.Debugging)
                    {
                        continue;
                    }

                    system.Draw(entityManager, spriteBatch);
                }
            }

        }
}
