using BulletHellGame.Logic.Entities;

namespace BulletHellGame.Logic.Builders
{
    /// <summary>
    /// A class for building entities. Each entity builder has a specific id of entity data class
    /// that it uses to build an entity.
    /// </summary>
    /// <typeparam name="T"> A data class used to build an entity. </typeparam>
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

        // Part Building (how to build each component):
        public virtual void BuildHealth() { }
        public virtual void BuildSprite() { }
        public virtual void BuildPosition() { }
        public virtual void BuildVelocity() { }
        public virtual void BuildAcceleration() { }
        public virtual void BuildSpeed() { }
        public virtual void BuildMovementPattern() { }
        public virtual void BuildShooting() { }
        public virtual void BuildLoot() { }
        public virtual void BuildHitbox() { }
        public virtual void BuildPhases() { } // Only for bosses
        public virtual void BuildOwner() { }  // Only for bullets, options, etc.
        public virtual void BuildDamage() { }
        public virtual void BuildHoming() { }
        public virtual void BuildAttractable() { } // Only for collectibles
        public virtual void BuildPickupEffect() { } // Only for collectibles
        public virtual void BuildCollector() { }  // Only for player/collector entities
        public virtual void BuildMagnet() { } // Only for player
        public virtual void BuildPlayerStats() { } // Only for player
        public virtual void BuildController() { }
        public virtual void BuildInvincibility() { }
        public virtual void BuildPowerLevel() { }
        public virtual void BuildIndicator() { }
        public virtual void BuildDespawn() { }
        public virtual void BuildCollisionStrategy() { }
        public virtual void BuildBombing() { }
        public virtual void BuildPush() { }

        public Entity GetResult()
        {
            return _entity;
        }
    }
}
