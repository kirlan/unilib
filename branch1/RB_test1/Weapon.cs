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
            comboBox1.Items.Add("0 - ������������ �������� �����������");
            comboBox1.Items.Add("1 - ����������� ���");
            comboBox1.Items.Add("2 - ��������� ���");
            comboBox1.Items.Add("3 - �������������� ���");
            comboBox1.Items.Add("4 - ��������������� ���");
            comboBox1.Items.Add("5 - �������������� ���");
            comboBox1.Items.Add("6 - ��������� ���");
            comboBox1.Items.Add("7 - ��������� ���");
            comboBox1.Items.Add("8 - �������");
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
                        comboBox1.Items.Add("0 - ������������ �������� �����������");
                        comboBox1.Items.Add("1 - ����������� ���");
                        comboBox1.Items.Add("2 - ��������� ���");
                        comboBox1.Items.Add("3 - �������������� ���");
                        comboBox1.Items.Add("4 - ��������������� ���");
                        comboBox1.Items.Add("5 - �������������� ���");
                        comboBox1.Items.Add("6 - ��������� ���");
                        comboBox1.Items.Add("7 - ��������� ���");
                        comboBox1.Items.Add("8 - �������");
                        comboBox1.SelectedIndex = 1;
                        break;
                    case WeaponType.Bio:
                        comboBox1.Items.Clear();
                        comboBox1.Items.Add("0 - ������� �������");
                        comboBox1.Items.Add("1 - ������");
                        comboBox1.Items.Add("2 - �������");
                        comboBox1.Items.Add("3 - ����������");
                        comboBox1.Items.Add("4 - ���");
                        comboBox1.Items.Add("5 - �����");
                        comboBox1.Items.Add("6 - ���");
                        comboBox1.Items.Add("7 - �������");
                        comboBox1.Items.Add("8 - ������");
                        comboBox1.SelectedIndex = 1;
                        break;
                }
            }
        }
    }
}
