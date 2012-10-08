using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration;
using Random;
using Socium.Languages;
using Socium.Psichology;
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
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Human,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanWhite,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                HairsGenetix.HumanWhite),
                new CultureTemplate(AdvancementRate.UniformlyPrecise, AdvancementRate.Leap, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.Delayed, AdvancementRate.UniformlyPrecise)),
            new Race("latini", 1, Language.Latin, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Human,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanWhite,
                                                BrainGenetix.HumanReal,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                new HairsGenetix(HairsAmount.Sparse, HairsAmount.Thick, HairsAmount.Sparse, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Blonde, HairsColor.Brunette, HairsColor.DarkBlond, HairsColor.Black })),
                new CultureTemplate(AdvancementRate.Plateau, AdvancementRate.Delayed, AdvancementRate.Plateau, AdvancementRate.UniformlyPrecise, AdvancementRate.Delayed, AdvancementRate.UniformlyPrecise)),
            new Race("slavic", 1, Language.Slavic, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Forest}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Human,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanWhite,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                new HairsGenetix(HairsAmount.Thick, HairsAmount.Thick, HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Blonde, HairsColor.DarkBlond, HairsColor.Red })),
                new CultureTemplate(AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.Rapid, AdvancementRate.UniformlyModerate, AdvancementRate.UniformlyPrecise)),
            new Race("hindu", 1, Language.Hindu, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Desert},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Human,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanTan,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                new HairsGenetix(HairsAmount.Sparse, HairsAmount.Thick, HairsAmount.Sparse, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new CultureTemplate(AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.Delayed, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.Plateau)),
            new Race("yellow", 1, Language.Asian, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Human,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanYellow,
                                                BrainGenetix.HumanSF,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                new HairsGenetix(HairsAmount.Sparse, HairsAmount.Thick, HairsAmount.Sparse, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new CultureTemplate(AdvancementRate.Delayed, AdvancementRate.UniformlyPrecise, AdvancementRate.Leap, AdvancementRate.UniformlyLoose, AdvancementRate.Plateau, AdvancementRate.Plateau)),
            new Race("red", 1, Language.Aztec, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Savanna}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Human,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanRed,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                new HairsGenetix(HairsAmount.Sparse, HairsAmount.Thick, HairsAmount.Sparse, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new CultureTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise)),
            new Race("hellene", 1, Language.Greek, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains/*, LandTypes<LandTypeInfoX>.Savanna*/}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Taiga},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Human,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanWhite,
                                                BrainGenetix.HumanReal,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                new HairsGenetix(HairsAmount.Sparse, HairsAmount.Thick, HairsAmount.Sparse, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond, HairsColor.Blonde })),
                new CultureTemplate(AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.Plateau)),
            new Race("dervish", 1, Language.Arabian, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert/*, LandTypes<LandTypeInfoX>.Savanna*/}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Taiga},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Human,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanTan,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                new HairsGenetix(HairsAmount.Sparse, HairsAmount.Thick, HairsAmount.Sparse, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new CultureTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Plateau)),
            new Race("barb", 1, Language.Northman, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Savanna},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Barbarian,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanWhite,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                new HairsGenetix(HairsAmount.Sparse, HairsAmount.Thick, HairsAmount.Sparse, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Blonde, HairsColor.Albino, HairsColor.DarkBlond, HairsColor.Red })),
                new CultureTemplate(AdvancementRate.Leap, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.Delayed, AdvancementRate.Delayed)),
            new Race("nomad", 1, Language.Eskimoid, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Plains}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Taiga},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Human,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanYellow,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                new HairsGenetix(HairsAmount.Sparse, HairsAmount.Thick, HairsAmount.Sparse, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new CultureTemplate(AdvancementRate.Leap, AdvancementRate.UniformlyPrecise, AdvancementRate.UniformlyPrecise, AdvancementRate.Delayed, AdvancementRate.Plateau, AdvancementRate.Delayed)),
            new Race("black", 1, Language.African, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle/*, LandTypes<LandTypeInfoX>.Desert*/, LandTypes<LandTypeInfoX>.Savanna}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Taiga},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Human,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanBlack,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                HairsGenetix.HumanBlack),
                new CultureTemplate(AdvancementRate.Leap, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Plateau)),
        //rank 10 - common non-humans
            new Race("orc", 10, Language.Orkish, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, /*LandTypes<LandTypeInfoX>.Mountains, */LandTypes<LandTypeInfoX>.Savanna}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Orc,
                                                HeadGenetix.Pitecantrop,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.Orc,
                                                BrainGenetix.Barbarian,
                                                LifeCycleGenetix.Orc,
                                                FaceGenetix.Human,
                                                EarsGenetix.Elf,
                                                EyesGenetix.Human,
                                                HairsGenetix.None),
                new CultureTemplate(AdvancementRate.Delayed, AdvancementRate.UniformlyLoose, AdvancementRate.Random, AdvancementRate.Plateau, AdvancementRate.Delayed, AdvancementRate.Delayed)),
            new Race("goblin", 10, Language.Orkish, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, /*LandTypes<LandTypeInfoX>.Mountains, */LandTypes<LandTypeInfoX>.Savanna}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Goblin,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.Orc,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Orc,
                                                FaceGenetix.Human,
                                                EarsGenetix.Elf,
                                                EyesGenetix.Human,
                                                HairsGenetix.None),
                new CultureTemplate(AdvancementRate.Delayed)),
            new Race("centaur", 10, Language.Greek, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Savanna}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Forest},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Barbarian,
                                                HeadGenetix.Human,
                                                LegsGenetix.Horse,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.Short,
                                                HideGenetix.HumanTan,
                                                BrainGenetix.HumanReal,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                new HairsGenetix(HairsAmount.Thick, HairsAmount.Thick, HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new CultureTemplate(AdvancementRate.Leap)),
            new Race("ogre", 10, Language.Orkish, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Forest}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Giant,
                                                HeadGenetix.Pitecantrop,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanWhite,
                                                BrainGenetix.Barbarian,
                                                LifeCycleGenetix.Orc,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                HairsGenetix.None),
                new CultureTemplate(AdvancementRate.Plateau, AdvancementRate.Rapid, AdvancementRate.Random, AdvancementRate.Rapid, AdvancementRate.Plateau, AdvancementRate.Delayed)),
            new Race("halfling", 10, Language.European, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Mountains},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Hobbit,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanWhite,
                                                BrainGenetix.HumanReal,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                HairsGenetix.HumanWhite),
                new CultureTemplate(AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed)),
            new Race("minotaur", 10, Language.Greek, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Mountains}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Bull,
                                                HeadGenetix.Human,
                                                LegsGenetix.Demon,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.Long,
                                                HideGenetix.HumanTan,
                                                BrainGenetix.Barbarian,
                                                LifeCycleGenetix.Barbarian,
                                                FaceGenetix.Animal,
                                                EarsGenetix.Herbivore,
                                                EyesGenetix.Herbivore,
                                                new HairsGenetix(HairsAmount.Thick, HairsAmount.Thick, HairsAmount.None, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond, HairsColor.Red })),
                new CultureTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Rapid, AdvancementRate.Delayed, AdvancementRate.Delayed)),
            new Race("elf", 10, Language.Elven, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Plains}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Elf,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanWhite,
                                                BrainGenetix.Elf,
                                                LifeCycleGenetix.Elf,
                                                FaceGenetix.Human,
                                                EarsGenetix.Elf,
                                                EyesGenetix.Human,
                                                HairsGenetix.Elf),
                new CultureTemplate(AdvancementRate.Rapid, AdvancementRate.Delayed, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Rapid)),
            new Race("dwarf", 10, Language.Dwarwen, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Dwarf,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanWhite,
                                                BrainGenetix.HumanReal,
                                                LifeCycleGenetix.Dwarf,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                HairsGenetix.Dwarf),
                new CultureTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Delayed, AdvancementRate.Plateau)),
            new Race("vampire", 10, Language.European, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Vampire,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanWhite,
                                                BrainGenetix.Elf,
                                                LifeCycleGenetix.Elf,
                                                FaceGenetix.Human,
                                                EarsGenetix.Elf,
                                                EyesGenetix.Human,
                                                HairsGenetix.HumanWhite),
                new CultureTemplate(AdvancementRate.Plateau, AdvancementRate.Rapid, AdvancementRate.Delayed, AdvancementRate.Plateau, AdvancementRate.Delayed, AdvancementRate.Rapid)),
        //rank 20 - not so common non-humans
            new Race("cobold", 20, Language.Dwarwen, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Dwarf,
                                                HeadGenetix.Pitecantrop,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanTan,
                                                BrainGenetix.Barbarian,
                                                LifeCycleGenetix.Orc,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                HairsGenetix.Dwarf),
                new CultureTemplate(AdvancementRate.UniformlyLoose, AdvancementRate.Delayed, AdvancementRate.Random, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed)),
            new Race("gnoll", 20, Language.Orkish, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Swamp}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Desert},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Goblin,
                                                HeadGenetix.Human,
                                                LegsGenetix.Furry,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.Long,
                                                HideGenetix.Beast,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Orc,
                                                FaceGenetix.Animal,
                                                EarsGenetix.Carnivore,
                                                EyesGenetix.Human,
                                                HairsGenetix.AnimalWhiskers),
                new CultureTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Rapid, AdvancementRate.Delayed)),
            new Race("satyr", 20, Language.Greek, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Desert},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Human,
                                                HeadGenetix.Human,
                                                LegsGenetix.Demon,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.Long,
                                                HideGenetix.Beast,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                new HairsGenetix(HairsAmount.Thick, HairsAmount.Thick, HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new CultureTemplate(AdvancementRate.Leap, AdvancementRate.Rapid, AdvancementRate.Leap, AdvancementRate.Delayed, AdvancementRate.UniformlyLoose, AdvancementRate.Rapid)),
            new Race("werewolf", 20, Language.European, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Werevolf,
                                                HeadGenetix.Human,
                                                LegsGenetix.Furry,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.Long,
                                                HideGenetix.Beast,
                                                BrainGenetix.Barbarian,
                                                LifeCycleGenetix.Orc,
                                                FaceGenetix.Animal,
                                                EarsGenetix.Carnivore,
                                                EyesGenetix.Human,
                                                HairsGenetix.AnimalWhiskers),
                new CultureTemplate(AdvancementRate.Delayed, AdvancementRate.Leap, AdvancementRate.Leap, AdvancementRate.Rapid, AdvancementRate.Leap, AdvancementRate.Plateau)),
            new Race("feline", 20, Language.African, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Forest}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Furry,
                                                HeadGenetix.Human,
                                                LegsGenetix.Furry,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.Long,
                                                HideGenetix.Beast,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Animal,
                                                EarsGenetix.Carnivore,
                                                EyesGenetix.Cat,
                                                HairsGenetix.AnimalWhiskers),
                new CultureTemplate(AdvancementRate.Leap, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Leap, AdvancementRate.Delayed, AdvancementRate.Rapid)),
            //new RaceTemplate("yeti", 20, Language.Eskimoid, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Tundra}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle}),
            //new RaceTemplate("littlefolk ", 20, Language.Elven, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Plains}),
            new Race("lizard", 20, Language.Aztec, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Mountains},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Furry,
                                                HeadGenetix.Human,
                                                LegsGenetix.Furry,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.Long,
                                                HideGenetix.Reptile,
                                                BrainGenetix.Elf,
                                                LifeCycleGenetix.Elf,
                                                FaceGenetix.Animal,
                                                EarsGenetix.None,
                                                EyesGenetix.Human,
                                                HairsGenetix.None),
                new CultureTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Rapid, AdvancementRate.Delayed)),
            new Race("reptile", 20, Language.Aztec, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Taiga},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Barbarian,
                                                HeadGenetix.Human,
                                                LegsGenetix.Furry,
                                                ArmsGenetix.Human4,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.Reptile,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Animal,
                                                EarsGenetix.None,
                                                EyesGenetix.Fish,
                                                HairsGenetix.None),
                new CultureTemplate(AdvancementRate.UniformlyLoose, AdvancementRate.Rapid, AdvancementRate.UniformlyLoose, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Plateau)),
            //new RaceTemplate("half-elf ", 20, Language.Elven, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Forest}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Savanna}),
            //new RaceTemplate("half-orc ", 20, Language.Orkish, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Forest}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra}),
        //rank 30 - exotic non-humans
            new Race("ratling", 30, Language.Asian, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Goblin,
                                                HeadGenetix.Human,
                                                LegsGenetix.Furry,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.Long,
                                                HideGenetix.Beast,
                                                BrainGenetix.Elf,
                                                LifeCycleGenetix.Barbarian,
                                                FaceGenetix.Animal,
                                                EarsGenetix.Carnivore,
                                                EyesGenetix.Human,
                                                HairsGenetix.AnimalWhiskers),
                new CultureTemplate(AdvancementRate.Plateau, AdvancementRate.Delayed, AdvancementRate.Plateau, AdvancementRate.Delayed, AdvancementRate.Plateau, AdvancementRate.Plateau)),
            new Race("ursan", 30, Language.Slavic, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Barbarian,
                                                HeadGenetix.Human,
                                                LegsGenetix.Furry,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.Short,
                                                HideGenetix.Beast,
                                                BrainGenetix.HumanReal,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Animal,
                                                EarsGenetix.Carnivore,
                                                EyesGenetix.Human,
                                                HairsGenetix.AnimalWhiskers),
                new CultureTemplate(AdvancementRate.Rapid, AdvancementRate.Plateau, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Plateau, AdvancementRate.Delayed)),
            //new RaceTemplate("half-dragon ", 30, Language.Drow, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Desert}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Taiga}),
            //new RaceTemplate("half-dwarf ", 30, NameGenerator.Language.Dwarf, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Plains}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp}),
            //new RaceTemplate("half-faery ", 30, NameGenerator.Language.Elf1, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Plains}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Swamp}),
            //new RaceTemplate("golem ", 30, NameGenerator.Language.Aztec, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Savanna, }, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}),
            new Race("naga", 30, Language.Hindu, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Jungle}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Taiga},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Human,
                                                HeadGenetix.Human,
                                                LegsGenetix.Snake,
                                                ArmsGenetix.Human4,
                                                WingsGenetix.None,
                                                TailGenetix.Long,
                                                HideGenetix.Reptile,
                                                BrainGenetix.Elf,
                                                LifeCycleGenetix.Elf,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                new HairsGenetix(HairsAmount.Sparse, HairsAmount.Thick, HairsAmount.None, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new CultureTemplate(AdvancementRate.Leap, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Rapid)),
            new Race("harpy", 30, Language.Greek, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Swamp},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Human,
                                                HeadGenetix.Human,
                                                LegsGenetix.Bird,
                                                ArmsGenetix.Human,
                                                WingsGenetix.Bird,
                                                TailGenetix.Short,
                                                HideGenetix.HumanTan,
                                                BrainGenetix.Barbarian,
                                                LifeCycleGenetix.Harpy,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                new HairsGenetix(HairsAmount.Thick, HairsAmount.Thick, HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new CultureTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed)),
            new Race("faery", 30, Language.Elven, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Mountains},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Pixie,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.Insect,
                                                TailGenetix.None,
                                                HideGenetix.HumanWhite,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Harpy,
                                                FaceGenetix.Human,
                                                EarsGenetix.Elf,
                                                EyesGenetix.Human,
                                                HairsGenetix.Elf),
                new CultureTemplate(AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Random, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Rapid)),
            new Race("pixie", 30, Language.Elven, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Jungle}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Mountains},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Pixie,
                                                HeadGenetix.Human,
                                                LegsGenetix.Snake,
                                                ArmsGenetix.Human4,
                                                WingsGenetix.Insect,
                                                TailGenetix.Long,
                                                HideGenetix.HumanWhite,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Harpy,
                                                FaceGenetix.Human,
                                                EarsGenetix.Elf,
                                                EyesGenetix.Human,
                                                HairsGenetix.Elf),
                new CultureTemplate(AdvancementRate.Rapid, AdvancementRate.Random, AdvancementRate.Random, AdvancementRate.Delayed, AdvancementRate.Rapid, AdvancementRate.Rapid)),
            new Race("drow", 30, Language.Drow, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Mountains}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Human,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.Drow,
                                                BrainGenetix.Elf,
                                                LifeCycleGenetix.Elf,
                                                FaceGenetix.Human,
                                                EarsGenetix.Elf,
                                                EyesGenetix.Human,
                                                HairsGenetix.Drow),
                new CultureTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Rapid)),
        //rank 40 - powerful mythic creatures
            new Race("djinn", 40, Language.Arabian, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Plains},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Barbarian,
                                                HeadGenetix.Human,
                                                LegsGenetix.Ifrit,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanTan,
                                                BrainGenetix.Elf,
                                                LifeCycleGenetix.Elf,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                new HairsGenetix(HairsAmount.Sparse, HairsAmount.Thick, HairsAmount.Sparse, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new CultureTemplate(AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.UniformlyLoose)),
            new Race("rakshasa", 40, Language.Hindu, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Mountains}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Barbarian,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human4,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.Navi,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                HairsGenetix.None),
                new CultureTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Rapid, AdvancementRate.Delayed)),
            new Race("asura", 40, Language.Hindu, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Savanna}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Taiga},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Giant,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human4,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanTan,
                                                BrainGenetix.Elf,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Human,
                                                EarsGenetix.Elf,
                                                EyesGenetix.Human,
                                                new HairsGenetix(HairsAmount.Sparse, HairsAmount.Thick, HairsAmount.Sparse, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette, HairsColor.Black, HairsColor.DarkBlond })),
                new CultureTemplate(AdvancementRate.Plateau, AdvancementRate.Rapid, AdvancementRate.Plateau, AdvancementRate.Rapid, AdvancementRate.Delayed, AdvancementRate.Plateau)),
            new Race("drakonid", 40, Language.Drow, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Taiga},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Barbarian,
                                                HeadGenetix.Human,
                                                LegsGenetix.Bird,
                                                ArmsGenetix.Human,
                                                WingsGenetix.Bat,
                                                TailGenetix.Long,
                                                HideGenetix.Reptile,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Barbarian,
                                                FaceGenetix.Animal,
                                                EarsGenetix.None,
                                                EyesGenetix.Human,
                                                HairsGenetix.None),
                new CultureTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Rapid, AdvancementRate.UniformlyLoose, AdvancementRate.UniformlyLoose, AdvancementRate.Delayed)),
        //rank 50 - complete aliens
            new Race("insectoid", 50, Language.African, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Desert}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Elf,
                                                HeadGenetix.Insect,
                                                LegsGenetix.Insect6,
                                                ArmsGenetix.Insect4,
                                                WingsGenetix.Insect,
                                                TailGenetix.Long,
                                                HideGenetix.Insect,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Insect,
                                                FaceGenetix.Insect,
                                                EarsGenetix.None,
                                                EyesGenetix.Insect,
                                                HairsGenetix.None),
                new CultureTemplate(AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Rapid)),
            new Race("arachnid", 50, Language.Drow, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Jungle}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Barbarian,
                                                HeadGenetix.Insect,
                                                LegsGenetix.Insect8,
                                                ArmsGenetix.Insect2,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.Insect,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Insect,
                                                FaceGenetix.Insect,
                                                EarsGenetix.None,
                                                EyesGenetix.Spider,
                                                HairsGenetix.None),
                new CultureTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed)),
            new Race("illithid", 50, Language.Aztec, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Desert}, 
                //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Savanna},
                new Fenotype<LandTypeInfoX>(BodyGenetix.Human,
                                                HeadGenetix.Human,
                                                LegsGenetix.Ktulhu,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanWhite,
                                                BrainGenetix.Elf,
                                                LifeCycleGenetix.Elf,
                                                FaceGenetix.Ktulhu,
                                                EarsGenetix.Elf,
                                                EyesGenetix.Ktulhu,
                                                HairsGenetix.Ktulhu),
                new CultureTemplate(AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Delayed, AdvancementRate.Rapid, AdvancementRate.Rapid, AdvancementRate.Delayed)),
        };
        #endregion

        public string m_sName;
        public int m_iRank;
        public Language m_pLanguage;

        public Fenotype<LandTypeInfoX> m_pFenotype;

        public List<Nation> m_cNations = new List<Nation>();

        public CultureTemplate m_pCulture;

        public Race(string sName, int iRank, Language pLanguage, Fenotype<LandTypeInfoX> pFenotype, CultureTemplate pCulture)
        { 
            m_sName = sName;
            m_iRank = iRank;

            m_pLanguage = pLanguage;

            m_pFenotype = (Fenotype<LandTypeInfoX>)pFenotype.MutateRace();

            m_pCulture = pCulture;
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
    
    public class Nation 
    {
        public int m_iTechLevel = 0;
        public int m_iMagicLimit = 0;

        public Fenotype<LandTypeInfoX> m_pFenotype;

        public MagicAbilityDistribution m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_weak;

        public Culture m_pCulture = null;
        public Customs m_pCustoms = null;

        public Race m_pRace = null;

        public LandTypeInfoX[] m_aPreferredLands;
        public LandTypeInfoX[] m_aHatedLands;

        public bool m_bDying = false;
        public bool m_bHegemon = false;
        public bool m_bInvader = false;

        public Epoch m_pEpoch = null;

        public string m_sName = "";

        public Nation(Race pRace, Epoch pEpoch)
        {
            m_pRace = pRace;

            m_sName = m_pRace.m_pLanguage.RandomNationName();

            m_pEpoch = pEpoch;

            bool bNew = false;
            do
            {
                m_pFenotype = (Fenotype<LandTypeInfoX>)m_pRace.m_pFenotype.MutateNation();

                bNew = !m_pFenotype.IsIdentical(m_pRace.m_pFenotype);

                foreach (Nation pOtherNation in m_pRace.m_cNations)
                    if (m_pFenotype.IsIdentical(pOtherNation.m_pFenotype))
                        bNew = false;
            }
            while(!bNew);

            m_pFenotype.GetTerritoryPreferences(out m_aPreferredLands, out m_aHatedLands);

            m_pCulture = new Culture(pRace.m_pCulture);
            m_pCustoms = new Customs();

            m_pRace.m_cNations.Add(this);
        }

        public override string ToString()
        {
            //if (m_bDying)
            //    return string.Format("ancient {1} ({0})", m_sName, m_pRace).ToLower();
            //else
            //    if(m_bHegemon)
            //        return string.Format("great {1} ({0})", m_sName, m_pRace).ToLower();
            //    else
                    return string.Format("{1} ({0})", m_sName, m_pRace).ToLower();
        }

        /// <summary>
        /// Согласовать параметры расы с параметрами мира.
        /// </summary>
        /// <param name="pWorld">мир</param>
        public void Accommodate(World pWorld, Epoch pEpoch)
        {
            if (m_bInvader)
            {
                //m_iTechLevel = Math.Min(pEpoch.m_iInvadersMaxTechLevel, pEpoch.m_iInvadersMinTechLevel + 1 + (int)(Math.Pow(Rnd.Get(20), 3) / 1000));
                //m_iMagicLimit = Math.Min(pEpoch.m_iInvadersMaxMagicLevel, pEpoch.m_iInvadersMinMagicLevel + (int)(Math.Pow(Rnd.Get(21), 3) / 1000));

                m_iTechLevel = pEpoch.m_iInvadersMinTechLevel + Rnd.Get(pEpoch.m_iInvadersMaxTechLevel - pEpoch.m_iInvadersMinTechLevel + 1);
                m_iMagicLimit = pEpoch.m_iInvadersMinMagicLevel + Rnd.Get(pEpoch.m_iInvadersMaxMagicLevel - pEpoch.m_iInvadersMinMagicLevel + 1);

                if (m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Primitive)
                    m_iTechLevel = pEpoch.m_iInvadersMinTechLevel;
            
                int iMagicLimit = (int)(Math.Pow(Rnd.Get(15), 3) / 1000);
                if (Rnd.OneChanceFrom(10))
                    m_iMagicLimit += iMagicLimit;
                else
                    m_iMagicLimit -= iMagicLimit;

                int iOldTechLevel = m_iTechLevel;

                int iTechLevel = (int)(Math.Pow(Rnd.Get(15), 3) / 1000);
                if (m_pFenotype.m_pBrain.m_eIntelligence != Intelligence.Primitive &&
                    (m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Ingenious || Rnd.OneChanceFrom(10)))
                    m_iTechLevel += iTechLevel;
                else
                    m_iTechLevel -= iTechLevel;

                if (m_iMagicLimit < pEpoch.m_iInvadersMinMagicLevel)
                    m_iMagicLimit = pEpoch.m_iInvadersMinMagicLevel;
                if (m_iMagicLimit > pEpoch.m_iInvadersMaxMagicLevel)
                    m_iMagicLimit = pEpoch.m_iInvadersMaxMagicLevel;

                if (m_iTechLevel < pEpoch.m_iInvadersMinTechLevel)
                    m_iTechLevel = pEpoch.m_iInvadersMinTechLevel;
                if (m_iTechLevel > pEpoch.m_iInvadersMaxTechLevel)
                    m_iTechLevel = pEpoch.m_iInvadersMaxTechLevel;
            }
            else
            {
                if (!m_bDying)
                {
                    if (m_iMagicLimit <= pWorld.m_iMagicLimit)
                        m_iMagicLimit = pWorld.m_iMagicLimit;
                    else
                        m_iMagicLimit = (m_iMagicLimit + pWorld.m_iMagicLimit + 1) / 2;

                    if (m_iTechLevel <= pWorld.m_iTechLevel)
                    {
                        m_iTechLevel = pWorld.m_iTechLevel;

                        if (m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Primitive)
                            m_iTechLevel = pEpoch.m_iNativesMinTechLevel;
                    }
                    else
                        m_iTechLevel = (m_iTechLevel + pWorld.m_iTechLevel + 1) / 2;
                }

                int iMagicLimit = (int)(Math.Pow(Rnd.Get(15), 3) / 1000);
                if (Rnd.OneChanceFrom(10))
                    m_iMagicLimit += iMagicLimit;
                else
                    m_iMagicLimit -= iMagicLimit;

                int iOldTechLevel = m_iTechLevel;

                int iTechLevel = (int)(Math.Pow(Rnd.Get(15), 3) / 1000);
                if (m_pFenotype.m_pBrain.m_eIntelligence != Intelligence.Primitive && 
                    (m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Ingenious || Rnd.OneChanceFrom(10)))
                    m_iTechLevel += iTechLevel;
                else
                    m_iTechLevel -= iTechLevel;

                if (!m_bDying)
                {
                    if (m_iMagicLimit < pEpoch.m_iNativesMinMagicLevel)
                        m_iMagicLimit = pEpoch.m_iNativesMinMagicLevel;
                    if (m_iMagicLimit > pEpoch.m_iNativesMaxMagicLevel)
                        m_iMagicLimit = pEpoch.m_iNativesMaxMagicLevel;

                    if (m_iTechLevel < pEpoch.m_iNativesMinTechLevel)
                        m_iTechLevel = pEpoch.m_iNativesMinTechLevel;
                    if (m_iTechLevel > pEpoch.m_iNativesMaxTechLevel)
                        m_iTechLevel = pEpoch.m_iNativesMaxTechLevel;
                }
                else
                {
                    if (m_iMagicLimit < 0)
                        m_iMagicLimit = 0;
                    if (m_iMagicLimit > 8)
                        m_iMagicLimit = 8;

                    if (m_iTechLevel < 0)
                        m_iTechLevel = 0;
                    if (m_iTechLevel > 8)
                        m_iTechLevel = 8;
                }

                //m_pCustoms = new Customs();
            }

            m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_average;
            if (m_pFenotype.m_pBrain.m_iMagicAbilityPotential > m_iMagicLimit + 1)
                m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_powerful;
            if (m_pFenotype.m_pBrain.m_iMagicAbilityPotential < m_iMagicLimit - 1)
                m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_weak;

            //int iNewLevel = Math.Max(m_iTechLevel, m_iMagicLimit);
            //if (iNewLevel > iOldLevel)
            //    for (int i = 0; i < iNewLevel * iNewLevel - iOldLevel * iOldLevel; i++)
            //    {
            //        m_pCulture.Evolve();
            //        m_pCustoms.Evolve();
            //    }
            //else
            //    for (int i = 0; i < iOldLevel - iNewLevel; i++)
            //    {
            //        m_pCulture.Degrade();
            //        m_pCustoms.Degrade();
            //    }
        }
    }
}
