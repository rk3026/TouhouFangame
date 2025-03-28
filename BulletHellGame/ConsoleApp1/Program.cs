using BulletHell.ECS;
using System;

public partial class Program
{
    public static void Main()
    {
        // Create the world (game context)
        var world = World.Create();
        Console.WriteLine("World created.\n");

        // Create some initial entities (player, enemy, boss, etc.)
        CreateEntities(world, 23);

        // Query entities by components and display their details
        QueryEntitiesWithPosition(world);
        QueryEntitiesWithVelocity(world);
        QueryEntitiesWithHealth(world);

        // Update the player and enemy positions
        UpdateEntityPositions(world);

        // Test HealthComponent: simulate damage
        SimulateDamage(world);

        // Remove PositionComponent from the player and check updates
        RemovePositionComponentFromPlayer(world);

        // Query entities with both PositionComponent and VelocityComponent after removal
        QueryEntitiesWithPositionAndVelocity(world);

        // Add a new component (e.g., Shield) to the player
        AddShieldComponentToPlayer(world);

        // Query entities with PositionComponent, VelocityComponent, and ShieldComponent
        QueryEntitiesWithPositionVelocityAndShield(world);

        // Query entities with multiple combinations of components
        QueryEntitiesWithMultipleComponents(world);

        Console.WriteLine("\nEnd of program.");
    }

    private static void CreateEntities(World world, int numberOfEntities)
    {
        Console.WriteLine("Creating entities...");

        Random rand = new Random();
        for (int i = 0; i < numberOfEntities; i++)
        {
            // Randomly decide which components to assign
            var position = new PositionComponent { X = rand.Next(-50, 50), Y = rand.Next(-50, 50) };
            var velocity = new VelocityComponent { X = rand.Next(-5, 5), Y = rand.Next(-5, 5) };
            var health = new HealthComponent { Health = rand.Next(10, 100) };
            ShieldComponent? shield = rand.Next(0, 2) == 0 ? new ShieldComponent { Shield = rand.Next(10, 50) } : (ShieldComponent?)null;

            // Create the entity with some random components
            if (shield != null)
            {
                world.CreateEntity(position, velocity, health, shield);
                Console.WriteLine($"Created entity {i} with Position {position.X} {position.Y}, Velocity {velocity.X} {velocity.Y}, Health {health.Health}, and Shield {shield.Value}.");
            }
            else
            {
                world.CreateEntity(position, velocity, health);
                Console.WriteLine($"Created entity {i} with Position {position.X} {position.Y}, Velocity {velocity.X} {velocity.Y}, Health {health.Health}.");
            }
        }
        Console.WriteLine();
    }

    private static void QueryEntitiesWithPosition(World world)
    {
        Console.WriteLine("Querying all entities with PositionComponent...");
        var queryDescription = new QueryDescription().WithAll<PositionComponent>();
        var sortedEntities = world.Query(queryDescription).OrderBy(entity => entity.Item1); // Sort by entityId

        foreach (var (entityId, components) in sortedEntities)
        {
            var position = (PositionComponent)components[0];
            Console.WriteLine($"Entity {entityId} has PositionComponent with position: ({position.X}, {position.Y})");
        }
        Console.WriteLine();
    }

    private static void QueryEntitiesWithVelocity(World world)
    {
        Console.WriteLine("Querying all entities with VelocityComponent...");
        var queryDescription = new QueryDescription().WithAll<VelocityComponent>();
        var sortedEntities = world.Query(queryDescription).OrderBy(entity => entity.Item1); // Sort by entityId

        foreach (var (entityId, components) in sortedEntities)
        {
            var velocity = (VelocityComponent)components[0];
            Console.WriteLine($"Entity {entityId} has VelocityComponent with velocity: ({velocity.X}, {velocity.Y})");
        }
        Console.WriteLine();
    }

    private static void QueryEntitiesWithHealth(World world)
    {
        Console.WriteLine("Querying all entities with HealthComponent...");
        var queryDescription = new QueryDescription().WithAll<HealthComponent>();
        var sortedEntities = world.Query(queryDescription).OrderBy(entity => entity.Item1); // Sort by entityId

        foreach (var (entityId, components) in sortedEntities)
        {
            var health = (HealthComponent)components[0];
            Console.WriteLine($"Entity {entityId} has HealthComponent with health: {health.Health}");
        }
        Console.WriteLine();
    }

    private static void UpdateEntityPositions(World world)
    {
        Console.WriteLine("Updating positions...");
        var queryDescription = new QueryDescription().WithAll(typeof(PositionComponent), typeof(VelocityComponent));
        var sortedEntities = world.Query(queryDescription).OrderBy(entity => entity.Item1); // Sort by entityId

        foreach (var (entityId, components) in sortedEntities)
        {
            var position = (PositionComponent)components[0];
            var velocity = (VelocityComponent)components[1];

            // Simple movement logic: update position by velocity
            position.X += velocity.X;
            position.Y += velocity.Y;

            Console.WriteLine($"Entity {entityId} updated position to: ({position.X}, {position.Y})");
        }
        Console.WriteLine();
    }

    private static void SimulateDamage(World world)
    {
        Console.WriteLine("Simulating damage to entities...");
        var queryDescription = new QueryDescription().WithAll<HealthComponent>();
        var sortedEntities = world.Query(queryDescription).OrderBy(entity => entity.Item1); // Sort by entityId

        foreach (var (entityId, components) in sortedEntities)
        {
            var health = (HealthComponent)components[0];

            // Simulate taking damage: reduce health by 10
            health.Health -= 10;

            Console.WriteLine($"Entity {entityId} took 10 damage. New health: {health.Health}");
        }
        Console.WriteLine();
    }

    private static void RemovePositionComponentFromPlayer(World world)
    {
        Console.WriteLine("\nRemoving PositionComponent from player entity...");
        int playerId = 0;  // Assuming player ID is 0 (for example purposes)
        world.RemoveComponent(playerId, typeof(PositionComponent));

        Console.WriteLine("PositionComponent removed from player.\n");

        // Query entities again after removal
        QueryEntitiesWithPosition(world);
    }

    private static void QueryEntitiesWithPositionAndVelocity(World world)
    {
        Console.WriteLine("\nQuerying all entities with both PositionComponent and VelocityComponent...");
        var queryDescription = new QueryDescription().WithAll(typeof(PositionComponent), typeof(VelocityComponent));
        var sortedEntities = world.Query(queryDescription).OrderBy(entity => entity.Item1); // Sort by entityId

        foreach (var (entityId, components) in sortedEntities)
        {
            if (components.Length > 0)
            {
                var position = (PositionComponent)components[0];
                var velocity = (VelocityComponent)components[1];
                Console.WriteLine($"Entity {entityId} position: ({position.X}, {position.Y}), velocity: ({velocity.X}, {velocity.Y})");
            }
            else
            {
                Console.WriteLine($"Entity {entityId} does not have both PositionComponent and VelocityComponent.");
            }
        }
        Console.WriteLine();
    }

    private static void AddShieldComponentToPlayer(World world)
    {
        Console.WriteLine("\nAdding ShieldComponent to player entity...");
        int playerId = 0;  // Assuming player ID is 0 (for example purposes)
        world.AddComponent(playerId, new ShieldComponent { Shield = 50 });

        Console.WriteLine("ShieldComponent added to player.\n");
    }

    private static void QueryEntitiesWithPositionVelocityAndShield(World world)
    {
        Console.WriteLine("\nQuerying all entities with PositionComponent, VelocityComponent, and ShieldComponent...");
        var queryDescription = new QueryDescription().WithAll(typeof(PositionComponent), typeof(VelocityComponent), typeof(ShieldComponent));
        var sortedEntities = world.Query(queryDescription).OrderBy(entity => entity.Item1); // Sort by entityId

        foreach (var (entityId, components) in sortedEntities)
        {
            var position = (PositionComponent)components[0];
            var velocity = (VelocityComponent)components[1];
            var shield = (ShieldComponent)components[2];
            Console.WriteLine($"Entity {entityId} position: ({position.X}, {position.Y}), velocity: ({velocity.X}, {velocity.Y}), shield: {shield.Shield}");
        }
        Console.WriteLine();
    }

    private static void QueryEntitiesWithMultipleComponents(World world)
    {
        Console.WriteLine("\nQuerying all entities with different combinations of components...");
        var queryDescription = new QueryDescription()
            .WithAll<PositionComponent>()
            .WithAll<VelocityComponent>()
            .WithAll<HealthComponent>();

        var sortedEntities = world.Query(queryDescription).OrderBy(entity => entity.Item1); // Sort by entityId

        foreach (var (entityId, components) in sortedEntities)
        {
            var position = (PositionComponent)components[0];
            var velocity = (VelocityComponent)components[1];
            var health = (HealthComponent)components[2];
            Console.WriteLine($"Entity {entityId} position: ({position.X}, {position.Y}), velocity: ({velocity.X}, {velocity.Y}), health: {health.Health}");
        }
        Console.WriteLine();
    }
}
