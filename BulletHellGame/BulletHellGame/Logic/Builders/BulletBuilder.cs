﻿using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;
using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Strategies.CollisionStrategies;

namespace BulletHellGame.Logic.Builders
{
    public class BulletBuilder : EntityBuilder<BulletData>
    {
        public BulletBuilder() : base() { }
        public BulletBuilder(BulletData data) : base(data) { }

        /// <summary>
        /// Takes an already constructed bullet and changes the data (in the components) to match the new bullet data
        /// </summary>
        /// <param name="bullet"> The bullet entity to modify. </param>
        /// <param name="bulletData"> The data to apply to the entity. </param>
        public void ApplyBulletData(Entity bullet, BulletData bulletData)
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(bulletData.SpriteName);
            bullet.RemoveComponent<SpriteComponent>();
            bullet.AddComponent(new SpriteComponent(spriteData));
            bullet.GetComponent<SpriteComponent>().SpriteData.Origin = new Vector2(bullet.GetComponent<SpriteComponent>().CurrentFrame.Width / 2, bullet.GetComponent<SpriteComponent>().CurrentFrame.Height / 2);
            bullet.GetComponent<SpriteComponent>().RotationSpeed = bulletData.RotationSpeed;
            bullet.GetComponent<SpriteComponent>().CurrentRotation = 0f;
            bullet.GetComponent<SpriteComponent>().Scale = Vector2.One;
            bullet.GetComponent<HitboxComponent>().Hitbox = new Vector2(bullet.GetComponent<SpriteComponent>().CurrentFrame.Width, bullet.GetComponent<SpriteComponent>().CurrentFrame.Height);
            bullet.GetComponent<DamageComponent>().BaseDamage = bulletData.Damage;
            bullet.GetComponent<AccelerationComponent>().Acceleration = bulletData.Acceleration;
            bullet.GetComponent<BounceComponent>().CanBounce = bulletData.BulletType == BulletType.Bouncy;
            if (bulletData.BulletType != BulletType.Homing)
            {
                if (bullet.HasComponent<HomingComponent>())
                {
                    bullet.RemoveComponent<HomingComponent>();
                }
            }
            else
            {
                if (!bullet.HasComponent<HomingComponent>())
                {
                    bullet.AddComponent(new HomingComponent());
                }
                else
                {
                    bullet.TryGetComponent<HomingComponent>(out HomingComponent homingComponent);
                    homingComponent.Reset();
                }
            }

            ResetPushLogicAndSplitting(bullet);

            bullet.GetComponent<BounceComponent>().CanBounce = bulletData.BulletType == BulletType.Bouncy;
        }

        private void ResetPushLogicAndSplitting(Entity bullet)
        {
            bullet.RemoveComponent<PushableComponent>();
            bullet.RemoveComponent<PusherComponent>();
            bullet.RemoveComponent<BulletContainerComponent>();

            Vector2 size = bullet.GetComponent<HitboxComponent>().Hitbox;
            bool isLargeBullet = size.X > 50f;

            if (isLargeBullet)
            {
                bullet.AddComponent(new PusherComponent());

                var container = new BulletContainerComponent();
                container.BulletsToSpawn.Add(new BulletData
                {
                    BulletType = BulletType.Bouncy,
                    SpriteName = "SingleCircle.Red",
                    Damage = _entityData.Damage / 2,
                    RotationSpeed = _entityData.RotationSpeed,
                    Acceleration = _entityData.Acceleration,
                }, 10);

                bullet.AddComponent(container);
            }
            else
            {
                bullet.AddComponent(new PushableComponent());
            }
        }


        public override void BuildSprite()
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(_entityData.SpriteName);
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2);
            spriteComponent.RotationSpeed = _entityData.RotationSpeed;
            _entity.AddComponent(spriteComponent);
        }


        public override void BuildPosition()
        {
            _entity.AddComponent(new PositionComponent());
        }

        public override void BuildVelocity()
        {
            _entity.AddComponent(new VelocityComponent());
        }

        public override void BuildAcceleration()
        {
            _entity.AddComponent(new AccelerationComponent(_entityData.Acceleration));
        }

        public override void BuildDamage()
        {
            _entity.AddComponent(new DamageComponent(_entityData.Damage));
        }

        public override void BuildHoming()
        {
            if (_entityData.BulletType == BulletType.Homing)
            {
                _entity.AddComponent(new HomingComponent());
            }
        }

        public override void BuildHitbox()
        {
            HitboxComponent hc = new HitboxComponent(_entity, HitboxLayer.EnemiesAndEnemyBullets);
            SpriteComponent sc = _entity.GetComponent<SpriteComponent>();
            hc.Hitbox = new Vector2(sc.CurrentFrame.Width, sc.CurrentFrame.Width);
            _entity.AddComponent(hc); // Layer 1 = enemies and their bullets
        }

        public override void BuildDespawn()
        {
            _entity.AddComponent(new DespawnComponent());
        }

        public override void BuildCollisionStrategy()
        {
            _entity.AddComponent(new CollisionStrategyComponent(new BulletCollisionStrategy()));
        }

        public override void BuildPush()
        {
            float largeBulletMinimumWidth = 50f;
            //Vector2 bulletSize = _entity.GetComponent<SpriteComponent>().CurrentFrame.Size.ToVector2();
            Vector2 bulletSize = _entity.GetComponent<HitboxComponent>().Hitbox;
            if (bulletSize.X > largeBulletMinimumWidth)
            {
                _entity.AddComponent(new PusherComponent());
            }
            else
            {
                _entity.AddComponent(new PushableComponent());
            }

        }

        public override void BuildBulletContainer()
        {
            float largeBulletMinimumWidth = 50f;
            Vector2 bulletSize = _entity.GetComponent<HitboxComponent>().Hitbox;

            if (bulletSize.X > largeBulletMinimumWidth)
            {
                BulletContainerComponent bulletContainerComponent = new BulletContainerComponent();
                BulletData smallBulletData = new BulletData
                {
                    BulletType = BulletType.Bouncy,
                    SpriteName = "SingleCircle.Red",
                    Damage = _entityData.Damage / 2,
                    RotationSpeed = _entityData.RotationSpeed,
                    Acceleration = _entityData.Acceleration,
                };
                bulletContainerComponent.BulletsToSpawn.Add(smallBulletData, 10);
                _entity.AddComponent(bulletContainerComponent);
            }
        }

        public override void BuildBounce()
        {
            _entity.AddComponent(new BounceComponent(_entityData.BulletType == BulletType.Bouncy));
        }

    }
}
