﻿namespace BulletHellGame.Logic.Builders
{
    public class EntityDirector
    {
        public void ConstructEntity<T>(EntityBuilder<T> builder) where T : class
        {
            builder.Reset();
            builder.BuildSprite();
            builder.BuildHealth();
            builder.BuildPosition();
            builder.BuildVelocity();
            builder.BuildAcceleration();
            builder.BuildSpeed();
            builder.BuildHitbox();
            builder.BuildMovementPattern();
            builder.BuildShooting();
            builder.BuildLoot();
            builder.BuildPhases();
            builder.BuildOwner();
            builder.BuildDamage();
            builder.BuildHoming();
            builder.BuildAttractable();
            builder.BuildPickupEffect();
            builder.BuildCollector();
            builder.BuildMagnet();
            builder.BuildPlayerStats();
            builder.BuildInvincibility();
            builder.BuildPowerLevel();
            builder.BuildInput();
            builder.BuildIndicator();
            builder.BuildDespawn();
        }
    }
}
