using BulletHellGame.DataAccess.DataTransferObjects;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace BulletHellGame.DataAccess.DataLoaders
{
    // Handles loading movement patterns from JSON
    public class MovementPatternLoader
    {
        public Dictionary<string, List<MovementData>> LoadMovementPatterns()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/MovementPatterns/MovementPatterns.json");
            string json = File.ReadAllText(filePath);

            var movementPatterns = JsonConvert.DeserializeObject<Dictionary<string, List<JObject>>>(json);
            var loadedPatterns = new Dictionary<string, List<MovementData>>();

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
                loadedPatterns[pattern.Key] = patternData;
            }

            return loadedPatterns;
        }
    }
}
