using BulletHellGame.DataAccess.DataLoaders;

namespace BulletHellGame.Logic.Managers
{
    public class BulletPatternManager
    {
        private static BulletPatternManager _instance;
        public static BulletPatternManager Instance => _instance ??= new BulletPatternManager();

        private Dictionary<string, BulletPatternData> _bulletPatterns;

        private BulletPatternManager()
        {
            _bulletPatterns = new BulletPatternLoader().LoadBulletPatterns();
        }

        public BulletPatternData GetPattern(string patternId)
        {
            if (_bulletPatterns.TryGetValue(patternId, out var pattern))
            {
                return pattern;
            }
            else
            {
                Console.WriteLine($"Bullet pattern '{patternId}' not found.");
                return null;
            }
        }
    }
}
