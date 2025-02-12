using System;
using System.Collections.Generic;
using System.Linq;
namespace BulletHellGame.Data.DataTransferObjects
{
    public class WeaponData
    {
        public BulletData BulletData { get; set; }
        public float FireRate { get; set; }
        public float TimeSinceLastShot { get; set; }

        public List<Vector2> FireDirections { get; set; }
    }
}
