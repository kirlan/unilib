using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RB.Socium.Psichology
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
    
    public class CultureTemplate: Dictionary<Mentality, AdvancementRate>
    {
        public CultureTemplate(AdvancementRate eValue)
        {
            this[Mentality.Agression] = eValue;
            this[Mentality.Fanaticism] = eValue;
            this[Mentality.Piety] = eValue;
            this[Mentality.Treachery] = eValue;
            this[Mentality.Selfishness] = eValue;
            this[Mentality.Rudeness] = eValue;
        }

        public CultureTemplate(AdvancementRate eAgression, AdvancementRate eFanaticism, AdvancementRate ePiety, AdvancementRate eTreachery, AdvancementRate eSelfishness, AdvancementRate eRudeness)
        {
            this[Mentality.Agression] = eAgression;
            this[Mentality.Fanaticism] = eFanaticism;
            this[Mentality.Piety] = ePiety;
            this[Mentality.Treachery] = eTreachery;
            this[Mentality.Selfishness] = eSelfishness;
            this[Mentality.Rudeness] = eRudeness;
        }
    }
}
