using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Factories
{
    public class PlayerFactory
    {
        public PlayerFactory() { }
        public Entity CreatePlayer(CharacterData playerData)
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(playerData.SpriteName);
            Entity player = new Entity();
            player.AddComponent(new HealthComponent(playerData.Health));
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2);
            player.AddComponent(spriteComponent); player.AddComponent(new SpeedComponent(playerData.MovementSpeed, playerData.FocusedSpeed));
            player.AddComponent(new PlayerInputComponent());
            player.AddComponent(new PositionComponent());
            player.AddComponent(new VelocityComponent());
            player.AddComponent(new CollectorComponent());
            player.AddComponent(new MagnetComponent());
            player.AddComponent(new PlayerStatsComponent(playerData.InitialLives, playerData.InitialBombs));
            player.AddComponent(new InvincibilityComponent());

            ShootingLevelComponent plc = new ShootingLevelComponent();
            foreach (var powerLevel in playerData.PowerLevels)
            {
                plc.PowerLevels[powerLevel.Key] = powerLevel.Value.FrontWeapon;
            }
            player.AddComponent(plc);

            BulletData bd = playerData.PowerLevels[0].FrontWeapon.BulletData;
            ShootingComponent sc = new ShootingComponent(bd);
            sc.FireRate = playerData.PowerLevels[0].FrontWeapon.FireRate;
            sc.FireDirections = playerData.PowerLevels[0].FrontWeapon.FireDirections;
            player.AddComponent(sc);

            // Set the hitbox:
            HitboxComponent hc = new HitboxComponent(player, 2);
            hc.Hitbox = new Vector2(4, 4);
            player.AddComponent(hc);
            return player;
        }
    }
}
