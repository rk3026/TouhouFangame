using BulletHellGame.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace BulletHellGame.Components
{
    public class MovementPatternManager
    {
        private static MovementPatternManager _instance;
        public static MovementPatternManager Instance => _instance ??= new MovementPatternManager();

        private Dictionary<string, List<MovementData>> _movementPatterns;

        private MovementPatternManager()
        {
            _movementPatterns = new Dictionary<string, List<MovementData>>();
            LoadMovementPatterns();
        }

        private void LoadMovementPatterns()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/MovementPatterns.json");
            string json = File.ReadAllText(filePath);

            // Deserialize the movement patterns from the JSON file
            var movementPatterns = JsonConvert.DeserializeObject<Dictionary<string, List<JObject>>>(json);

            // Manually create MovementData and cache it
            foreach (var pattern in movementPatterns)
            {
                List<MovementData> patternData = new List<MovementData>();

                foreach (var item in pattern.Value)
                {
                    float time = item["time"].Value<float>();
                    var velocityObj = item["velocity"] as JObject;
                    float x = velocityObj["x"].Value<float>();
                    float y = velocityObj["y"].Value<float>();

                    patternData.Add(new MovementData
                    {
                        Time = time,
                        Velocity = new Vector2(x, y)
                    });
                }

                // Cache the pattern data for reuse
                _movementPatterns[pattern.Key] = patternData;
            }
        }

        // Retrieve a movement pattern by name
        public List<MovementData> GetPattern(string patternName)
        {
            if (_movementPatterns.ContainsKey(patternName))
            {
                return _movementPatterns[patternName];
            }
            else
            {
                throw new Exception($"Movement pattern '{patternName}' not found.");
            }
        }
    }
}
