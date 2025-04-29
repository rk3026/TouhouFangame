using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Utilities.EntityDataGenerator;
using BulletHellGame.Logic.Utilities.EntityDataGenerator.EntityDataGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace BulletHellGame.DataAccess.DataLoaders
{
    public static class BulletPatternLoader
    {
        const string BULLET_PATTERN_JSON_PATH = "./Data/Levels/bulletPatterns.jason";

        public static List<Vector2> GetPattern(string patternId)
        {
            var patternJson = JsonUtility.LoadJson(BULLET_PATTERN_JSON_PATH)[patternId];

            List<Vector2> bulletPattern = new List<Vector2>();

            foreach (var fireDirection in patternJson)
            {
                float x = fireDirection["x"].Value<float>();
                float y = fireDirection["y"].Value<float>();
                bulletPattern.Add(new Vector2(x, y));
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
        [JsonProperty("GAME_SPEED")]
        public float Speed { get; set; }
        [JsonProperty("angle_offset")]
        public float AngleOffset { get; set; }
        [JsonProperty("spread")]
        public float Spread { get; set; }
        [JsonProperty("fire_rate")]
        public float FireRate { get; set; }
    }
}
