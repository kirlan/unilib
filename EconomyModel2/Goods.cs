using System;
using System.Collections.Generic;
using System.Text;

namespace EconomyModel2
{
    public enum Goods
    {
        None,
        Ore,
        //Oil,
        //Food,
        //Alloys,
        //Chemicals,
        //Programms,
        //Weapon,
        //Armor,
        //Medicine,
        Clothes,
        //Electronics,
        //Jevelry,
        //FacionClothes,
        //Alcogol,
        //Parfume,
        //Drugs,
        //MusicRecords,
        //SimStimRecords,
        //VRModules
    }

    public static class GOODS
    {
        public static List<Goods> All = new List<Goods>((Goods[])Enum.GetValues(typeof(Goods)));

        private static Goods[] primary = 
        { 
            Goods.Clothes, 
            //Goods.Electronics, 
            //Goods.Food, 
            //Goods.Medicine 
        };

        public static List<Goods> Primary = new List<Goods>(primary);

        private static Goods[] luctury = 
        { 
            //Goods.Alcogol, 
            //Goods.Drugs, 
            //Goods.FacionClothes, 
            //Goods.Jevelry,
            //Goods.MusicRecords,
            //Goods.Parfume,
            //Goods.SimStimRecords,
            //Goods.VRModules
        };
        public static List<Goods> Luctury = new List<Goods>(luctury);

        private static Goods[] manufacturedGoods = 
        { 
            //Goods.Alloys,
            //Goods.Chemicals,
            //Goods.Programms,
            
            //Goods.Weapon,
            //Goods.Armor,
            
            Goods.Clothes, 
            //Goods.Electronics, 
            //Goods.Medicine, 
            
            //Goods.Alcogol, 
            //Goods.Drugs, 
            //Goods.FacionClothes, 
            //Goods.Jevelry,
            //Goods.MusicRecords,
            //Goods.Parfume,
            //Goods.SimStimRecords,
            //Goods.VRModules
        };
        public static List<Goods> ManufacturedGoods = new List<Goods>(manufacturedGoods);

        private static Goods[] resources = 
        { 
            Goods.Ore,
            //Goods.Oil,
            //Goods.Alloys,
            //Goods.Chemicals,
            //Goods.Programms
        };
        public static List<Goods> Resources = new List<Goods>(resources);

    }
}
