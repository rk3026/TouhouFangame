﻿using BulletHellGame.Logic.Entities;

namespace BulletHellGame.Data.DataTransferObjects
{
    public class CollectibleData
    {
        public string SpriteName { get; set; }
        public Dictionary<CollectibleType, int> Effects { get; set; } = new();

        public CollectibleData() { }
    }
}
