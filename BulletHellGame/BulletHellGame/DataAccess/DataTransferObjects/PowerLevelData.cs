﻿namespace BulletHellGame.DataAccess.DataTransferObjects
{
    public class PowerLevelData
    {
        public List<WeaponData> MainWeapons { get; set; } = new();
        public List<OptionData> Options { get; set; } = new();
    }
}