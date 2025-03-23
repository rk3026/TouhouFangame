using BulletHellGame.DataAccess.DataLoaders;
using BulletHellGame.DataAccess.DataTransferObjects;

namespace BulletHellGame.Logic.Managers
{

    // Manages access to movement patterns
    public class MovementPatternManager
    {
        private static MovementPatternManager _instance;
        public static MovementPatternManager Instance => _instance ??= new MovementPatternManager();

        private readonly Dictionary<string, List<MovementData>> _movementPatterns;

        private MovementPatternManager()
        {
            _movementPatterns = new MovementPatternLoader().LoadMovementPatterns();
        }

        public List<MovementData> GetPattern(string patternName)
        {
            if (_movementPatterns.TryGetValue(patternName, out var patternData))
            {
                return patternData;
            }
            else
            {
                Console.WriteLine($"Movement pattern '{patternName}' not found.");
                return null;
            }
        }
    }
}
