using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneLab.Genetix
{
    public enum EarsType
    {
        /// <summary>
        /// ушей нет
        /// </summary>
        None,
        /// <summary>
        /// обычные человеческие уши
        /// </summary>
        Human,
        /// <summary>
        /// заострённые эльфийские
        /// </summary>
        PointyElven,
        /// <summary>
        /// круглые звериные, как у мышей, овец или медведя
        /// </summary>
        RoundAnimal,
        /// <summary>
        /// заострённые звериные, как у хищников
        /// </summary>
        PointyAnimal,
        /// <summary>
        /// большие звериные, как у слона
        /// </summary>
        BigAnimal,
        /// <summary>
        /// усики, как у насекомых
        /// </summary>
        Feelers
    }

    public enum HairsAmount
    { 
        /// <summary>
        /// нет другой растительности на голове, кроме естественного покрова тела
        /// </summary>
        None,
        /// <summary>
        /// редкие волосы
        /// </summary>
        Sparse,
        /// <summary>
        /// густые волосы
        /// </summary>
        Thick
    }

    public enum HairsType
    { 
        /// <summary>
        /// обычные волосы
        /// </summary>
        Hair,
        /// <summary>
        /// вибриссы, как у животных
        /// </summary>
        Whiskers,
        /// <summary>
        /// щупальца
        /// </summary>
        Tentackles
    }

    public enum HairsColor
    {
        /// <summary>
        /// светлые волосы
        /// </summary>
        Blonde,
        /// <summary>
        /// тёмные волосы
        /// </summary>
        Brunette,
        /// <summary>
        /// рыжие волосы
        /// </summary>
        Red,
        /// <summary>
        /// русые волосы
        /// </summary>
        DarkBlond,
        /// <summary>
        /// альбинос - чисто белые волосы
        /// </summary>
        Albino,
        /// <summary>
        /// зелёные волосы
        /// </summary>
        Green,
        /// <summary>
        /// синие волосы
        /// </summary>
        Blue,
        /// <summary>
        /// цвет волос совпадает с цветом естественного покрова тела
        /// </summary>
        Hide
    }

    public enum NoseType
    {
        /// <summary>
        /// носа нет, только две ноздри
        /// </summary>
        None,
        /// <summary>
        /// обычный человеческий нос
        /// </summary>
        Normal,
        /// <summary>
        /// обычный звериный нос
        /// </summary>
        Snout,
        /// <summary>
        /// хобот, как у слона
        /// </summary>
        Proboscis
    }

    public enum MouthType
    {
        /// <summary>
        /// обычный рот, как у человека или другого млекопитающего животного (зубы, язык, губы)
        /// </summary>
        Normal,
        /// <summary>
        /// клюв как у птиц
        /// </summary>
        Beak,
        /// <summary>
        /// жвалы, как у насекомых
        /// </summary>
        Mandibles,
        /// <summary>
        /// щупальца, как у Ктулху
        /// </summary>
        Tentackles
    }

    public enum EyesPlacement
    {
        /// <summary>
        /// все глаза расположены на передней стороне головы и смотрят в одном направлении (вперёд).
        /// </summary>
        Tunnel,
        /// <summary>
        /// глаза разнесены на разные стороны головы и смотрят в разные стороны, обеспечивая максимальный угол обзора
        /// </summary>
        Panoramic,
        /// <summary>
        /// глаза расположены на подвижных стебельках и могут смотреть в любую сторону
        /// </summary>
        Stalks
    }

    public enum EyesType
    {
        /// <summary>
        /// обычные глаза, как у человека (круглый зрачок, закрывающееся веко)
        /// </summary>
        Normal,
        /// <summary>
        /// глаза, как у кошки (вертикальный зрачок, закрывающееся веко)
        /// </summary>
        CatEye,
        /// <summary>
        /// рыбий глаз - с круглым зрачком, прозрачной роговой пластинкой, без закрывающегося века
        /// </summary>
        FishEye,
        /// <summary>
        /// фасеточный глаз, как у стрекозы
        /// </summary>
        Facetted
    }

    public enum NeckLength
    {
        /// <summary>
        /// шеи нет, голова растёт прямо из тела (и не поворачивается)
        /// </summary>
        None,
        /// <summary>
        /// короткая, ограниченно подвижная шея, как у людей
        /// </summary>
        Short,
        /// <summary>
        /// короткая, хорошо поворачивающаяся шея, как у сов
        /// </summary>
        ShortRotary,
        /// <summary>
        /// длинная шея, как у лошадей
        /// </summary>
        Long,
        /// <summary>
        /// очень длинная и гибкая шея, как у лебедя или диплодока
        /// </summary>
        ExtraLong
    }

//Шея (отсутствует/короткая/длинная/очень длинная и гибкая)
    public class HeadGenetix: GenetixBase
    {
        public int m_iHeadsCount = 1;

        public EarsType m_eEarsType = EarsType.Human;

        public HairsAmount m_eHairsM = HairsAmount.Sparse;

        public HairsAmount m_eHairsF = HairsAmount.Thick;

        public HairsAmount m_eBeardM = HairsAmount.Thick;

        public HairsAmount m_eBeardF = HairsAmount.None;

        public HairsType m_eHairsType = HairsType.Hair;

        public List<HairsColor> m_cHairColors = new List<HairsColor>(new HairsColor[] {HairsColor.Brunette, HairsColor.Blonde, HairsColor.DarkBlond, HairsColor.Red});

        public NoseType m_eNoseType = NoseType.Normal;

        public MouthType m_eMouthType = MouthType.Normal;

        public EyesPlacement m_eEyesPlacement = EyesPlacement.Tunnel;

        public int m_iEyesCount = 2;

        public EyesType m_eEyesType = EyesType.Normal;

        public NeckLength m_eNeckLength = NeckLength.Short;

        #region GenetixBase Members

        public GenetixBase MutateRace()
        {
            throw new NotImplementedException();
        }

        public GenetixBase MutateNation()
        {
            throw new NotImplementedException();
        }

        public GenetixBase MutateFamily()
        {
            throw new NotImplementedException();
        }

        public GenetixBase MutateIndividual()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
