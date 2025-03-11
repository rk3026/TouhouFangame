namespace BulletHellGame.Builders
{
    public class EntityDirector
    {
        public void ConstructEntity<T>(EntityBuilder<T> builder) where T : class
        {
            builder.Reset();
            builder.SetSprite();
            builder.SetHealth();
            builder.SetPosition();
            builder.SetVelocity();
            builder.SetSpeed();
            builder.SetHitbox();
            builder.SetMovementPattern();
            builder.SetShooting();
            builder.SetLoot();
            builder.SetPhases();
            builder.SetOwner();
            builder.SetDamage();
            builder.SetHoming();
            builder.SetAttractable();
            builder.SetPickupEffect();
            builder.SetCollector();
            builder.SetMagnet();
            builder.SetStats();
            builder.SetInvincibility();
            builder.SetPowerLevel();
            builder.SetPlayerInput();
        }
    }
}
