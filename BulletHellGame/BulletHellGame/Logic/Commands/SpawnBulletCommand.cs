using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Logic.Entities;

namespace BulletHellGame.Logic.Commands
{
    public class SpawnBulletCommand : ICommand
    {
        private BulletData _bulletData;
        private Vector2 _position;
        private Vector2 _direction;
        private int _layer;
        private Entity _owner;

        public SpawnBulletCommand(BulletData bulletData, Vector2 position, int layer, Vector2 direction, Entity owner)
        {
            _bulletData = bulletData;
            _position = position;
            _layer = layer;
            _direction = direction;
            _owner = owner;
        }

        public void Execute()
        {
            //entityManager.SpawnBullet(_bulletData, _position, _layer, _direction, _owner);
        }
    }
}
