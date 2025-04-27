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

namespace BulletHellGame.DataAccess.DataLoaders
{
    public static class BulletDataLoader
    {
        const string BULLET_JSON_PATH = "./Data/Levels/bullets.json";

        public static BulletData GetBullet(string bulletId)
        {
            var bulletJson = JsonUtility.LoadJson(BULLET_JSON_PATH)[bulletId];

            return new BulletData
            {
                SpriteName = bulletJson["sprite"].Value<string>(),
                Damage = bulletJson["damage"].Value<int>(),
                BulletType = Enum.Parse<BulletType>(bulletJson["type"].Value<string>()),
                RotationSpeed = bulletJson["damage"].Value<float>()
            };
        }
    }
}
