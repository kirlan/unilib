using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration;
using Random;
using Socium.Languages;
using Socium.Psychology;
using GeneLab;
using GeneLab.Genetix;
using System.Drawing;

namespace Socium.Nations
{
    public class Race
    {
        #region Races
        public static Race[] m_cAllRaces =
        {
        //rank 1 - usual people
            new Race("white", 1, Language.European, 
                //new LandTypeInfoX[] {LandTypes.Plains}, 
                //new LandTypeInfoX[] {LandTypes.Desert, LandTypes.Jungle, LandTypes.Tundra},
                new Phenotype.PhensStorage().Set(BrainGenetix.HumanFantasy),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoAverage).
                                             Set(HairsGenetix.HumanWhiteF),
                new MentalityTemplate(AdvancementRate.UniformlyPrecise, AdvancementRate.Leap, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.Delayed, AdvancementRate.UniformlyPrecise)),
            new Race("latini", 1, Language.Latin, 
                //new LandTypeInfoX[] {LandTypes.Plains}, 
                //new LandTypeInfoX[] {LandTypes.Desert, LandTypes.Jungle, LandTypes.Tundra},
                new Phenotype.PhensStorage().Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.Sparse, HairsType.Hair, new HairsColor[] { HairsColor.Blonde, HairsColor.Brunette, HairsColor.DarkBlond, HairsColor.Black })),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoAverage).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Blonde, HairsColor.Brunette, HairsColor.DarkBlond, HairsColor.Black })),
                new MentalityTemplate(AdvancementRate.Plateau, AdvancementRate.Delayed, AdvancementRate.Plateau, AdvancementRate.UniformlyPrecise, AdvancementRate.Delayed, AdvancementRate.UniformlyPrecise)),
            new Race("slavic", 1, Language.Slavic, 
                //new LandTypeInfoX[] {LandTypes.Plains, LandTypes.Forest}, 
                //new LandTypeInfoX[] {LandTypes.Desert, LandTypes.Jungle, LandTypes.Tundra},
                new Phenotype.PhensStorage().Set(BrainGenetix.HumanFantasy).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.Thick, HairsType.Hair, new HairsColor[] { HairsColor.Blonde, HairsColor.DarkBlond, HairsColor.Red })),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoAverage).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Blonde, HairsColor.DarkBlond, HairsColor.Red })),
                new MentalityTemplate(AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.Rapid, AdvancementRate.UniformlyModerate, AdvancementRate.UniformlyPrecise)),
            new Race("hindu", 1, Language.Hindu, 
                //new LandTypeInfoX[] {LandTypes.Plains}, 
                //new LandTypeInfoX[] {LandTypes.Tundra, LandTypes.Taiga, LandTypes.Desert},
                new Phenotype.PhensStorage().Set(HideGenetix.HumanTan).
                                             Set(BrainGenetix.HumanFantasy).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.Thick, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoAverage).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new MentalityTemplate(AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.Delayed, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.Plateau)),
            new Race("yellow", 1, Language.Asian, 
                //new LandTypeInfoX[] {LandTypes.Plains}, 
                //new LandTypeInfoX[] {LandTypes.Desert, LandTypes.Jungle, LandTypes.Tundra},
                new Phenotype.PhensStorage().Set(HideGenetix.HumanYellow).
                                             Set(BrainGenetix.HumanSF).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.Sparse, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoSmall).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new MentalityTemplate(AdvancementRate.Delayed, AdvancementRate.UniformlyPrecise, AdvancementRate.Leap, AdvancementRate.UniformlyLoose, AdvancementRate.Plateau, AdvancementRate.Plateau)),
            new Race("red", 1, Language.Aztec, 
                //new LandTypeInfoX[] {LandTypes.Plains, LandTypes.Savanna}, 
                //new LandTypeInfoX[] {LandTypes.Desert, LandTypes.Tundra},
                new Phenotype.PhensStorage().Set(HideGenetix.HumanRed).
                                             Set(BrainGenetix.HumanFantasy).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoSmall).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new MentalityTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise)),
            new Race("hellene", 1, Language.Greek, 
                //new LandTypeInfoX[] {LandTypes.Plains/*, LandTypes.Savanna*/}, 
                //new LandTypeInfoX[] {LandTypes.Desert, LandTypes.Jungle, LandTypes.Tundra, LandTypes.Swamp, LandTypes.Taiga},
                new Phenotype.PhensStorage().Set(HideGenetix.HumanTan).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.Thick, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond, HairsColor.Blonde })),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoSmall).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond, HairsColor.Blonde })),
                new MentalityTemplate(AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.Plateau)),
            new Race("dervish", 1, Language.Arabian, 
                //new LandTypeInfoX[] {LandTypes.Desert/*, LandTypes.Savanna*/}, 
                //new LandTypeInfoX[] {LandTypes.Tundra, LandTypes.Swamp, LandTypes.Jungle, LandTypes.Forest, LandTypes.Taiga},
                new Phenotype.PhensStorage().Set(HideGenetix.HumanTan).
                                             Set(BrainGenetix.HumanFantasy).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.Thick, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoAverage).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new MentalityTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Plateau)),
            new Race("barb", 1, Language.Northman, 
                //new LandTypeInfoX[] {LandTypes.Tundra, LandTypes.Taiga}, 
                //new LandTypeInfoX[] {LandTypes.Swamp, LandTypes.Desert, LandTypes.Jungle, LandTypes.Savanna},
                new Phenotype.PhensStorage().Set(BodyGenetix.Barbarian).
                                             Set(BrainGenetix.HumanFantasy).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.Thick, HairsType.Hair, new HairsColor[] { HairsColor.Blonde, HairsColor.Albino, HairsColor.DarkBlond, HairsColor.Red })),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoBig).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Blonde, HairsColor.Albino, HairsColor.DarkBlond, HairsColor.Red })),
                new MentalityTemplate(AdvancementRate.Leap, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.Delayed, AdvancementRate.Delayed)),
            new Race("nomad", 1, Language.Eskimoid, 
                //new LandTypeInfoX[] {LandTypes.Tundra, LandTypes.Plains}, 
                //new LandTypeInfoX[] {LandTypes.Desert, LandTypes.Jungle, LandTypes.Mountains, LandTypes.Forest, LandTypes.Swamp, LandTypes.Taiga},
                new Phenotype.PhensStorage().Set(HideGenetix.HumanYellow).
                                             Set(BrainGenetix.HumanFantasy).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.Sparse, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoAverage).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new MentalityTemplate(AdvancementRate.Leap, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.Delayed, AdvancementRate.Plateau, AdvancementRate.Delayed)),
            new Race("black", 1, Language.African, 
                //new LandTypeInfoX[] {LandTypes.Jungle/*, LandTypes.Desert*/, LandTypes.Savanna}, 
                //new LandTypeInfoX[] {LandTypes.Tundra, LandTypes.Forest, LandTypes.Plains, LandTypes.Taiga},
                new Phenotype.PhensStorage().Set(HideGenetix.HumanBlack).
                                             Set(BrainGenetix.HumanFantasy).
                                             Set(HairsGenetix.HumanBlackM),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoBig).
                                             Set(HairsGenetix.HumanBlackF),
                new MentalityTemplate(AdvancementRate.Leap, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Plateau)),
        //rank 10 - common non-humans
            new Race("orc", 10, Language.Orkish, 
                //new LandTypeInfoX[] {LandTypes.Plains, /*LandTypes.Mountains, */LandTypes.Savanna}, 
                //new LandTypeInfoX[] {LandTypes.Desert},
                new Phenotype.PhensStorage().Set(BodyGenetix.Barbarian).
                                             Set(NutritionGenetix.Predator).
                                             Set(HeadGenetix.Pitecantrop).
                                             Set(HideGenetix.Orc).
                                             Set(BrainGenetix.Barbarian).
                                             Set(LifeCycleGenetix.OrcM).
                                             Set(EarsGenetix.Elf).
                                             Set(HairsGenetix.None),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoBig).
                                             Set(LifeCycleGenetix.OrcF),
                new MentalityTemplate(AdvancementRate.Delayed, AdvancementRate.UniformlyLoose, AdvancementRate.Random, AdvancementRate.Plateau, AdvancementRate.Delayed, AdvancementRate.Delayed)),
            new Race("goblin", 10, Language.Orkish, 
                //new LandTypeInfoX[] {LandTypes.Plains, /*LandTypes.Mountains, */LandTypes.Savanna}, 
                //new LandTypeInfoX[] {LandTypes.Desert},
                new Phenotype.PhensStorage().Set(BodyGenetix.Goblin).
                                             Set(HideGenetix.Orc).
                                             Set(BrainGenetix.HumanFantasy).
                                             Set(LifeCycleGenetix.OrcM).
                                             Set(EarsGenetix.Elf).
                                             Set(HairsGenetix.None),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoAverage).
                                             Set(LifeCycleGenetix.OrcF),
                new MentalityTemplate(AdvancementRate.Delayed)),
            new Race("centaur", 10, Language.Greek, 
                //new LandTypeInfoX[] {LandTypes.Plains, LandTypes.Savanna}, 
                //new LandTypeInfoX[] {LandTypes.Jungle, LandTypes.Swamp, LandTypes.Mountains, LandTypes.Forest},
                new Phenotype.PhensStorage().Set(BodyGenetix.Barbarian).
                                             Set(LegsGenetix.Horse).
                                             Set(TailGenetix.Short).
                                             Set(HideGenetix.HumanTan).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.Thick, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoAverage).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new MentalityTemplate(AdvancementRate.Leap)),
            new Race("ogre", 10, Language.Orkish, 
                //new LandTypeInfoX[] {LandTypes.Savanna, LandTypes.Mountains, LandTypes.Forest}, 
                //new LandTypeInfoX[] {LandTypes.Jungle},
                new Phenotype.PhensStorage().Set(BodyGenetix.Giant).
                                             Set(HeadGenetix.Pitecantrop).
                                             Set(BrainGenetix.Barbarian).
                                             Set(LifeCycleGenetix.OrcM).
                                             Set(HairsGenetix.None),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoBig).
                                             Set(LifeCycleGenetix.OrcF),
                new MentalityTemplate(AdvancementRate.Plateau, AdvancementRate.Rapid, AdvancementRate.Random, AdvancementRate.Rapid, AdvancementRate.Plateau, AdvancementRate.Delayed)),
            new Race("halfling", 10, Language.European, 
                //new LandTypeInfoX[] {LandTypes.Plains}, 
                //new LandTypeInfoX[] {LandTypes.Tundra, LandTypes.Desert, LandTypes.Jungle, LandTypes.Mountains},
                new Phenotype.PhensStorage().Set(BodyGenetix.Hobbit),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoBig).
                                             Set(HairsGenetix.HumanWhiteF),
                new MentalityTemplate(AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed)),
            new Race("minotaur", 10, Language.Greek, 
                //new LandTypeInfoX[] {LandTypes.Jungle, LandTypes.Swamp, LandTypes.Mountains}, 
                //new LandTypeInfoX[] {LandTypes.Tundra},
                new Phenotype.PhensStorage().Set(BodyGenetix.Barbarian).
                                             Set(NutritionGenetix.Vegetarian).
                                             Set(HeadGenetix.Minotaur).
                                             Set(LegsGenetix.Demon).
                                             Set(TailGenetix.Long).
                                             Set(HideGenetix.HumanTan).
                                             Set(BrainGenetix.Barbarian).
                                             Set(LifeCycleGenetix.Barbarian).
                                             Set(FaceGenetix.Animal).
                                             Set(EarsGenetix.Herbivore).
                                             Set(EyesGenetix.Herbivore).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond, HairsColor.Red })),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoBig).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond, HairsColor.Red })),
                new MentalityTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Rapid, AdvancementRate.Delayed, AdvancementRate.Delayed)),
            new Race("elf", 10, Language.Elven, 
                //new LandTypeInfoX[] {LandTypes.Forest, LandTypes.Plains}, 
                //new LandTypeInfoX[] {LandTypes.Mountains, LandTypes.Desert, LandTypes.Swamp, LandTypes.Tundra},
                new Phenotype.PhensStorage().Set(BodyGenetix.Elf).
                                             Set(BrainGenetix.Elf).
                                             Set(LifeCycleGenetix.Elf).
                                             Set(EarsGenetix.Elf).
                                             Set(HairsGenetix.Elf),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoSmall),
                new MentalityTemplate(AdvancementRate.Rapid, AdvancementRate.Delayed, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Rapid)),
            new Race("dwarf", 10, Language.Dwarwen, 
                //new LandTypeInfoX[] {LandTypes.Mountains}, 
                //new LandTypeInfoX[] {LandTypes.Forest, LandTypes.Jungle, LandTypes.Swamp, LandTypes.Tundra},
                new Phenotype.PhensStorage().Set(BodyGenetix.Dwarf).
                                             Set(LifeCycleGenetix.DwarfM),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoBig).
                                             Set(LifeCycleGenetix.DwarfF).
                                             Set(HairsGenetix.HumanWhiteF),
                new MentalityTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Delayed, AdvancementRate.Plateau)),
            new Race("vampire", 10, Language.European, 
                //new LandTypeInfoX[] {LandTypes.Plains}, 
                //new LandTypeInfoX[] {LandTypes.Desert, LandTypes.Jungle, LandTypes.Tundra},
                new Phenotype.PhensStorage().Set(BodyGenetix.Elf).
                                             Set(NutritionGenetix.Vampire).
                                             Set(BrainGenetix.Elf).
                                             Set(LifeCycleGenetix.Elf).
                                             Set(EarsGenetix.Elf),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoBig).
                                             Set(HairsGenetix.HumanWhiteF),
                new MentalityTemplate(AdvancementRate.Plateau, AdvancementRate.Rapid, AdvancementRate.Delayed, AdvancementRate.Plateau, AdvancementRate.Delayed, AdvancementRate.Rapid)),
        //rank 20 - not so common non-humans
            new Race("cobold", 20, Language.Dwarwen, 
                //new LandTypeInfoX[] {LandTypes.Mountains}, 
                //new LandTypeInfoX[] {LandTypes.Forest, LandTypes.Jungle, LandTypes.Swamp, LandTypes.Desert, LandTypes.Tundra},
                new Phenotype.PhensStorage().Set(BodyGenetix.Dwarf).
                                             Set(HeadGenetix.Pitecantrop).
                                             Set(HideGenetix.HumanTan).
                                             Set(BrainGenetix.Barbarian).
                                             Set(LifeCycleGenetix.OrcM),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoSmall).
                                             Set(LifeCycleGenetix.OrcF).
                                             Set(HairsGenetix.HumanWhiteF),
                new MentalityTemplate(AdvancementRate.UniformlyLoose, AdvancementRate.Delayed, AdvancementRate.Random, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed)),
            new Race("gnoll", 20, Language.Orkish, 
                //new LandTypeInfoX[] {LandTypes.Savanna, LandTypes.Swamp}, 
                //new LandTypeInfoX[] {LandTypes.Jungle, LandTypes.Tundra, LandTypes.Desert},
                new Phenotype.PhensStorage().Set(BodyGenetix.Goblin).
                                             Set(BreastsGenetix.FourMale).
                                             Set(NutritionGenetix.Predator).
                                             Set(LegsGenetix.Furry).
                                             Set(TailGenetix.Long).
                                             Set(HideGenetix.Beast).
                                             Set(BrainGenetix.HumanFantasy).
                                             Set(LifeCycleGenetix.OrcM).
                                             Set(FaceGenetix.Animal).
                                             Set(EarsGenetix.Carnivore).
                                             Set(HairsGenetix.AnimalWhiskers),
                new Phenotype.PhensStorage().Set(BreastsGenetix.FourSmall).
                                             Set(LifeCycleGenetix.OrcF),
                new MentalityTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Rapid, AdvancementRate.Delayed)),
            new Race("satyr", 20, Language.Greek, 
                //new LandTypeInfoX[] {LandTypes.Forest, LandTypes.Jungle}, 
                //new LandTypeInfoX[] {LandTypes.Tundra, LandTypes.Desert},
                new Phenotype.PhensStorage().Set(HeadGenetix.DemonM).
                                             Set(LegsGenetix.Demon).
                                             Set(TailGenetix.Long).
                                             Set(HideGenetix.Beast).
                                             Set(BrainGenetix.HumanFantasy).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.Thick, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoBig).
                                             Set(HeadGenetix.DemonF).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new MentalityTemplate(AdvancementRate.Leap, AdvancementRate.Rapid, AdvancementRate.Leap, AdvancementRate.Delayed, AdvancementRate.UniformlyLoose, AdvancementRate.Rapid)),
            new Race("werewolf", 20, Language.European, 
                //new LandTypeInfoX[] {LandTypes.Forest, LandTypes.Jungle, LandTypes.Swamp}, 
                //new LandTypeInfoX[] {LandTypes.Desert},
                new Phenotype.PhensStorage().Set(BodyGenetix.Barbarian).
                                             Set(NutritionGenetix.ManEater).
                                             Set(LegsGenetix.Furry).
                                             Set(TailGenetix.Long).
                                             Set(HideGenetix.Beast).
                                             Set(BrainGenetix.Barbarian).
                                             Set(LifeCycleGenetix.OrcM).
                                             Set(FaceGenetix.Animal).
                                             Set(EarsGenetix.Carnivore).
                                             Set(HairsGenetix.AnimalWhiskers),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoBig).
                                             Set(LifeCycleGenetix.OrcF),
                new MentalityTemplate(AdvancementRate.Delayed, AdvancementRate.Leap, AdvancementRate.Leap, AdvancementRate.Rapid, AdvancementRate.Leap, AdvancementRate.Plateau)),
            new Race("feline", 20, Language.African, 
                //new LandTypeInfoX[] {LandTypes.Jungle, LandTypes.Savanna, LandTypes.Forest}, 
                //new LandTypeInfoX[] {LandTypes.Desert, LandTypes.Taiga, LandTypes.Swamp, LandTypes.Tundra},
                new Phenotype.PhensStorage().Set(BodyGenetix.Elf).
                                             Set(BreastsGenetix.FourMale).
                                             Set(NutritionGenetix.Predator).
                                             Set(LegsGenetix.Furry).
                                             Set(TailGenetix.Long).
                                             Set(HideGenetix.Beast).
                                             Set(BrainGenetix.HumanFantasy).
                                             Set(FaceGenetix.Animal).
                                             Set(EarsGenetix.Carnivore).
                                             Set(EyesGenetix.Cat).
                                             Set(HairsGenetix.AnimalWhiskers),
                new Phenotype.PhensStorage().Set(BreastsGenetix.FourSmall),
                new MentalityTemplate(AdvancementRate.Leap, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Leap, AdvancementRate.Delayed, AdvancementRate.Rapid)),
            //new RaceTemplate("yeti", 20, Language.Eskimoid, 
            //    new LandTypeInfoX[] {LandTypes.Mountains, LandTypes.Tundra}, 
            //    new LandTypeInfoX[] {LandTypes.Plains, LandTypes.Desert, LandTypes.Jungle}),
            //new RaceTemplate("littlefolk ", 20, Language.Elven, 
            //    new LandTypeInfoX[] {LandTypes.Forest}, 
            //    new LandTypeInfoX[] {LandTypes.Swamp, LandTypes.Tundra, LandTypes.Desert, LandTypes.Plains}),
            new Race("lizard", 20, Language.Aztec, 
                //new LandTypeInfoX[] {LandTypes.Swamp}, 
                //new LandTypeInfoX[] {LandTypes.Desert, LandTypes.Tundra, LandTypes.Plains, LandTypes.Mountains},
                new Phenotype.PhensStorage().Set(BodyGenetix.Elf).
                                             Set(BreastsGenetix.None).
                                             Set(LegsGenetix.Furry).
                                             Set(TailGenetix.Long).
                                             Set(HideGenetix.Reptile).
                                             Set(BrainGenetix.Elf).
                                             Set(LifeCycleGenetix.Elf).
                                             Set(FaceGenetix.Animal).
                                             Set(EarsGenetix.None).
                                             Set(HairsGenetix.None),
                null,
                new MentalityTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Rapid, AdvancementRate.Delayed)),
            new Race("reptile", 20, Language.Aztec, 
                //new LandTypeInfoX[] {LandTypes.Swamp, LandTypes.Desert, LandTypes.Jungle}, 
                //new LandTypeInfoX[] {LandTypes.Tundra, LandTypes.Forest, LandTypes.Taiga},
                new Phenotype.PhensStorage().Set(BodyGenetix.Barbarian).
                                             Set(BreastsGenetix.None).
                                             Set(NutritionGenetix.Predator).
                                             Set(LegsGenetix.Furry).
                                             Set(ArmsGenetix.Human4).
                                             Set(HideGenetix.Reptile).
                                             Set(BrainGenetix.HumanFantasy).
                                             Set(FaceGenetix.Animal).
                                             Set(EarsGenetix.None).
                                             Set(EyesGenetix.Fish).
                                             Set(HairsGenetix.None),
                null,
                new MentalityTemplate(AdvancementRate.UniformlyLoose, AdvancementRate.Rapid, AdvancementRate.UniformlyLoose, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Plateau)),
            //new RaceTemplate("half-elf ", 20, Language.Elven, 
            //    new LandTypeInfoX[] {LandTypes.Plains, LandTypes.Forest}, 
            //    new LandTypeInfoX[] {LandTypes.Swamp, LandTypes.Mountains, LandTypes.Tundra, LandTypes.Taiga, LandTypes.Desert, LandTypes.Savanna}),
            //new RaceTemplate("half-orc ", 20, Language.Orkish, 
            //    new LandTypeInfoX[] {LandTypes.Plains, LandTypes.Mountains, LandTypes.Forest}, 
            //    new LandTypeInfoX[] {LandTypes.Tundra}),
        //rank 30 - exotic non-humans
            new Race("ratling", 30, Language.Asian, 
                //new LandTypeInfoX[] {LandTypes.Plains}, 
                //new LandTypeInfoX[] {LandTypes.Desert},
                new Phenotype.PhensStorage().Set(BodyGenetix.Goblin).
                                             Set(BreastsGenetix.EightMale).
                                             Set(LegsGenetix.Furry).
                                             Set(TailGenetix.Long).
                                             Set(HideGenetix.Beast).
                                             Set(BrainGenetix.Elf).
                                             Set(LifeCycleGenetix.Barbarian).
                                             Set(FaceGenetix.Animal).
                                             Set(EarsGenetix.Carnivore).
                                             Set(HairsGenetix.AnimalWhiskers),
                new Phenotype.PhensStorage().Set(BreastsGenetix.EightSmall),
                new MentalityTemplate(AdvancementRate.Plateau, AdvancementRate.Delayed, AdvancementRate.Plateau, AdvancementRate.Delayed, AdvancementRate.Plateau, AdvancementRate.Plateau)),
            new Race("ursan", 30, Language.Slavic, 
                //new LandTypeInfoX[] {LandTypes.Forest}, 
                //new LandTypeInfoX[] {LandTypes.Desert},
                new Phenotype.PhensStorage().Set(BodyGenetix.Barbarian).
                                             Set(BreastsGenetix.FourMale).
                                             Set(LegsGenetix.Furry).
                                             Set(TailGenetix.Short).
                                             Set(HideGenetix.Beast).
                                             Set(FaceGenetix.Animal).
                                             Set(EarsGenetix.Carnivore).
                                             Set(HairsGenetix.AnimalWhiskers),
                new Phenotype.PhensStorage().Set(BreastsGenetix.FourAverage),
                new MentalityTemplate(AdvancementRate.Rapid, AdvancementRate.Plateau, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Plateau, AdvancementRate.Delayed)),
            //new RaceTemplate("half-dragon ", 30, Language.Drow, 
            //    new LandTypeInfoX[] {LandTypes.Mountains, LandTypes.Desert}, 
            //    new LandTypeInfoX[] {LandTypes.Swamp, LandTypes.Tundra, LandTypes.Forest, LandTypes.Jungle, LandTypes.Taiga}),
            //new RaceTemplate("half-dwarf ", 30, NameGenerator.Language.Dwarf, 
            //    new LandTypeInfoX[] {LandTypes.Mountains, LandTypes.Plains}, 
            //    new LandTypeInfoX[] {LandTypes.Forest, LandTypes.Jungle, LandTypes.Swamp}),
            //new RaceTemplate("half-faery ", 30, NameGenerator.Language.Elf1, 
            //    new LandTypeInfoX[] {LandTypes.Forest, LandTypes.Plains}, 
            //    new LandTypeInfoX[] {LandTypes.Tundra, LandTypes.Swamp}),
            //new RaceTemplate("golem ", 30, NameGenerator.Language.Aztec, 
            //    new LandTypeInfoX[] {LandTypes.Savanna, }, 
            //    new LandTypeInfoX[] {LandTypes.Mountains}),
            new Race("naga", 30, Language.Hindu, 
                //new LandTypeInfoX[] {LandTypes.Swamp, LandTypes.Jungle}, 
                //new LandTypeInfoX[] {LandTypes.Forest, LandTypes.Mountains, LandTypes.Tundra, LandTypes.Savanna, LandTypes.Taiga},
                new Phenotype.PhensStorage().Set(LegsGenetix.Snake).
                                             Set(ArmsGenetix.Human4).
                                             Set(TailGenetix.Long).
                                             Set(HideGenetix.Reptile).
                                             Set(BrainGenetix.Elf).
                                             Set(LifeCycleGenetix.Elf).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoAverage),
                new MentalityTemplate(AdvancementRate.Leap, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Rapid)),
            new Race("harpy", 30, Language.Greek, 
                //new LandTypeInfoX[] {LandTypes.Mountains}, 
                //new LandTypeInfoX[] {LandTypes.Forest, LandTypes.Jungle, LandTypes.Tundra, LandTypes.Taiga, LandTypes.Swamp},
                new Phenotype.PhensStorage().Set(NutritionGenetix.Predator).
                                             Set(LegsGenetix.Bird).
                                             Set(WingsGenetix.Bird).
                                             Set(TailGenetix.Short).
                                             Set(HideGenetix.Bird).
                                             Set(BrainGenetix.Barbarian).
                                             Set(LifeCycleGenetix.HarpyM).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoBig).
                                             Set(LifeCycleGenetix.HarpyF),
                new MentalityTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed)),
            new Race("faery", 30, Language.Elven, 
                //new LandTypeInfoX[] {LandTypes.Forest, LandTypes.Jungle}, 
                //new LandTypeInfoX[] {LandTypes.Desert, LandTypes.Swamp, LandTypes.Tundra, LandTypes.Mountains},
                new Phenotype.PhensStorage().Set(BodyGenetix.Pixie).
                                             Set(NutritionGenetix.Vegetarian).
                                             Set(HeadGenetix.DemonM).
                                             Set(WingsGenetix.Insect).
                                             Set(BrainGenetix.HumanFantasy).
                                             Set(LifeCycleGenetix.HarpyM).
                                             Set(EarsGenetix.Elf).
                                             Set(HairsGenetix.Elf),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoSmall).
                                             Set(HeadGenetix.DemonF).
                                             Set(LifeCycleGenetix.HarpyF),
                new MentalityTemplate(AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Random, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Rapid)),
            new Race("pixie", 30, Language.Elven, 
                //new LandTypeInfoX[] {LandTypes.Forest, LandTypes.Swamp, LandTypes.Jungle}, 
                //new LandTypeInfoX[] {LandTypes.Desert, LandTypes.Tundra, LandTypes.Mountains},
                new Phenotype.PhensStorage().Set(BodyGenetix.Pixie).
                                             Set(NutritionGenetix.Vegetarian).
                                             Set(LegsGenetix.Snake).
                                             Set(ArmsGenetix.Human4).
                                             Set(WingsGenetix.Insect).
                                             Set(TailGenetix.Long).
                                             Set(BrainGenetix.HumanFantasy).
                                             Set(LifeCycleGenetix.HarpyM).
                                             Set(EarsGenetix.Elf).
                                             Set(HairsGenetix.Elf),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoSmall).
                                             Set(LifeCycleGenetix.HarpyF),
                new MentalityTemplate(AdvancementRate.Rapid, AdvancementRate.Random, AdvancementRate.Random, AdvancementRate.Delayed, AdvancementRate.Rapid, AdvancementRate.Rapid)),
            new Race("drow", 30, Language.Drow, 
                //new LandTypeInfoX[] {LandTypes.Jungle, LandTypes.Desert, LandTypes.Mountains}, 
                //new LandTypeInfoX[] {LandTypes.Tundra},
                new Phenotype.PhensStorage().Set(HideGenetix.Drow).
                                             Set(BrainGenetix.Elf).
                                             Set(LifeCycleGenetix.Elf).
                                             Set(EarsGenetix.Elf).
                                             Set(HairsGenetix.Drow),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoAverage),
                new MentalityTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Rapid)),
        //rank 40 - powerful mythic creatures
            new Race("djinn", 40, Language.Arabian, 
                //new LandTypeInfoX[] {LandTypes.Desert}, 
                //new LandTypeInfoX[] {LandTypes.Jungle, LandTypes.Swamp, LandTypes.Forest, LandTypes.Taiga, LandTypes.Tundra, LandTypes.Plains},
                new Phenotype.PhensStorage().Set(BodyGenetix.Barbarian).
                                             Set(LegsGenetix.Ifrit).
                                             Set(HideGenetix.HumanTan).
                                             Set(BrainGenetix.Elf).
                                             Set(LifeCycleGenetix.Elf).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.Thick, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoBig).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new MentalityTemplate(AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.UniformlyLoose)),
            new Race("rakshasa", 40, Language.Hindu, 
                //new LandTypeInfoX[] {LandTypes.Jungle, LandTypes.Mountains}, 
                //new LandTypeInfoX[] {LandTypes.Tundra, LandTypes.Taiga},
                new Phenotype.PhensStorage().Set(BodyGenetix.Barbarian).
                                             Set(BreastsGenetix.FourMale).
                                             Set(NutritionGenetix.ManEater).
                                             Set(HeadGenetix.DemonM).
                                             Set(ArmsGenetix.Human4).
                                             Set(HideGenetix.Navi).
                                             Set(BrainGenetix.HumanFantasy).
                                             Set(HairsGenetix.None),
                new Phenotype.PhensStorage().Set(BreastsGenetix.FourBig).
                                             Set(HeadGenetix.DemonF),
                new MentalityTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Rapid, AdvancementRate.Delayed)),
            new Race("asura", 40, Language.Hindu, 
                //new LandTypeInfoX[] {LandTypes.Plains, LandTypes.Savanna}, 
                //new LandTypeInfoX[] {LandTypes.Jungle, LandTypes.Swamp, LandTypes.Forest, LandTypes.Taiga},
                new Phenotype.PhensStorage().Set(BodyGenetix.Giant).
                                             Set(BreastsGenetix.FourMale).
                                             Set(ArmsGenetix.Human4).
                                             Set(HideGenetix.HumanTan).
                                             Set(BrainGenetix.Elf).
                                             Set(EarsGenetix.Elf).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.Thick, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new Phenotype.PhensStorage().Set(BreastsGenetix.FourBig).
                                             Set(new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new MentalityTemplate(AdvancementRate.Plateau, AdvancementRate.Rapid, AdvancementRate.Plateau, AdvancementRate.Rapid, AdvancementRate.Delayed, AdvancementRate.Plateau)),
            new Race("drakonid", 40, Language.Drow, 
                //new LandTypeInfoX[] {LandTypes.Mountains}, 
                //new LandTypeInfoX[] {LandTypes.Swamp, LandTypes.Tundra, LandTypes.Forest, LandTypes.Jungle, LandTypes.Taiga},
                new Phenotype.PhensStorage().Set(BodyGenetix.Barbarian).
                                             Set(BreastsGenetix.None).
                                             Set(LegsGenetix.Bird).
                                             Set(WingsGenetix.Bat).
                                             Set(TailGenetix.Long).
                                             Set(HideGenetix.Reptile).
                                             Set(BrainGenetix.HumanFantasy).
                                             Set(LifeCycleGenetix.Barbarian).
                                             Set(FaceGenetix.Animal).
                                             Set(EarsGenetix.None).
                                             Set(HairsGenetix.None),
                null,
                new MentalityTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Rapid, AdvancementRate.UniformlyLoose, AdvancementRate.UniformlyLoose, AdvancementRate.Delayed)),
        //rank 50 - complete aliens
            new Race("insectoid", 50, Language.African, 
                //new LandTypeInfoX[] {LandTypes.Savanna, LandTypes.Swamp, LandTypes.Desert}, 
                //new LandTypeInfoX[] {LandTypes.Plains},
                new Phenotype.PhensStorage().Set(BodyGenetix.Elf).
                                             Set(BreastsGenetix.None).
                                             Set(HeadGenetix.Insect).
                                             Set(LegsGenetix.Insect4).
                                             Set(ArmsGenetix.Insect2).
                                             Set(WingsGenetix.None).
                                             Set(TailGenetix.None).
                                             Set(HideGenetix.Insect).
                                             Set(BrainGenetix.HumanFantasy).
                                             Set(LifeCycleGenetix.InsectM).
                                             Set(FaceGenetix.Insect).
                                             Set(EarsGenetix.None).
                                             Set(EyesGenetix.Insect).
                                             Set(HairsGenetix.None),
                new Phenotype.PhensStorage().Set(BodyGenetix.InsectQueen).
                                             Set(WingsGenetix.Insect).
                                             Set(TailGenetix.Long).
                                             Set(LifeCycleGenetix.InsectF),
                new MentalityTemplate(AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Rapid)),
            new Race("tranx", 50, Language.Asian, 
                //new LandTypeInfoX[] {LandTypes.Savanna, LandTypes.Swamp, LandTypes.Desert}, 
                //new LandTypeInfoX[] {LandTypes.Plains},
                new Phenotype.PhensStorage().Set(BodyGenetix.Elf).
                                             Set(BreastsGenetix.None).
                                             Set(HeadGenetix.Insect).
                                             Set(LegsGenetix.Insect2).
                                             Set(ArmsGenetix.Insect4).
                                             Set(WingsGenetix.None).
                                             Set(TailGenetix.None).
                                             Set(HideGenetix.Insect).
                                             Set(BrainGenetix.HumanSF).
                                             Set(LifeCycleGenetix.InsectF).
                                             Set(FaceGenetix.Insect).
                                             Set(EarsGenetix.None).
                                             Set(EyesGenetix.Insect).
                                             Set(HairsGenetix.None),
                new Phenotype.PhensStorage().Set(LifeCycleGenetix.InsectM),
                new MentalityTemplate(AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Rapid)),
            new Race("arachnid", 50, Language.Drow, 
                //new LandTypeInfoX[] {LandTypes.Swamp, LandTypes.Jungle}, 
                //new LandTypeInfoX[] {LandTypes.Desert, LandTypes.Tundra},
                new Phenotype.PhensStorage().Set(BodyGenetix.Barbarian).
                                             Set(BreastsGenetix.None).
                                             Set(NutritionGenetix.ManEater).
                                             Set(HeadGenetix.Insect).
                                             Set(LegsGenetix.Insect6).
                                             Set(ArmsGenetix.Insect2).
                                             Set(WingsGenetix.None).
                                             Set(TailGenetix.None).
                                             Set(HideGenetix.Insect).
                                             Set(BrainGenetix.HumanFantasy).
                                             Set(LifeCycleGenetix.InsectM).
                                             Set(FaceGenetix.Insect).
                                             Set(EarsGenetix.None).
                                             Set(EyesGenetix.Spider).
                                             Set(HairsGenetix.None),
                new Phenotype.PhensStorage().Set(BodyGenetix.InsectQueen).
                                             Set(LifeCycleGenetix.InsectF),
                new MentalityTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed)),
            new Race("illithid", 50, Language.Aztec, 
                //new LandTypeInfoX[] {LandTypes.Mountains, LandTypes.Jungle, LandTypes.Desert}, 
                //new LandTypeInfoX[] {LandTypes.Taiga, LandTypes.Tundra, LandTypes.Forest, LandTypes.Plains, LandTypes.Savanna},
                new Phenotype.PhensStorage().Set(NutritionGenetix.EnergyVampire).
                                             Set(LegsGenetix.Ktulhu).
                                             Set(BrainGenetix.Elf).
                                             Set(LifeCycleGenetix.Elf).
                                             Set(FaceGenetix.Ktulhu).
                                             Set(EarsGenetix.Elf).
                                             Set(EyesGenetix.Ktulhu).
                                             Set(HairsGenetix.Ktulhu),
                new Phenotype.PhensStorage().Set(BreastsGenetix.TwoAverage),
                new MentalityTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Delayed)),
        };
        #endregion

        public string m_sName;
        public int m_iRank;
        public Language m_pLanguage;

        public Phenotype m_pPhenotypeM;

        public Phenotype m_pPhenotypeF;

        public Phenotype.PhensStorage m_pGenderDiffFemale;

        public List<Nation> m_cNations = new List<Nation>();

        public MentalityTemplate m_pMentalityTemplate;

        public Race(string sName, int iRank, Language pLanguage, Phenotype.PhensStorage pDiffFromWhiteMale, Phenotype.PhensStorage pGenderDiffFemale, MentalityTemplate pCulture)
        { 
            m_sName = sName;
            m_iRank = iRank;

            m_pLanguage = pLanguage;

            m_pPhenotypeM = (Phenotype)new Phenotype(pDiffFromWhiteMale).MutateRace();
            if (m_pPhenotypeM.m_pValues.Get<HairsGenetix>().m_cHairColors.Count == 0 &&
                (m_pPhenotypeM.m_pValues.Get<HairsGenetix>().Hairs != HairsAmount.None ||
                 m_pPhenotypeM.m_pValues.Get<HairsGenetix>().Beard != HairsAmount.None))
                throw new Exception();

            var pExpectedPhenotypeF = Phenotype.Combine(m_pPhenotypeM, pGenderDiffFemale);

            m_pPhenotypeF = (Phenotype)pExpectedPhenotypeF.MutateGender();
            if (m_pPhenotypeF.m_pValues.Get<HairsGenetix>().m_cHairColors.Count == 0 &&
                (m_pPhenotypeF.m_pValues.Get<HairsGenetix>().Hairs != HairsAmount.None ||
                 m_pPhenotypeF.m_pValues.Get<HairsGenetix>().Beard != HairsAmount.None))
                throw new Exception();

            m_pGenderDiffFemale = m_pPhenotypeF.m_pValues - m_pPhenotypeM.m_pValues;

            m_pMentalityTemplate = pCulture;
        }

        public override string ToString()
        {
            return m_sName.Trim();
        }

        public void ResetNations()
        {
            m_cNations.Clear();
        }
    }
}
