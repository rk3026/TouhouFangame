using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHellGame.Entities.Bullets
{
    internal class HomingBullet : Bullet
    {
        public HomingBullet(BulletType type, Texture2D texture, Vector2 position, Vector2 velocity) : base(BulletType.Homing, texture, position, velocity)
        {
        }
    }
}
