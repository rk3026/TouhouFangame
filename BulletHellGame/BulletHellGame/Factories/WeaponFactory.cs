using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;
using System.Linq;

namespace BulletHellGame.Factories
{
    public class WeaponFactory
    {
        public WeaponFactory() { }

        public Entity CreateWeapon(WeaponData weaponData)
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(weaponData.SpriteName);
            spriteData.Origin = new Vector2(spriteData.Animations.First().Value.First().Width / 2, spriteData.Animations.First().Value.First().Height / 2);
            Entity weapon = new Entity();
            SpriteComponent sc = new SpriteComponent(spriteData);
            sc.RotationSpeed = 0.1f;
            weapon.AddComponent(sc);
            weapon.AddComponent(new PositionComponent());
            weapon.AddComponent(new VelocityComponent());

            ShootingComponent shootingComponent = new ShootingComponent(weaponData.bulletData);
            shootingComponent.FireRate = weaponData.FireRate;
            shootingComponent.FireDirections = weaponData.FireDirections;
            weapon.AddComponent(shootingComponent);
            return weapon;
        }
    }
}
