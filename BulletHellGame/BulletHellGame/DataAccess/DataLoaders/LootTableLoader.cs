using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Utilities.EntityDataGenerator;
using BulletHellGame.Logic.Utilities.EntityDataGenerator.EntityDataGenerators;

namespace BulletHellGame.DataAccess.DataLoaders
{
    public static class LootTableLoader
    {
        public static List<CollectibleData> GetLoot(string lootTableId)
        {
            return CollectibleDataGenerator.GenerateRandomLoot();
        }
    }
}
