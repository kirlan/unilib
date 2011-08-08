﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaEngine
{
    /// <summary>
    /// Распредление действующих лиц по ролям, доступных в указанном действии
    /// </summary>
    public class ActionCast
    {
        private Action m_pAction;

        public Action Action
        {
            get { return m_pAction; }
        }

        /// <summary>
        /// Здесь хранятся не самостоятельные состояния, а ссылки на состояния, которыми владеет сцена.
        /// Список ролей соответсвует списку ролей указанного действия.
        /// </summary>
        private Dictionary<Role, CharacterState> m_cRoles = new Dictionary<Role, CharacterState>();

        public Dictionary<Role, CharacterState> Roles
        {
            get { return m_cRoles; }
        }

        public ActionCast(Action pAction)
        {
            m_pAction = pAction;

            foreach (Role pRole in m_pAction.Roles)
                m_cRoles[pRole] = null;
        }
    }
}
