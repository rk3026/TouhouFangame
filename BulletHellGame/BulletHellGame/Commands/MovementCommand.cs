﻿using BulletHellGame.Components;
using BulletHellGame.Entities;

namespace BulletHellGame.Commands
{
    public class MovementCommand : ICommand
    {
        private Entity _entity;
        private Vector2 _direction;

        public MovementCommand(Entity entity, Vector2 direction)
        {
            _entity = entity;
            _direction = direction;
        }

        public void Execute()
        {
            if (_entity.TryGetComponent<VelocityComponent>(out var velocityComponent))
            {
                velocityComponent.Velocity = _direction;
            }
        }
    }

}
