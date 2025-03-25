using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Systems.LogicSystems
{
    public class CollisionSystem : ILogicSystem
    {
        private const int CELL_WIDTH = 100;
        private Dictionary<(int, int), List<HitboxComponent>> hitboxGrid;

        public CollisionSystem()
        {
            hitboxGrid = new Dictionary<(int, int), List<HitboxComponent>>();
        }

        private void ApplyCollision(EntityManager entityManager, Entity owner, Entity other)
        {
            if (owner.TryGetComponent<CollisionStrategyComponent>(out var csc))
            {
                csc.CollisionStrategy.ApplyCollision(entityManager, owner, other);
            }
        }


        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            hitboxGrid.Clear();


            entityManager.OperateOnEntities((entity) => {

                entity.TryGetComponent<HitboxComponent>(out var hc);
                entity.TryGetComponent<PositionComponent>(out var pc);
                // Insert the hitbox into the appropriate grid cells
                InsertIntoGrid(hc, pc);

            }, typeof(HitboxComponent), typeof(PositionComponent));

            DoCollisionLogic(entityManager);
        }

        private void InsertIntoGrid(HitboxComponent hc, PositionComponent pc)
        {
            int minX = (int)(pc.Position.X - hc.Hitbox.X / 2);
            int minY = (int)(pc.Position.Y - hc.Hitbox.Y / 2);
            int maxX = (int)(pc.Position.X + hc.Hitbox.X / 2);
            int maxY = (int)(pc.Position.Y + hc.Hitbox.Y / 2);

            int lowCol = minX / CELL_WIDTH;
            int highCol = maxX / CELL_WIDTH;
            int lowRow = minY / CELL_WIDTH;
            int highRow = maxY / CELL_WIDTH;

            for (int c = lowCol; c <= highCol; ++c)
            {
                for (int r = lowRow; r <= highRow; ++r)
                {
                    var gridKey = (c, r);
                    if (!hitboxGrid.ContainsKey(gridKey))
                    {
                        hitboxGrid[gridKey] = new List<HitboxComponent>();
                    }
                    hitboxGrid[gridKey].Add(hc);
                }
            }
        }

        private void DoCollisionLogic(EntityManager entityManager)
        {
            // Check collisions for each grid cell
            foreach (var cell in hitboxGrid)
            {
                var hitboxes = cell.Value;
                for (int i = 0; i < hitboxes.Count; i++)
                {
                    for (int j = 0; j < hitboxes.Count; j++)
                    {
                        if (i == j) continue; // Don't check the same hitbox
                        var hitboxA = hitboxes[i];
                        var hitboxB = hitboxes[j];
                        if (hitboxA.Layer == hitboxB.Layer) continue; // Don't collide if on same layer

                        Vector2 hitAPos = hitboxA.Owner.GetComponent<PositionComponent>().Position;
                        Vector2 hitBPos = hitboxB.Owner.GetComponent<PositionComponent>().Position;
                        //Rectangle rectA = new Rectangle((int)hitAPos.X, (int)hitAPos.Y, (int)hitboxA.Hitbox.X, (int)hitboxA.Hitbox.Y);
                        //Rectangle rectB = new Rectangle((int)hitBPos.X, (int)hitBPos.Y, (int)hitboxB.Hitbox.X, (int)hitboxB.Hitbox.Y);
                        Rectangle rectA = new Rectangle((int)(hitAPos.X - hitboxA.Hitbox.X / 2), (int)(hitAPos.Y - hitboxA.Hitbox.Y / 2), (int)hitboxA.Hitbox.X, (int)hitboxA.Hitbox.Y);
                        Rectangle rectB = new Rectangle((int)(hitBPos.X - hitboxB.Hitbox.X / 2), (int)(hitBPos.Y - hitboxB.Hitbox.Y / 2), (int)hitboxB.Hitbox.X, (int)hitboxB.Hitbox.Y);

                        if (rectA.Intersects(rectB))
                        {
                            ApplyCollision(entityManager, hitboxA.Owner, hitboxB.Owner);
                        }
                    }
                }
            }
        }
    }
}
