﻿using BulletHellGame.DataAccess.DataLoaders;
using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Utilities.EntityDataGenerator.EntityDataGenerators;

namespace BulletHellGame.Logic.Utilities.EntityDataGenerator
{
    /// <summary>
    /// This class is a facade for getting Data Objects that are generated in code (as opposed to reading from jsons).
    /// This is the one class that can be used to generate all the data objects needed for a playing a level.
    /// </summary>
    public static class EntityDataGenerator
    {

        public static LevelData GenerateLevelData(Rectangle playableArea)
        {
            //return LevelDataLoader.GetLevel("type");
            return LevelDataGenerator.GenerateLevelData(playableArea);
        }

        public static CharacterData CreateReimuData()
        {
            return CharacterDataGenerator.CreateReimuData();
        }

        public static CharacterData CreateMarisaData()
        {
            return CharacterDataGenerator.CreateMarisaData();
        }

        public static CharacterData CreateSakuyaData()
        {
            return CharacterDataGenerator.CreateSakuyaData();
        }

        public static CharacterData CreateEpicTestData()
        {
            return CharacterDataGenerator.CreateEpicTestData();
        }

        public static GruntData CreateEnemyData()
        {
            return GruntDataGenerator.CreateGruntData();
        }
        public static BossData CreateSubBossData()
        {
            return BossDataGenerator.CreateSubBossData();
        }

        public static BossData CreateBossData()
        {
            return BossDataGenerator.CreateBossData();
        }

        public static List<CollectibleData> GenerateRandomLoot()
        {
            return CollectibleDataGenerator.GenerateRandomLoot();
        }
    }
}
