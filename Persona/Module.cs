using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Parameters;

namespace Persona
{
    /// <summary>
    /// Игровой модуль. Содержит набор ситуаций, список категорий по которым эти ситуации распределены и 
    /// список параметров, описывающих игровую ситуацию в конкретный момент времени.
    /// </summary>
    class Module
    {
        /// <summary>
        /// Короткое имя модуля.
        /// </summary>
        public string m_sName = "Новый модуль";
        
        /// <summary>
        /// Длинное описание модуля, модет содержать в себе описание игрового мира, в котором происходит действие,
        /// предысторию событий, указания на то, чего следует ожидать игрокам...
        /// </summary>
        public string m_sDescription = "Описание отсутствует";

        /// <summary>
        /// Список категорий событий, например - "Секс", "Работа", "Учёба"...
        /// </summary>
        public List<Domain> m_cDomains = new List<Domain>();
        
        /// <summary>
        /// Список возможных событий.
        /// </summary>
        public List<Event> m_cEvents = new List<Event>();

        /// <summary>
        /// Список числовых параметров, описывающих игровую ситуацию в конкретный момент времени.
        /// Числовые параметры могут участвовать в условиях сравнения и попадания в диапазон.
        /// </summary>
        public List<NumericParameter> m_cNumericParameters = new List<NumericParameter>();

        /// <summary>
        /// Список логических параметров, описывающих игровую ситуацию в конкретный момент времени.
        /// Логические параметры могут участвовать в условиях проверки истинности.
        /// </summary>
        public List<BoolParameter> m_cBoolParameters = new List<BoolParameter>();

        /// <summary>
        /// Список строковых параметров, описывающих игровую ситуацию в конкретный момент времени.
        /// Строковые параметры не могут участвовать ни в каких условиях.
        /// </summary>
        public List<StringParameter> m_cStringParameters = new List<StringParameter>();

        public Module()
        {
            m_cDomains.Add(new Domain("Семья"));
            m_cDomains.Add(new Domain("Друзья"));
            m_cDomains.Add(new Domain("Учёба"));
            m_cDomains.Add(new Domain("Работа"));
            m_cDomains.Add(new Domain("Любовь и секс"));
            m_cDomains.Add(new Domain("Здоровье"));
        }
    }
}
