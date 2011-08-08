using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS30Conf
{
    public enum StringCategory
    { 
        ACTION,
        COMMAND,
        CONDITION,
        REACTION,
        FETISH,
        LEISURE,
        STAT,
        ROOM,
        SKILL,
        CONFIGURATION,
        ITEM
    }

    public enum StringSubCategory
    { 
        NULL,
        ROOM,
        SKILL,
        STAT,
        STATUS,
        RND,
        ITEM,
        ITEM_GIFT,
        ITEM_HHM,
        ITEM_SEX_TOY,
        ITEM_UNIFORM
    }

    public abstract class CConfigString
    {
        private static Dictionary<StringCategory, string> m_scCategoryNames;

        public static Dictionary<StringCategory, string> CategoryNames
        {
            get { return CConfigString.m_scCategoryNames; }
        }

        private static Dictionary<StringSubCategory, string> m_scSubCategoryNames;

        public static Dictionary<StringSubCategory, string> SubCategoryNames
        {
            get { return CConfigString.m_scSubCategoryNames; }
        }

        private static CConfigRepository m_spRepository;

        internal CConfigRepository Repository
        {
            get { return m_spRepository; }
        }

        protected string m_sName;
        public string Name
        {
            get { return m_sName; }
            set
            {
                if (m_sName != value)
                {
                    m_sName = value;
                    Modify();
                }
            }
        }

        protected string m_sDescription;
        public string Desc
        {
            get { return m_sDescription; }
            set
            {
                if (m_sDescription != value)
                {
                    m_sDescription = value;
                    Modify();
                }
            }
        }

        private StringCategory m_eCategory;

        private string m_sID;
        public string ID
        {
            get { return m_sID; }
            set 
            {
                m_sID = MakeID(value);
            }
        }

        private string MakeID(string newID)
        {
            if (newID.StartsWith(m_scCategoryNames[m_eCategory]))
                return newID;
            else
                return m_scCategoryNames[m_eCategory] + newID;
        }

        public enum StringState
        { 
            Added,
            Erased,
            Modified,
            Unmodified
        }

        private StringState m_eState = StringState.Unmodified;
        public StringState State
        {
            get { return m_eState; }
        }

        public void Modify()
        {
            if (m_eState == StringState.Unmodified)
                m_eState = StringState.Modified;
        }

        private void Init(StringCategory eCategory)
        {
            if (m_spRepository == null)
                m_spRepository = new CConfigRepository();

            if (m_scCategoryNames == null)
            {
                m_scCategoryNames = new Dictionary<StringCategory, string>();
                m_scCategoryNames[StringCategory.ACTION] = "act_";
                m_scCategoryNames[StringCategory.COMMAND] = "cmd_";
                m_scCategoryNames[StringCategory.CONDITION] = "con_";
                m_scCategoryNames[StringCategory.CONFIGURATION] = "cfg_";
                m_scCategoryNames[StringCategory.FETISH] = "fet_";
                m_scCategoryNames[StringCategory.ITEM] = "itm_";
                m_scCategoryNames[StringCategory.LEISURE] = "lsr_";
                m_scCategoryNames[StringCategory.REACTION] = "rct_";
                m_scCategoryNames[StringCategory.ROOM] = "roo_";
                m_scCategoryNames[StringCategory.SKILL] = "skl_";
                m_scCategoryNames[StringCategory.STAT] = "stt_";
            }

            if (m_scSubCategoryNames == null)
            {
                m_scSubCategoryNames = new Dictionary<StringSubCategory, string>();
                m_scSubCategoryNames[StringSubCategory.STATUS] = "cmd";
                m_scSubCategoryNames[StringSubCategory.ITEM_GIFT] = "gft";
                m_scSubCategoryNames[StringSubCategory.ITEM_HHM] = "hhm";
                m_scSubCategoryNames[StringSubCategory.ITEM] = "itm";
                m_scSubCategoryNames[StringSubCategory.ROOM] = "roo";
                m_scSubCategoryNames[StringSubCategory.ITEM_SEX_TOY] = "sex";
                m_scSubCategoryNames[StringSubCategory.SKILL] = "skl";
                m_scSubCategoryNames[StringSubCategory.STAT] = "stt";
                m_scSubCategoryNames[StringSubCategory.ITEM_UNIFORM] = "uni";
            }

            if (!m_spRepository.Strings.ContainsKey(eCategory) || m_spRepository.Strings[eCategory] == null)
                m_spRepository.Strings[eCategory] = new Dictionary<string, CConfigString>();
        }

        public CConfigString(StringCategory eCategory)
        {
            Init(eCategory);

            m_eState = StringState.Added;

            m_eCategory = eCategory;
            m_sID = MakeID(DateTime.Now.Ticks.ToString("x"));

            m_spRepository.Strings[eCategory][m_sID] = this;
        }

        public CConfigString(StringCategory eCategory, string sID)
        {
            Init(eCategory);

            m_eState = StringState.Unmodified;

            m_eCategory = eCategory;
            m_sID = MakeID(sID);

            m_spRepository.Strings[eCategory][m_sID] = this;
        }

        public CConfigString(CConfigParser pParser)
        {
            m_eCategory = pParser.Category;
            Init(m_eCategory);

            Parse(pParser);
            m_eState = StringState.Unmodified;

            m_spRepository.Strings[m_eCategory][m_sID] = this;
        }

        /// <summary>
        /// Записывает в строку ID, имя и описание элемента
        /// </summary>
        /// <returns></returns>
        public virtual string GetCfgString()
        {
            return string.Format("{0}|{1}|{2}", ID, Name, Desc);
        }

        /// <summary>
        /// Считывает из трёх первых токенов ID, имя и описание элемента
        /// </summary>
        /// <param name="tokens"></param>
        public virtual void Parse(CConfigParser pParser)
        {
            ID = pParser.ReadString();
            Name = pParser.ReadString();
            Desc = pParser.ReadString();
        }

        /// <summary>
        /// Вызывается после того, как все строки считаны из файла -
        /// для того, чтобы восстановить перекрёстные ссылки (если они есть)
        /// </summary>
        public virtual void PostParse()
        { 
        }
    }
}
