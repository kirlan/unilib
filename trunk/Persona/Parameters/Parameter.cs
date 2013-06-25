using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persona.Parameters
{
    /// <summary>
    /// Хранимый параметр.
    /// </summary>
    public abstract class Parameter
    {
        /// <summary>
        /// Короткое имя параметра, например - "Здоровье", "$$$", "Встретил лешего"
        /// </summary>
        public string m_sName;

        /// <summary>
        /// Короткое название группы, в которую параметр должен быть отнесён при отображении пользователю.
        /// </summary>
        public string m_sGroup;
        
        /// <summary>
        /// Если true. то параметр будет скрыт от игрока.
        /// </summary>
        public bool m_bHidden = false;

        /// <summary>
        /// Комментарий к параметру - используется только при разработке модуля,
        /// может содержать, например, расшифровку числовых значений...
        /// </summary>
        public string m_sComment;
    }
}
