using BulletHellGame.Entities;

namespace BulletHellGame.Builders
{
    public abstract class EntityBuilder<T> where T : class
    {
        protected Entity _entity;
        protected T _entityData;

        public EntityBuilder()
        {
            _entity = new Entity();
        }

        public EntityBuilder(T entityData)
        {
            _entity = new Entity();
            _entityData = entityData;
        }

        public void Reset()
        {
            _entity = new Entity();
        }

        public void SetEntityData(T entityData)
        {
            _entityData = entityData;
        }

        public virtual void SetHealth() { }
        public virtual void SetSprite() { }
        public virtual void SetPosition() { }
        public virtual void SetVelocity() { }
        public virtual void SetSpeed() { }
        public virtual void SetMovementPattern() { }
        public virtual void SetShooting() { }
        public virtual void SetLoot() { }
        public virtual void SetHitbox() { }
        public virtual void SetPhases() { } // Only for bosses
        public virtual void SetOwner() { }  // Only for bullets, options, etc.
        public virtual void SetDamage() { }
        public virtual void SetHoming() { }
        public virtual void SetAttractable() { } // Only for collectibles
        public virtual void SetPickupEffect() { } // Only for collectibles
        public virtual void SetCollector() { }  // Only for player/collector entities
        public virtual void SetMagnet() { } // Only for player
        public virtual void SetStats() { } // Only for player
        public virtual void SetPlayerInput() { } // Only for player
        public virtual void SetInvincibility() { }
        public virtual void SetPowerLevel() { }

        public Entity GetResult()
        {
            return _entity;
        }
    }
}
