using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Socium.Psychology
{
    public enum AdvancementRate
    {
        /// <summary>
        /// Линейный рост с минимальными случайными отклонениями
        /// </summary>
        UniformlyPrecise,
        /// <summary>
        /// Линейный рост со средними случайными отклонениями
        /// </summary>
        UniformlyModerate,
        /// <summary>
        /// Линейный рост со значительными случайными отклонениями
        /// </summary>
        UniformlyLoose,
        /// <summary>
        /// Замедленное начало развития, но быстрый рост в конце
        /// </summary>
        Delayed,
        /// <summary>
        /// Ускоренное развитие в начале, но в конце период стагнации
        /// </summary>
        Rapid,
        /// <summary>
        /// Замедленное начало, потом быстрый рост в середине развития и в конце снова замедленный рост
        /// </summary>
        Leap,
        /// <summary>
        /// Ускоренное начало, потом период стагнации, в конце снова быстрый рост
        /// </summary>
        Plateau,
        /// <summary>
        /// Случайная формула роста из вышеприведённого списка
        /// </summary>
        Random // должен обязательно быть последним в списке!
    }

    [Serializable]
    public class MentalityTemplate : Dictionary<Trait, AdvancementRate>
    {
        public MentalityTemplate(AdvancementRate eValue)
        {
            this[Trait.Agression] = eValue;
            this[Trait.Fanaticism] = eValue;
            this[Trait.Piety] = eValue;
            this[Trait.Treachery] = eValue;
            this[Trait.Selfishness] = eValue;
            this[Trait.Simplicity] = eValue;
        }

        public MentalityTemplate(AdvancementRate eAgression, AdvancementRate eFanaticism, AdvancementRate ePiety, AdvancementRate eTreachery, AdvancementRate eSelfishness, AdvancementRate eRudeness)
        {
            this[Trait.Agression] = eAgression;
            this[Trait.Fanaticism] = eFanaticism;
            this[Trait.Piety] = ePiety;
            this[Trait.Treachery] = eTreachery;
            this[Trait.Selfishness] = eSelfishness;
            this[Trait.Simplicity] = eRudeness;
        }

        protected MentalityTemplate(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
