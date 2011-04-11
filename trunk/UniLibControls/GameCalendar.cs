using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MiscControls
{
    public partial class GameCalendar : UserControl
    {
        public GameCalendar()
        {
            InitializeComponent();
        }

        private int m_iDate = 0;

        /// <summary>
        /// Count of days, passed since game start. 
        /// </summary>
        public int DaysPassed
        {
            get { return m_iDate; }
            set 
            {
                if (value >= 0)
                {
                    m_iDate = value;
                    ReDraw();
                }
            }
        }

        private int m_iStartYear = 2008;
        /// <summary>
        /// Year of the game start.
        /// </summary>
        public int StartYear
        {
            get { return m_iStartYear; }
            set 
            { 
                m_iStartYear = value;
                ReDraw();
            }
        }

        private int m_iStartMonth = 9;
        /// <summary>
        /// Month of the game start.
        /// </summary>
        public int StartMonth
        {
            get { return m_iStartMonth; }
            set 
            {
                if (value > 0 && value <= 12)
                {
                    m_iStartMonth = value;
                    ReDraw();
                }
            }
        }

        private int m_iStartDay = 1;
        /// <summary>
        /// Day of month of the game start.
        /// </summary>
        public int StartDay
        {
            get { return m_iStartDay; }
            set 
            {
                if (value > 0 && value <= 31)
                {
                    m_iStartDay = value;
                    ReDraw();
                }
            }
        }

        private void ReDraw()
        {
            DateTime calendar = new DateTime(m_iStartYear, m_iStartMonth, m_iStartDay);
            calendar = calendar.AddDays((double)m_iDate);

            int month = calendar.Month;
            int day = calendar.Day;

            switch (month)
            {
                case 1:
                    MonthLabel.Text = "Январь";
                    break;
                case 2:
                    MonthLabel.Text = "Февраль";
                    break;
                case 3:
                    MonthLabel.Text = "Март";
                    break;
                case 4:
                    MonthLabel.Text = "Апрель";
                    break;
                case 5:
                    MonthLabel.Text = "Май";
                    break;
                case 6:
                    MonthLabel.Text = "Июнь";
                    break;
                case 7:
                    MonthLabel.Text = "Июль";
                    break;
                case 8:
                    MonthLabel.Text = "Август";
                    break;
                case 9:
                    MonthLabel.Text = "Сентябрь";
                    break;
                case 10:
                    MonthLabel.Text = "Октябрь";
                    break;
                case 11:
                    MonthLabel.Text = "Ноябрь";
                    break;
                case 12:
                    MonthLabel.Text = "Декабрь";
                    break;
            }

            calendar = new DateTime(calendar.Year, calendar.Month, 1);
            int dayIndex = 1 - (int)calendar.DayOfWeek;
            calendar = calendar.AddDays(dayIndex);
            for (int j = 1; j < CalendarTable.RowCount; j++)
                for (int i = 0; i < CalendarTable.ColumnCount; i++)
                {
                    Label lab = (Label)CalendarTable.GetControlFromPosition(i, j);
                    lab.BorderStyle = BorderStyle.None;
                    lab.Text = calendar.Day.ToString();

                    if (calendar.Month != month)
                    {
                        lab.ForeColor = Color.LightGray;
                    }
                    else
                    {
                        if (i < 5)
                            lab.ForeColor = Color.Black;
                        else
                            lab.ForeColor = Color.Red;

                        if (calendar.Day == day)
                        {
                            lab.BorderStyle = BorderStyle.FixedSingle;
                        }
                    }
                    calendar = calendar.AddDays(1);
                }
        }

        private void GameCalendar_Paint(object sender, PaintEventArgs e)
        {
            ReDraw();
        }
    }
}
