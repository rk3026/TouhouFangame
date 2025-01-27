﻿using BulletHellGame.Entities;

namespace BulletHellGame.Data.DataTransferObjects
{
    public class BulletData
    {
        public SpriteData SpriteData { get; set; }
        public BulletType BulletType { get; set; }
        public int Damage { get; set; }
    }
}
