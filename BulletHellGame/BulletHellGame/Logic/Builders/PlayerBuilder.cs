using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Controllers;
using BulletHellGame.Logic.Managers;
using System.Linq;
using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Strategies.CollisionStrategies;

namespace BulletHellGame.Logic.Builders
{
    public class PlayerBuilder : EntityBuilder<CharacterData>
    {
        public PlayerBuilder() : base() { }
        public PlayerBuilder(CharacterData data) : base(data) { }

        public override void BuildHealth()
        {
            _entity.AddComponent(new HealthComponent(_entityData.Health));
        }

        public override void BuildSpeed()
        {
            _entity.AddComponent(new SpeedComponent(_entityData.MovementSpeed, _entityData.FocusedSpeed));
        }

        public override void BuildPosition()
        {
            _entity.AddComponent(new PositionComponent());
        }

        public override void BuildVelocity()
        {
            _entity.AddComponent(new VelocityComponent());
        }

        public override void BuildSprite()
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(_entityData.SpriteName);
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2);
            _entity.AddComponent(spriteComponent);
        }

        public override void BuildCollector()
        {
            _entity.AddComponent(new CollectorComponent());
        }

        public override void BuildMagnet()
        {
            _entity.AddComponent(new MagnetComponent());
        }

        public override void BuildPlayerStats()
        {
            _entity.AddComponent(new PlayerStatsComponent(_entityData.InitialLives, _entityData.InitialBombs));
        }

        public override void BuildInput()
        {
            _entity.AddComponent(new ControllerComponent(new PlayerController()));
        }

        public override void BuildInvincibility()
        {
            _entity.AddComponent(new InvincibilityComponent());
        }

        public override void BuildPowerLevel()
        {
            PowerLevelComponent plc = new PowerLevelComponent();
            foreach (var powerLevel in _entityData.ShotTypes.First().UnfocusedShot.PowerLevels)
            {
                plc.UnfocusedPowerLevels.Add(powerLevel.Key, powerLevel.Value);
                for (int i = 0; i < powerLevel.Value.MainWeapons.Count; ++i)
                {
                    plc.UnfocusedPowerLevels[powerLevel.Key].MainWeapons[i] = powerLevel.Value.MainWeapons[i];
                }
            }
            foreach (var powerLevel in _entityData.ShotTypes.First().FocusedShot.PowerLevels)
            {
                plc.FocusedPowerLevels.Add(powerLevel.Key, powerLevel.Value);
                for (int i = 0; i < powerLevel.Value.MainWeapons.Count; ++i)
                {
                    plc.FocusedPowerLevels[powerLevel.Key].MainWeapons[i] = powerLevel.Value.MainWeapons[i];
                }
            }
            _entity.AddComponent(plc);
        }

        public override void BuildShooting()
        {
            if (_entityData.ShotTypes.First().UnfocusedShot.PowerLevels.ContainsKey(0))
            {
                List<WeaponData> weapons = new List<WeaponData>();
                foreach (var weapon in _entityData.ShotTypes.First().UnfocusedShot.PowerLevels[0].MainWeapons)
                {
                    weapons.Add(weapon);
                }
                ShootingComponent sc = new ShootingComponent(weapons);
                _entity.AddComponent(sc);
            }
        }

        public override void BuildHitbox()
        {
            HitboxComponent hc = new HitboxComponent(_entity, 2);
            hc.Hitbox = new Vector2(4, 4);
            _entity.AddComponent(hc);
        }

        public override void BuildCollisionStrategy()
        {
            _entity.AddComponent(new CollisionStrategyComponent(new PlayerCollisionStrategy()));
        }

        public override void BuildBombing()
        {
            _entity.AddComponent(new BombingComponent(_entityData.InitialBombs, 3f));
        }
    }
}
