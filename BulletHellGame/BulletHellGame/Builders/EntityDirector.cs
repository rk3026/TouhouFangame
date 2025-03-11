namespace BulletHellGame.Builders
{
    public class EntityDirector
    {
        public void ConstructEntity<T>(EntityBuilder<T> builder) where T : class
        {
            builder.Reset();
            builder.SetHealth();
            builder.SetSprite();
            builder.SetPosition();
            builder.SetVelocity();
            builder.SetSpeed();
            builder.SetMovementPattern();
            builder.SetShooting();
            builder.SetLoot();
            builder.SetHitbox();
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
        }
    }
}
