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

            Entity weapon = new Entity();
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2);
            spriteComponent.RotationSpeed = 0.1f;
            weapon.AddComponent(spriteComponent);
            weapon.AddComponent(new PositionComponent());
            weapon.AddComponent(new VelocityComponent());

            ShootingComponent shootingComponent = new ShootingComponent(weaponData.BulletData);
            shootingComponent.FireRate = weaponData.FireRate;
            shootingComponent.FireDirections = weaponData.FireDirections;
            weapon.AddComponent(shootingComponent);
            return weapon;
        }
    }
}
