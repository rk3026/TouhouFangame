using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Factories
{
    public class WeaponFactory
    {
        public WeaponFactory() { }

        public Entity CreateWeapon(WeaponData weaponData)
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(weaponData.SpriteName);
            Entity weapon = new Entity();
            weapon.AddComponent(new SpriteComponent(spriteData));
            //weapon.AddComponent(new
            return weapon;
        }
    }
}
