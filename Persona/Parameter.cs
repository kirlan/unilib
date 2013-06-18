using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persona
{
    /// <summary>
    /// Хранимый параметр.
    /// </summary>
    class Parameter
    {
        /// <summary>
        /// Тип параметра
        /// </summary>
        public enum Type
        {
            Float,
            Bool,
            String
        }

        /// <summary>
        /// Короткое имя параметра, например - "Здоровье", "$$$", "Встретил лешего"
        /// </summary>
        public string m_sName;

        /// <summary>
        /// Короткое название группы, в которую параметр должен быть отнесён при отображении пользователю.
        /// </summary>
        public string m_sGroup;
        
        /// <summary>
        /// Тип параметра: число с плавающей точкой, булевская величина или строка
        /// </summary>
        public Type m_eType;

        /// <summary>
        /// Значение в начале игры.
        /// </summary>
        public string m_sDefaultValue;

        /// <summary>
        /// Если true. то параметр будет скрыт от игрока.
        /// </summary>
        public bool m_bHidden = false;

        /// <summary>
        /// Подробное описание параметра
        /// </summary>
        public string m_sDescription;

        /// <summary>
        /// Комментарий к параметру - используется только при разработке модуля,
        /// может содержать, например, расшифровку числовых значений...
        /// </summary>
        public string m_sComment;
    }
}
