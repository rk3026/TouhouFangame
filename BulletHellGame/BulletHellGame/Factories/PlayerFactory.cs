using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Factories
{
    public class PlayerFactory
    {
        public PlayerFactory() { }
        public Entity CreatePlayer(PlayerData playerData)
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(playerData.SpriteName);
            Entity player = new Entity();
            player.AddComponent(new HealthComponent(playerData.Health));
            player.AddComponent(new SpriteComponent(spriteData));
            player.AddComponent(new HitboxComponent(player, 2)); // Layer 2 = players and their bullets
            player.AddComponent(new SpeedComponent(playerData.MovementSpeed, playerData.FocusedSpeed));
            player.AddComponent(new PlayerInputComponent());
            player.AddComponent(new MovementComponent());

            // Setting up and adding a weapon
            BulletData bd = new BulletData();
            bd.SpriteData = TextureManager.Instance.GetSpriteData("Reimu.OrangeBullet");
            bd.Damage = 25;
            bd.BulletType = BulletType.Homing;
            WeaponComponent wc = new WeaponComponent(bd);
            wc.FireDirections.Add(new Vector2(0, -5));
            player.AddComponent(wc);

            return player;
        }
    }
}
