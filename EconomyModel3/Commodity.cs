using System;
using System.Collections.Generic;
using System.Text;

namespace EconomyModel3
{
    /// <summary>
    /// Представленные в игре товары
    /// </summary>
    public enum Commodity
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
        Jewelery,
        FashionClothes,
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
    public static class CommodityCategorizer
    {
        /// <summary>
        /// Список всех имеющихся товаров
        /// </summary>
        public static List<Commodity> All = new List<Commodity>((Commodity[])Enum.GetValues(typeof(Commodity)));

        private static Commodity[] primary = 
        { 
            Commodity.Clothes, 
            Commodity.Electronics, 
            Commodity.Food, 
            Commodity.Medicine 
        };
        /// <summary>
        /// Список товаров первой необходимости
        /// </summary>
        public static List<Commodity> Primary = new List<Commodity>(primary);

        private static Commodity[] luxury = 
        { 
            Commodity.Alcogol, 
            Commodity.Drugs, 
            Commodity.FashionClothes, 
            Commodity.Jewelery,
            Commodity.MusicRecords,
            Commodity.Parfume,
            Commodity.SimStimRecords,
            Commodity.VRModules
        };
        /// <summary>
        /// Список предметов роскоши
        /// </summary>
        public static List<Commodity> Luxury = new List<Commodity>(luxury);

        private static Commodity[] manufacturedGoodsT1 = 
        { 
            Commodity.Alloys,
            Commodity.Chemicals,
            Commodity.Programms
        };
        /// <summary>
        /// Список средств производства, для производства которых необходимы фабрики
        /// </summary>
        public static List<Commodity> ManufacturedGoodsT1 = new List<Commodity>(manufacturedGoodsT1);

        private static Commodity[] manufacturedGoodsT2 = 
        { 
            Commodity.Weapon,
            Commodity.Armor,
            
            Commodity.Clothes, 
            Commodity.Electronics, 
            Commodity.Medicine, 
            
            Commodity.Alcogol, 
            Commodity.Drugs, 
            Commodity.FashionClothes, 
            Commodity.Jewelery,
            Commodity.MusicRecords,
            Commodity.Parfume,
            Commodity.SimStimRecords,
            Commodity.VRModules
        };
        /// <summary>
        /// Список готовых товаров потребления
        /// </summary>
        public static List<Commodity> ManufacturedGoodsT2 = new List<Commodity>(manufacturedGoodsT2);

        private static Commodity[] resources = 
        { 
            Commodity.Ore,
            Commodity.Oil,
            Commodity.Alloys,
            Commodity.Chemicals,
            Commodity.Programms
        };
        /// <summary>
        /// Список товаров, необходимых для работы фабрик
        /// </summary>
        public static List<Commodity> Resources = new List<Commodity>(resources);

        private static Commodity[] primeResources = 
        { 
            Commodity.Ore,
            Commodity.Oil,
            Commodity.Food
        };
        /// <summary>
        /// Список товаров, добываемых без расхода какого-либо сырья
        /// </summary>
        public static List<Commodity> PrimeResources = new List<Commodity>(primeResources);

        private static Commodity[] tradedGoods = 
        { 
            Commodity.Ore,
            Commodity.Oil,
            Commodity.Food,

            Commodity.Alloys,
            Commodity.Chemicals,
            Commodity.Programms,
            
            Commodity.Weapon,
            Commodity.Armor,
            
            Commodity.Clothes, 
            Commodity.Electronics, 
            Commodity.Medicine, 
            
            Commodity.Alcogol, 
            Commodity.Drugs, 
            Commodity.FashionClothes, 
            Commodity.Jewelery,
            Commodity.MusicRecords,
            Commodity.Parfume,
            Commodity.SimStimRecords,
            Commodity.VRModules
        };
        /// <summary>
        /// Список товаров, являющихся предметами межзвёздной торговли
        /// </summary>
        public static List<Commodity> TradedGoods = new List<Commodity>(tradedGoods);
    }
}
