using BulletHellGame.Entities;
using BulletHellGame.Data.DataTransferObjects;

namespace BulletHellGame.Commands
{
    public class SpawnBulletCommand : ICommand
    {
        private BulletData _bulletData;
        private Vector2 _position;
        private int _layer;
        private Vector2 _direction;
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
