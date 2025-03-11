using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Managers;

namespace BulletHellGame.Builders
{
    public class PlayerBuilder : EntityBuilder<CharacterData>
    {
        public PlayerBuilder(CharacterData data) : base(data) { }

        public override void SetHealth()
        {
            _entity.AddComponent(new HealthComponent(_entityData.Health));
        }

        public override void SetSpeed()
        {
            _entity.AddComponent(new SpeedComponent(_entityData.MovementSpeed, _entityData.FocusedSpeed));
        }

        public override void SetPosition()
        {
            _entity.AddComponent(new PositionComponent());
        }

        public override void SetVelocity()
        {
            _entity.AddComponent(new VelocityComponent());
        }

        public override void SetSprite()
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(_entityData.SpriteName);
            SpriteComponent spriteComponent = new SpriteComponent(spriteData)
            {
                SpriteData = { Origin = new Vector2(spriteData.Texture.Width / 2, spriteData.Texture.Height / 2) }
            };
            _entity.AddComponent(spriteComponent);
        }

        public override void SetCollector()
        {
            _entity.AddComponent(new CollectorComponent());
        }

        public override void SetMagnet()
        {
            _entity.AddComponent(new MagnetComponent());
        }

        public override void SetStats()
        {
            _entity.AddComponent(new PlayerStatsComponent(_entityData.InitialLives, _entityData.InitialBombs));
        }

        public override void SetPlayerInput()
        {
            _entity.AddComponent(new PlayerInputComponent());
        }

        public override void SetInvincibility()
        {
            _entity.AddComponent(new InvincibilityComponent());
        }

        public override void SetPowerLevel()
        {
            PowerLevelComponent plc = new PowerLevelComponent();

            foreach (var powerLevel in _entityData.UnfocusedPowerLevels)
            {
                plc.UnfocusedPowerLevels.Add(powerLevel.Key, powerLevel.Value);
                for (int i = 0; i < powerLevel.Value.MainWeapons.Count; ++i)
                {
                    plc.UnfocusedPowerLevels[powerLevel.Key].MainWeapons[i] = powerLevel.Value.MainWeapons[i];
                }
            }

            foreach (var powerLevel in _entityData.FocusedPowerLevels)
            {
                plc.FocusedPowerLevels.Add(powerLevel.Key, powerLevel.Value);
                for (int i = 0; i < powerLevel.Value.MainWeapons.Count; ++i)
                {
                    plc.FocusedPowerLevels[powerLevel.Key].MainWeapons[i] = powerLevel.Value.MainWeapons[i];
                }
            }

            _entity.AddComponent(plc);
        }

        public override void SetShooting()
        {
            if (_entityData.UnfocusedPowerLevels.ContainsKey(0))
            {
                List<WeaponData> weapons = new List<WeaponData>(_entityData.UnfocusedPowerLevels[0].MainWeapons);
                _entity.AddComponent(new ShootingComponent(weapons));
            }
        }

        public override void SetHitbox()
        {
            HitboxComponent hc = new HitboxComponent(_entity, 2)
            {
                Hitbox = new Vector2(4, 4)
            };
            _entity.AddComponent(hc);
        }
    }
}
