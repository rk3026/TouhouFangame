using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Factories
{
    public class OptionFactory
    {
        public OptionFactory() { }

        public Entity CreateOption(OptionData optionData)
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(optionData.SpriteName);

            Entity weapon = new Entity();
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2);
            spriteComponent.RotationSpeed = 0.1f;
            weapon.AddComponent(spriteComponent);
            weapon.AddComponent(new PositionComponent());
            weapon.AddComponent(new VelocityComponent());

            ShootingComponent shootingComponent = new ShootingComponent(optionData.Weapons);
            weapon.AddComponent(shootingComponent);
            return weapon;
        }
    }
}
