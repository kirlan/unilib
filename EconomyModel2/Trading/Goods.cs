using System;
using System.Collections.Generic;
using System.Text;

namespace EconomyModel2.Trading
{
    /// <summary>
    /// Представленные в игре товары
    /// </summary>
    public enum Goods
    {
        None,
        Ore,
        Oil,
        Food,
        Alloys,
        Chemicals,
        Programms,
        Weapon,
        Armor,
        Medicine,
        Clothes,
        Electronics,
        Jevelry,
        FacionClothes,
        Alcogol,
        Parfume,
        Drugs,
        MusicRecords,
        SimStimRecords,
        VRModules,
        Credits,
        Workers,
        Specialists
    }

    /// <summary>
    /// Категоризатор товаров
    /// </summary>
    public static class GOODS
    {
        /// <summary>
        /// Список всех имеющихся товаров
        /// </summary>
        public static List<Goods> All = new List<Goods>((Goods[])Enum.GetValues(typeof(Goods)));

        private static Goods[] primary = 
        { 
            Goods.Clothes, 
            Goods.Electronics, 
            Goods.Food, 
            Goods.Medicine 
        };
        /// <summary>
        /// Список товаров первой необходимости
        /// </summary>
        public static List<Goods> Primary = new List<Goods>(primary);

        private static Goods[] luctury = 
        { 
            Goods.Alcogol, 
            Goods.Drugs, 
            Goods.FacionClothes, 
            Goods.Jevelry,
            Goods.MusicRecords,
            Goods.Parfume,
            Goods.SimStimRecords,
            Goods.VRModules
        };
        /// <summary>
        /// Список предметов роскоши
        /// </summary>
        public static List<Goods> Luctury = new List<Goods>(luctury);

        private static Goods[] manufacturedGoods = 
        { 
            Goods.Alloys,
            Goods.Chemicals,
            Goods.Programms,
            
            Goods.Weapon,
            Goods.Armor,
            
            Goods.Clothes, 
            Goods.Electronics, 
            Goods.Medicine, 
            
            Goods.Alcogol, 
            Goods.Drugs, 
            Goods.FacionClothes, 
            Goods.Jevelry,
            Goods.MusicRecords,
            Goods.Parfume,
            Goods.SimStimRecords,
            Goods.VRModules
        };
        /// <summary>
        /// Список товаров, для производства которых необходимы фабрики
        /// </summary>
        public static List<Goods> ManufacturedGoods = new List<Goods>(manufacturedGoods);

        private static Goods[] resources = 
        { 
            Goods.Ore,
            Goods.Oil,
            Goods.Alloys,
            Goods.Chemicals,
            Goods.Programms
        };
        /// <summary>
        /// Список товаров, необходимых для работы фабрик
        /// </summary>
        public static List<Goods> Resources = new List<Goods>(resources);

        private static Goods[] primeResources = 
        { 
            Goods.Ore,
            Goods.Oil,
            Goods.Food
        };
        /// <summary>
        /// Список товаров, добываемых без расхода какого-либо сырья
        /// </summary>
        public static List<Goods> PrimeResources = new List<Goods>(primeResources);

        private static Goods[] tradedGoods = 
        { 
            Goods.Ore,
            Goods.Oil,
            Goods.Food,

            Goods.Alloys,
            Goods.Chemicals,
            Goods.Programms,
            
            Goods.Weapon,
            Goods.Armor,
            
            Goods.Clothes, 
            Goods.Electronics, 
            Goods.Medicine, 
            
            Goods.Alcogol, 
            Goods.Drugs, 
            Goods.FacionClothes, 
            Goods.Jevelry,
            Goods.MusicRecords,
            Goods.Parfume,
            Goods.SimStimRecords,
            Goods.VRModules
        };
        /// <summary>
        /// Список товаров, являющихся предметами межзвёздной торговли
        /// </summary>
        public static List<Goods> TradedGoods = new List<Goods>(tradedGoods);
    }
}
