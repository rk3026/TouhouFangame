using Newtonsoft.Json;
using System.IO;

namespace BulletHellGame.DataAccess.DataLoaders
{
    public class BulletPatternLoader
    {
        public Dictionary<string, BulletPatternData> LoadBulletPatterns()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/BulletPatterns/BulletPatterns.json");
            string json = File.ReadAllText(filePath);

            var bulletPatterns = JsonConvert.DeserializeObject<BulletPatternContainer>(json);
            var patternDict = new Dictionary<string, BulletPatternData>();

            foreach (var pattern in bulletPatterns.Patterns)
            {
                patternDict[pattern.Id] = pattern;
            }

            return patternDict;
        }
    }

    public class BulletPatternContainer
    {
        [JsonProperty("patterns")]
        public List<BulletPatternData> Patterns { get; set; }
    }

    public class BulletPatternData
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("id")]
        public string Type { get; set; }
        [JsonProperty("speed")]
        public float Speed { get; set; }
        [JsonProperty("angle_offset")]
        public float AngleOffset { get; set; }
        [JsonProperty("spread")]
        public float Spread { get; set; }
        [JsonProperty("fire_rate")]
        public float FireRate { get; set; }
    }
}
