using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;

namespace RB_test1
{
    public partial class Weapon : UserControl
    {
        public Weapon()
        {
            InitializeComponent();

            m_eType = WeaponType.Tech;

            comboBox1.Items.Clear();
            comboBox1.Items.Add("0 - Материальная культура отсутствует");
            comboBox1.Items.Add("1 - Допороховая эра");
            comboBox1.Items.Add("2 - Пороховая эра");
            comboBox1.Items.Add("3 - Индустриальная эра");
            comboBox1.Items.Add("4 - Кибернетическая эра");
            comboBox1.Items.Add("5 - Энергетическая эра");
            comboBox1.Items.Add("6 - Квантовая эра");
            comboBox1.Items.Add("7 - Лептонная эра");
            comboBox1.Items.Add("8 - Переход");
            comboBox1.SelectedIndex = 1;
        }

        public int Tier
        {
            get { return comboBox1.SelectedIndex; }
            set { comboBox1.SelectedIndex = value; }
        }

        public int Level
        {
            get { return trackBar1.Value; }
            set { trackBar1.Value = value; }
        }

        public enum WeaponType
        {
            Tech,
            Bio
        };

        private WeaponType m_eType;
        public WeaponType WType
        {
            get { return m_eType; }
            set 
            {
                if (m_eType == value)
                    return;
                m_eType = value;
                switch (m_eType)
                {
                    case WeaponType.Tech:
                        comboBox1.Items.Clear();
                        comboBox1.Items.Add("0 - Материальная культура отсутствует");
                        comboBox1.Items.Add("1 - Допороховая эра");
                        comboBox1.Items.Add("2 - Пороховая эра");
                        comboBox1.Items.Add("3 - Индустриальная эра");
                        comboBox1.Items.Add("4 - Кибернетическая эра");
                        comboBox1.Items.Add("5 - Энергетическая эра");
                        comboBox1.Items.Add("6 - Квантовая эра");
                        comboBox1.Items.Add("7 - Лептонная эра");
                        comboBox1.Items.Add("8 - Переход");
                        comboBox1.SelectedIndex = 1;
                        break;
                    case WeaponType.Bio:
                        comboBox1.Items.Clear();
                        comboBox1.Items.Add("0 - Обычный человек");
                        comboBox1.Items.Add("1 - Мистик");
                        comboBox1.Items.Add("2 - Телепат");
                        comboBox1.Items.Add("3 - Супергерой");
                        comboBox1.Items.Add("4 - Маг");
                        comboBox1.Items.Add("5 - Джинн");
                        comboBox1.Items.Add("6 - Бог");
                        comboBox1.Items.Add("7 - Демиург");
                        comboBox1.Items.Add("8 - Единый");
                        comboBox1.SelectedIndex = 1;
                        break;
                }
            }
        }
    }
}
