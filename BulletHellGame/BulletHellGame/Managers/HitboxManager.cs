using System.Linq;
using BulletHellGame.Components;

namespace BulletHellGame.Managers
{
    /// <summary>
    /// Manages collisions between hitboxes
    /// </summary>
    public class HitboxManager
    {
        // side length of the grid cells
        private const int CELL_WIDTH = 100;

        // true if 
        private bool _changed;
        // collision check queue
        private Queue<HitboxComponent> _checkQueue;
        // hitbox grid insertion queue
        private Queue<HitboxComponent> _insertQueue;

        private static HitboxManager _instance;
        public static HitboxManager Instance => _instance ??= new HitboxManager();

        // enqueue collision check
        public void EnqueueCheck(HitboxComponent hitbox)
        {
            _checkQueue.Enqueue(hitbox);
        }

        public void EnqueueInsertion(HitboxComponent hitbox)
        {
            _insertQueue.Enqueue(hitbox);
        }

        // execute collisions on queued hitbox collision checks
        public void Update(GameTime gameTime)
        {
            // do nothing if no hitboxes were inserted
            if (_insertQueue.Count() == 0)
                return;

            int minX, minY;
            minX = minY = int.MaxValue;
            int maxX, maxY;
            maxX = maxY = int.MinValue;

            // determine boundaries and density of grid
            foreach(var hitbox in _insertQueue)
            {
                if (hitbox.Hitbox.Left < minX)
                    minX = hitbox.Hitbox.Left;
                if (hitbox.Hitbox.Top < minY)
                    minY = hitbox.Hitbox.Top;
                if (hitbox.Hitbox.Right > maxX)
                    maxX = hitbox.Hitbox.Right;
                if (hitbox.Hitbox.Bottom > maxY)
                    maxY = hitbox.Hitbox.Bottom;
            }

            // dimensions of grid
            int cols = (maxX - minX) / CELL_WIDTH + 1;
            int rows = (maxY - minY) / CELL_WIDTH + 1;

            // grid cells containing linked lists of hitboxes
            LinkedList<HitboxComponent>[,] hitboxGrid = new LinkedList<HitboxComponent>[cols,rows];

            // insert all queued hitboxes into grid
            HitboxComponent cHitbox;
            while(_insertQueue.TryDequeue(out cHitbox))
            {
                int lowCol = (cHitbox.Hitbox.Left - minX) / CELL_WIDTH;
                int highCol = (cHitbox.Hitbox.Right - minX) / CELL_WIDTH;
                int lowRow = (cHitbox.Hitbox.Top - minY) / CELL_WIDTH;
                int highRow = (cHitbox.Hitbox.Bottom - minY) / CELL_WIDTH;

                // insert hitbox into all grid cells that it occupies
                for(int c = lowCol; c <= highCol; ++c)
                {
                    for(int r = lowRow; r <= highRow; ++r)
                    {
                        if (hitboxGrid[c, r] == null)
                        {
                            hitboxGrid[c, r] = new LinkedList<HitboxComponent>();
                        }

                        hitboxGrid[c, r].AddLast(cHitbox);
                    }
                }
            }

            // check all queued collision checks
            while (_checkQueue.TryDequeue(out cHitbox))
            {
                int lowCol = (cHitbox.Hitbox.Left - minX) / CELL_WIDTH;
                int highCol = (cHitbox.Hitbox.Right - minX) / CELL_WIDTH;
                int lowRow = (cHitbox.Hitbox.Top - minY) / CELL_WIDTH;
                int highRow = (cHitbox.Hitbox.Bottom - minY) / CELL_WIDTH;

                // check all grid cells hitbox occupies
                for (int c = lowCol; c <= highCol; ++c)
                {
                    for (int r = lowRow; r <= highRow; ++r)
                    {
                        if (hitboxGrid[c, r] != null)
                        {
                            foreach(var hitbox in hitboxGrid[c, r])
                            {
                                if(cHitbox.Hitbox.Intersects(hitbox.Hitbox))
                                    cHitbox.OnCollision(hitbox.GetOwner());
                            }
                        }
                    }
                }
            }
        }

        private HitboxManager()
        {
            _checkQueue = new Queue<HitboxComponent> ();
            _insertQueue = new Queue<HitboxComponent> ();
        }
    }
}
