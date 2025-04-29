using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Utilities.EntityDataGenerator;
using BulletHellGame.Logic.Utilities.EntityDataGenerator.EntityDataGenerators;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BulletHellGame.DataAccess.DataLoaders
{
    /// <summary>
    /// This class will read data from GruntData.json.
    /// It will load all enemy types and return the specific one requested.
    /// </summary>
    public static class EnemyDataLoader
    {
        const string ENEMY_JSON_PATH = "./Data/Levels/enemies.json";

        public static GruntData GetGrunt(string enemyId)
        {
            var enemyJson = JsonUtility.LoadJson(ENEMY_JSON_PATH)[enemyId];

            GruntData gruntData = new GruntData();
            gruntData.Health = enemyJson["health"].Value<int>();
            gruntData.SpriteName = enemyJson["sprite"].Value<string>();
            gruntData.Name = enemyId;
            gruntData.MovementPattern = enemyJson["movementPattern"].Value<string>();

            List<WeaponData> weapons = new List<WeaponData>();
            foreach(var weaponId in enemyJson["weapons"])
            {
                weapons.Add(WeaponDataLoader.GetWeapon(weaponId.Value<string>()));
            }
            
            gruntData.Weapons = weapons;
            gruntData.Loot = LootTableLoader.GetLoot(enemyJson["lootTable"].Value<string>());

            return gruntData;
        }

        public static BossData GetBoss(string enemyId)
        {
            var enemyJson = JsonUtility.LoadJson(ENEMY_JSON_PATH)[enemyId];

            BossData bossData = new BossData();

            foreach (var gruntId in enemyJson["phases"])
            {
                bossData.Phases.Add(GetGrunt(gruntId.Value<string>()));
            }

            return bossData;
        }

        public static IEnemyData GetEnemy(string enemyId)
        {
            var enemyJson = JsonUtility.LoadJson(ENEMY_JSON_PATH)[enemyId];

            switch(enemyJson["type"].Value<string>())
            {
                case "grunt":
                    return GetGrunt(enemyId);
                case "boss":
                    return GetBoss(enemyId);
            }
            return null;
        }
        
    }
}
