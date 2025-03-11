﻿using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Factories
{
    public class PlayerFactory
    {
        public PlayerFactory() { }

        public Entity CreatePlayer(CharacterData characterData)
        {
            Entity player = new Entity();

            player.AddComponent(new HealthComponent(characterData.Health));
            player.AddComponent(new SpeedComponent(characterData.MovementSpeed, characterData.FocusedSpeed));
            player.AddComponent(new PlayerInputComponent());
            player.AddComponent(new PositionComponent());
            player.AddComponent(new VelocityComponent());
            player.AddComponent(new CollectorComponent());
            player.AddComponent(new MagnetComponent());
            player.AddComponent(new PlayerStatsComponent(characterData.InitialLives, characterData.InitialBombs));
            player.AddComponent(new InvincibilityComponent());

            // Adding sprite
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(characterData.SpriteName);
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2);
            player.AddComponent(spriteComponent);

            // Store all power levels
            PowerLevelComponent plc = new PowerLevelComponent();
            foreach (var powerLevel in characterData.UnfocusedPowerLevels)
            {
                plc.UnfocusedPowerLevels.Add(powerLevel.Key, powerLevel.Value);
                for (int i = 0; i < powerLevel.Value.MainWeapons.Count; ++i)
                {
                    plc.UnfocusedPowerLevels[powerLevel.Key].MainWeapons[i] = powerLevel.Value.MainWeapons[i];
                }
            }
            foreach (var powerLevel in characterData.FocusedPowerLevels)
            {
                plc.FocusedPowerLevels.Add(powerLevel.Key, powerLevel.Value);
                for (int i = 0; i < powerLevel.Value.MainWeapons.Count; ++i)
                {
                    plc.FocusedPowerLevels[powerLevel.Key].MainWeapons[i] = powerLevel.Value.MainWeapons[i];
                }
            }
            player.AddComponent(plc);

            // Set up the shootingcomponent
            if (characterData.UnfocusedPowerLevels.ContainsKey(0))
            {
                List<WeaponData> weapons = new List<WeaponData>();
                foreach (var weapon in characterData.UnfocusedPowerLevels[0].MainWeapons)
                {
                    weapons.Add(weapon);
                }
                ShootingComponent sc = new ShootingComponent(weapons);
                player.AddComponent(sc);

            }

            // Set the hitbox
            HitboxComponent hc = new HitboxComponent(player, 2);
            hc.Hitbox = new Vector2(4, 4);
            player.AddComponent(hc);

            return player;
        }
    }
}
