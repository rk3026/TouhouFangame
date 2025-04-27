using Newtonsoft.Json.Linq;
using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Entities;

namespace BulletHellGame.DataAccess.DataLoaders
{
    public static class LootTableLoader
    {
        const string LOOT_JSON_PATH = "./Data/Levels/loots.json";

        private static Random _random = new Random();

        public static List<CollectibleData> GetLoot(string lootTableId)
        {
            var lootTableJson = JsonUtility.LoadJson(LOOT_JSON_PATH)[lootTableId];

            var lootTable = new List<(CollectibleData item, float weight)>();

            foreach (var lootJson in lootTableJson)
            {
                string sprite = lootJson["sprite"].Value<string>();
                CollectibleType type = Enum.Parse<CollectibleType>(lootJson["type"].Value<string>());
                int quantity = lootJson["quantity"].Value<int>();
                float probability = lootJson["probability"].Value<float>();

                lootTable.Add((new CollectibleData { SpriteName = sprite, Effects = { { type, quantity } } }, probability));
            }

            var selectedLoot = new List<CollectibleData>();

            foreach (var (item, weight) in lootTable)
            {
                if (_random.NextDouble() < weight) // Compare against weight (probability)
                {
                    selectedLoot.Add(item);
                }
            }

            return selectedLoot;
        }
    }
}
