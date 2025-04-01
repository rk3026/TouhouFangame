﻿using BulletHellGame.Logic.Entities;

namespace BulletHellGame.DataAccess.DataTransferObjects
{
    public class SpawnerData
    {
        public List<WeaponData> Weapons { get; set; } = new List<WeaponData>();
        public Entity Owner { get; set; }
        public string SpriteName { get; set; }

        public SpawnerData(List<WeaponData> weapons, Entity owner, string spriteName)
        {
            Weapons = weapons;
            Owner = owner;
            SpriteName = spriteName;
        }
    }
}
