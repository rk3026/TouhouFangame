﻿using BulletHellGame.Entities;

namespace BulletHellGame.Data.DataTransferObjects
{
    public class EnemyData
    {
        public string SpriteName { get; set; }
        public Vector2 SpawnPosition { get; set; }
        public Vector2 StartPosition { get; set; }
        public Vector2 ExitPosition { get; set; }
        public int Health { get; set; }
        public string MovementPattern { get; set; }
        public ShootingPatternData BulletPattern { get; set; }
        public List<WeaponData> Weapons { get; set; }
        public List<CollectibleData> Loot {  get; set; } = new List<CollectibleData>();
    }
}
