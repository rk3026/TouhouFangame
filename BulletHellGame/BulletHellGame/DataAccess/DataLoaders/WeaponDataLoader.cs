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
    public static class WeaponDataLoader
    {
        const string WEAPON_JSON_PATH = "./Data/Levels/weapons.json";

        public static WeaponData GetWeapon(string weaponId)
        {
            var weaponJson = JsonUtility.LoadJson(WEAPON_JSON_PATH)[weaponId];

            WeaponData weapon = new WeaponData();

            weapon.BulletData = BulletDataLoader.GetBullet(weaponJson["bulletId"].Value<string>());
            weapon.FireRate = weaponJson["fireRate"].Value<float>();
            weapon.MovementPattern = weaponJson["movementPattern"] is null ? string.Empty : weaponJson["movementPattern"].Value<string>();
            float startDelay = weaponJson["startDelay"] is null ? 0.0f : weaponJson["startDelay"].Value<float>();

            weapon.TimeSinceLastShot = (weapon.FireRate - startDelay);

            foreach (var fireDirection in weaponJson["fireDirections"])
            {
                float x = fireDirection["x"].Value<float>();
                float y = fireDirection["y"].Value<float>();
                weapon.FireDirections.Add(new Vector2(x, y));
            }

            return weapon;
        }
    }
}
