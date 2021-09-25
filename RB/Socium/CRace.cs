using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Socium.Languages;
using RB.Genetix;
using RB.Genetix.GenetixParts;
using RB.Socium.Psichology;

namespace RB.Socium
{
    public class CRace
    {
        #region Races
        public static CRace[] m_cAllRaces = null;

        public static void Init()
        {
            Preset.Clear();
            m_cAllRaces = new CRace[]
            {
            //rank 1 - usual people
                new CRace("white", 1, Language.European, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra},
                    new Fenotype(BodyGenetix.Human,
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
                new CRace("latini", 1, Language.Latin, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra},
                    new Fenotype(BodyGenetix.Human,
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
                new CRace("slavic", 1, Language.Slavic, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Forest}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra},
                    new Fenotype(BodyGenetix.Human,
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
                new CRace("hindu", 1, Language.Hindu, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Desert},
                    new Fenotype(BodyGenetix.Human,
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
                new CRace("yellow", 1, Language.Asian, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra},
                    new Fenotype(BodyGenetix.Human,
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
                new CRace("red", 1, Language.Aztec, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Savanna}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra},
                    new Fenotype(BodyGenetix.Human,
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
                new CRace("hellene", 1, Language.Greek, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains/*, LandTypes<LandTypeInfoX>.Savanna*/}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Taiga},
                    new Fenotype(BodyGenetix.Human,
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
                new CRace("dervish", 1, Language.Arabian, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert/*, LandTypes<LandTypeInfoX>.Savanna*/}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Taiga},
                    new Fenotype(BodyGenetix.Human,
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
                new CRace("barb", 1, Language.Northman, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Savanna},
                    new Fenotype(BodyGenetix.Barbarian,
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
                new CRace("nomad", 1, Language.Eskimoid, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Plains}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Taiga},
                    new Fenotype(BodyGenetix.Human,
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
                new CRace("black", 1, Language.African, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle/*, LandTypes<LandTypeInfoX>.Desert*/, LandTypes<LandTypeInfoX>.Savanna}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Taiga},
                    new Fenotype(BodyGenetix.Human,
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
                new CRace("orc", 10, Language.Orkish, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, /*LandTypes<LandTypeInfoX>.Mountains, */LandTypes<LandTypeInfoX>.Savanna}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert},
                    new Fenotype(BodyGenetix.Orc,
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
                new CRace("goblin", 10, Language.Orkish, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, /*LandTypes<LandTypeInfoX>.Mountains, */LandTypes<LandTypeInfoX>.Savanna}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert},
                    new Fenotype(BodyGenetix.Goblin,
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
                new CRace("centaur", 10, Language.Greek, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Savanna}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Forest},
                    new Fenotype(BodyGenetix.Barbarian,
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
                new CRace("ogre", 10, Language.Orkish, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Forest}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle},
                    new Fenotype(BodyGenetix.Giant,
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
                new CRace("halfling", 10, Language.European, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Mountains},
                    new Fenotype(BodyGenetix.Hobbit,
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
                new CRace("minotaur", 10, Language.Greek, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Mountains}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra},
                    new Fenotype(BodyGenetix.Bull,
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
                new CRace("elf", 10, Language.Elven, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Plains}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra},
                    new Fenotype(BodyGenetix.Elf,
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
                new CRace("dwarf", 10, Language.Dwarwen, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra},
                    new Fenotype(BodyGenetix.Dwarf,
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
                new CRace("vampire", 10, Language.European, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra},
                    new Fenotype(BodyGenetix.Vampire,
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
                new CRace("cobold", 20, Language.Dwarwen, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra},
                    new Fenotype(BodyGenetix.Dwarf,
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
                new CRace("gnoll", 20, Language.Orkish, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Swamp}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Desert},
                    new Fenotype(BodyGenetix.Goblin,
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
                new CRace("satyr", 20, Language.Greek, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Desert},
                    new Fenotype(BodyGenetix.Human,
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
                new CRace("werewolf", 20, Language.European, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert},
                    new Fenotype(BodyGenetix.Werevolf,
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
                new CRace("feline", 20, Language.African, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Forest}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra},
                    new Fenotype(BodyGenetix.Furry,
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
                new CRace("lizard", 20, Language.Aztec, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Mountains},
                    new Fenotype(BodyGenetix.Furry,
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
                new CRace("reptile", 20, Language.Aztec, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Taiga},
                    new Fenotype(BodyGenetix.Barbarian,
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
                new CRace("ratling", 30, Language.Asian, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert},
                    new Fenotype(BodyGenetix.Goblin,
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
                new CRace("ursan", 30, Language.Slavic, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert},
                    new Fenotype(BodyGenetix.Barbarian,
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
                new CRace("naga", 30, Language.Hindu, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Jungle}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Taiga},
                    new Fenotype(BodyGenetix.Human,
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
                new CRace("harpy", 30, Language.Greek, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Swamp},
                    new Fenotype(BodyGenetix.Human,
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
                new CRace("faery", 30, Language.Elven, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Mountains},
                    new Fenotype(BodyGenetix.Pixie,
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
                new CRace("pixie", 30, Language.Elven, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Jungle}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Mountains},
                    new Fenotype(BodyGenetix.Pixie,
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
                new CRace("drow", 30, Language.Drow, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Mountains}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra},
                    new Fenotype(BodyGenetix.Human,
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
                new CRace("djinn", 40, Language.Arabian, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Plains},
                    new Fenotype(BodyGenetix.Barbarian,
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
                new CRace("rakshasa", 40, Language.Hindu, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Mountains}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga},
                    new Fenotype(BodyGenetix.Barbarian,
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
                new CRace("asura", 40, Language.Hindu, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Savanna}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Taiga},
                    new Fenotype(BodyGenetix.Giant,
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
                new CRace("drakonid", 40, Language.Drow, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Taiga},
                    new Fenotype(BodyGenetix.Barbarian,
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
                new CRace("insectoid", 50, Language.African, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Desert}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains},
                    new Fenotype(BodyGenetix.Elf,
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
                new CRace("arachnid", 50, Language.Drow, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Jungle}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra},
                    new Fenotype(BodyGenetix.Barbarian,
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
                new CRace("illithid", 50, Language.Aztec, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Desert}, 
                    //new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Savanna},
                    new Fenotype(BodyGenetix.Human,
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
        }
        #endregion

        public class Preset
        {
            private enum Set
            {
                HumansWestEurope,
                HumansEastEurope,
                HumansMiddleEast,
                HumansNewWorld,
                Humans,
                MithologyCreaturesWestEurope,
                MithologyCreaturesEastEurope,
                MithologyCreaturesMiddleEast,
                MithologyCreatures,
                FantasyRacesClassic,
                FantasyRacesExtended,
                FantasyRaces,
                GothicCreatures,
                BeastPeople,
                CompleteAliens
            }

            private static Dictionary<Set, Preset> s_cPresets = new Dictionary<Set, Preset>();

            internal static void Clear()
            {
                s_cPresets.Clear();
            }

            public static Preset HumansWestEurope
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Set.HumansWestEurope))
                        s_cPresets[Set.HumansWestEurope] = new Preset(Set.HumansWestEurope, new string[] { "white", "barb" });

                    return s_cPresets[Set.HumansWestEurope];
                }
            }

            public static Preset HumansEastEurope
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Set.HumansEastEurope))
                        s_cPresets[Set.HumansEastEurope] = new Preset(Set.HumansEastEurope, new string[] { "white", "barb" });

                    return s_cPresets[Set.HumansEastEurope];
                }
            }

            public static Preset HumansMiddleEast
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Set.HumansMiddleEast))
                        s_cPresets[Set.HumansMiddleEast] = new Preset(Set.HumansMiddleEast, new string[] { "hindu", "yellow", "dervish", "black" });

                    return s_cPresets[Set.HumansMiddleEast];
                }
            }

            public static Preset HumansNewWorld
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Set.HumansNewWorld))
                        s_cPresets[Set.HumansNewWorld] = new Preset(Set.HumansNewWorld, new string[] { "red", "nomad" });

                    return s_cPresets[Set.HumansNewWorld];
                }
            }

            public static Preset Humans
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Set.Humans))
                        s_cPresets[Set.Humans] = Preset.HumansEastEurope + Preset.HumansWestEurope + Preset.HumansMiddleEast + Preset.HumansNewWorld;

                    return s_cPresets[Set.Humans];
                }
            }

            public static Preset MithologyCreaturesWestEurope
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Set.MithologyCreaturesWestEurope))
                        s_cPresets[Set.MithologyCreaturesWestEurope] = new Preset(Set.MithologyCreaturesWestEurope, new string[] { "faery", "pixie" });

                    return s_cPresets[Set.MithologyCreaturesWestEurope];
                }
            }

            public static Preset MithologyCreaturesEastEurope
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Set.MithologyCreaturesEastEurope))
                        s_cPresets[Set.MithologyCreaturesEastEurope] = new Preset(Set.MithologyCreaturesEastEurope, new string[] { "centaur", "minotaur", "satyr", "harpy" });

                    return s_cPresets[Set.MithologyCreaturesEastEurope];
                }
            }

            public static Preset MithologyCreaturesMiddleEast
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Set.MithologyCreaturesMiddleEast))
                        s_cPresets[Set.MithologyCreaturesMiddleEast] = new Preset(Set.MithologyCreaturesMiddleEast, new string[] { "naga", "rakshasa", "asura", "djinn" });

                    return s_cPresets[Set.MithologyCreaturesMiddleEast];
                }
            }

            public static Preset MithologyCreatures
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Set.MithologyCreatures))
                        s_cPresets[Set.MithologyCreatures] = Preset.MithologyCreaturesWestEurope + Preset.MithologyCreaturesEastEurope + Preset.MithologyCreaturesMiddleEast;

                    return s_cPresets[Set.MithologyCreatures];
                }
            }

            public static Preset FantasyRacesClassic
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Set.FantasyRacesClassic))
                        s_cPresets[Set.FantasyRacesClassic] = new Preset(Set.FantasyRacesClassic, new string[] { "orc", "elf", "dwarf" });

                    return s_cPresets[Set.FantasyRacesClassic];
                }
            }

            public static Preset FantasyRacesExtended
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Set.FantasyRacesExtended))
                        s_cPresets[Set.FantasyRacesExtended] = new Preset(Set.FantasyRacesExtended, new string[] { "goblin", "ogre", "halfling", "cobold", "gnoll", "drow", "drakonid" });

                    return s_cPresets[Set.FantasyRacesExtended];
                }
            }

            public static Preset FantasyRaces
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Set.FantasyRaces))
                        s_cPresets[Set.FantasyRaces] = Preset.FantasyRacesClassic + Preset.FantasyRacesExtended;

                    return s_cPresets[Set.FantasyRaces];
                }
            }

            public static Preset GothicCreatures
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Set.GothicCreatures))
                        s_cPresets[Set.GothicCreatures] = new Preset(Set.GothicCreatures, new string[] { "vampire", "werewolf" });

                    return s_cPresets[Set.GothicCreatures];
                }
            }

            public static Preset BeastPeople
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Set.BeastPeople))
                        s_cPresets[Set.BeastPeople] = new Preset(Set.BeastPeople, new string[] { "feline", "lizard", "reptile", "ratling", "ursan" });

                    return s_cPresets[Set.BeastPeople];
                }
            }

            public static Preset CompleteAliens
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Set.CompleteAliens))
                        s_cPresets[Set.CompleteAliens] = new Preset(Set.CompleteAliens, new string[] { "insectoid", "arachnid", "illithid" });

                    return s_cPresets[Set.CompleteAliens];
                }
            }

            private string m_sName;

            private CRace[] m_aRaces;

            public CRace[] Races
            {
                get { return m_aRaces; }
            }

            private Preset(Set eSet, string[] aRaces)
            {
                m_sName = eSet.ToString();

                List<CRace> cRaces = new List<CRace>();
                foreach (CRace pRace in CRace.m_cAllRaces)
                {
                    if (aRaces.Contains(pRace.m_sName))
                        cRaces.Add(pRace);
                }

                m_aRaces = cRaces.ToArray();
            }

            private Preset(Preset pPreset1, Preset pPreset2)
            {
                m_sName = pPreset1.m_sName + " + " + pPreset2.m_sName;

                List<CRace> cRaces = new List<CRace>();
                cRaces.AddRange(pPreset1.m_aRaces);
                cRaces.AddRange(pPreset2.m_aRaces);

                m_aRaces = cRaces.ToArray();
            }

            public static Preset operator +(Preset a, Preset b)
            {
                return new Preset(a, b);
            }

            public static implicit operator CRace[](Preset pPreset)
            {
                return pPreset.m_aRaces;
            }

            public override string ToString()
            {
                return m_sName;
            }
        }

        public string m_sName;
        public int m_iRank;
        public Language m_pLanguage;

        public Fenotype m_pFenotype;

        public List<CNation> m_cNations = new List<CNation>();

        public CultureTemplate m_pCulture;

        public CRace(string sName, int iRank, Language pLanguage, Fenotype pFenotype, CultureTemplate pCulture)
        { 
            m_sName = sName;
            m_iRank = iRank;

            m_pLanguage = pLanguage;

            m_pFenotype = (Fenotype)pFenotype.MutateRace();

            if (m_pFenotype.m_pHairs.m_cHairColors.Count == 0 &&
                (m_pFenotype.m_pHairs.m_eHairsM != HairsAmount.None ||
                 m_pFenotype.m_pHairs.m_eHairsF != HairsAmount.None ||
                 m_pFenotype.m_pHairs.m_eBeardM != HairsAmount.None ||
                 m_pFenotype.m_pHairs.m_eBeardF != HairsAmount.None))
                throw new Exception();

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
}
