using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Entities;
using System;

namespace BulletHellGame.Logic.Utilities.EntityDataGenerator.EntityDataGenerators
{
    public static class CollectibleDataGenerator
    {
        private static Random _random = new Random();

        public static List<CollectibleData> GenerateRandomLoot()
        {
            var lootTable = new List<(CollectibleData item, float weight)>
            {
                (new CollectibleData { SpriteName = "PowerUpSmall", Effects = { { CollectibleType.PowerUp, 1 } } }, 0.15f),
                (new CollectibleData { SpriteName = "PowerUpLarge", Effects = { { CollectibleType.PowerUp, 8 } } }, 0.01f),
                (new CollectibleData { SpriteName = "PointItem", Effects = { { CollectibleType.PointItem, 10 } } }, 0.01f),
                (new CollectibleData { SpriteName = "BombItem", Effects = { { CollectibleType.Bomb, 1 } } }, 0.10f),
                (new CollectibleData { SpriteName = "FullPowerItem", Effects = { { CollectibleType.PowerUp, 128 } } }, 0.005f),
                (new CollectibleData { SpriteName = "OneUp", Effects = { { CollectibleType.OneUp, 1 } } }, 0.02f),
                (new CollectibleData { SpriteName = "StarItem", Effects = { { CollectibleType.StarItem, 10 } } }, 0.10f),
                (new CollectibleData { SpriteName = "CherryItem", Effects = { { CollectibleType.CherryItem, 10 } } }, 0.15f)
            };

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
