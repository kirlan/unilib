using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Drawing2D;

namespace PnPTest1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            checkBox1.Checked = chart1.ChartAreas[0].AxisX.IsLogarithmic;

            button1_Click(this, new EventArgs());
        }

        /// <summary>
        /// Заряд аккумулятора указанной массы (эрг)
        /// </summary>
        /// <param name="fBatteryMass"></param>
        /// <returns></returns>
        private double EnergyCapacity(double fBatteryMass)
        {
            return Math.Log(fBatteryMass*Math.Pow(10, 10) + Math.Exp(-fBatteryMass)) * fBatteryMass / 50;
        }

        private double FullMass(double fBatteryMass, double fShipMass)
        {
            return fShipMass + fBatteryMass;
        }

        //private double Optimus(double fBatteryMass, double fShipMass, double fKSpeed, double fPowerLevel)
        //{
        //    double f = fBatteryMass*fKSpeed*((fBatteryMass+fShipMass*2)*Math.Log(fBatteryMass+fShipMass)-6*fBatteryMass)/(fPowerLevel*Math.Pow((fBatteryMass+fShipMass),1.5)*Math.Pow(Math.Log(fBatteryMass+fShipMass),4));
        //    if (double.IsPositiveInfinity(f))
        //        f = 10000;
        //    if (double.IsNegativeInfinity(f))
        //        f = -10000;
        //    return f;
        //}

        /// <summary>
        /// Потребление энергии (эрг / день)
        /// </summary>
        /// <param name="fBatteryMass"></param>
        /// <param name="fShipMass"></param>
        /// <param name="fPowerLevel"></param>
        /// <returns></returns>
        private double EnergyConsumption(double fBatteryMass, double fShipMass, double fPowerLevel)
        {
            //double f = fPowerLevel * Math.Pow(FullMass(fBatteryMass, fShipMass), 0.5);// *(Math.Pow(Math.Log(FullMass(fBatteryMass, fShipMass)), 3) + 30 / Math.Pow(FullMass(fBatteryMass, fShipMass), 0.1));// Math.Pow(fShipMass, 6) * Math.Exp(-(fBatteryMass + fShipMass));
            //double f = Math.Pow(FullMass(fBatteryMass, fShipMass), 0.75) + fPowerLevel * Math.Pow(Math.Log(FullMass(fBatteryMass, fShipMass)), 4);// *(Math.Pow(Math.Log(FullMass(fBatteryMass, fShipMass)), 3) + 30 / Math.Pow(FullMass(fBatteryMass, fShipMass), 0.1));// Math.Pow(fShipMass, 6) * Math.Exp(-(fBatteryMass + fShipMass));
            //double f = Math.Pow(FullMass(fBatteryMass, fShipMass), 0.75) - fPowerLevel * Math.Log(FullMass(fBatteryMass, fShipMass)) * Math.Pow(FullMass(fBatteryMass, fShipMass), 0.5);// *(Math.Pow(Math.Log(FullMass(fBatteryMass, fShipMass)), 3) + 30 / Math.Pow(FullMass(fBatteryMass, fShipMass), 0.1));// Math.Pow(fShipMass, 6) * Math.Exp(-(fBatteryMass + fShipMass));
            //double f = Math.Pow(Math.Log(FullMass(fBatteryMass, fShipMass)), fPowerLevel) * Math.Pow(FullMass(fBatteryMass, fShipMass), 2.0 / 4);// *(Math.Pow(Math.Log(FullMass(fBatteryMass, fShipMass)), 3) + 30 / Math.Pow(FullMass(fBatteryMass, fShipMass), 0.1));// Math.Pow(fShipMass, 6) * Math.Exp(-(fBatteryMass + fShipMass));
            //double f = fPowerLevel*Math.Log(FullMass(fBatteryMass, fShipMass));// *(Math.Pow(Math.Log(FullMass(fBatteryMass, fShipMass)), 3) + 30 / Math.Pow(FullMass(fBatteryMass, fShipMass), 0.1));// Math.Pow(fShipMass, 6) * Math.Exp(-(fBatteryMass + fShipMass));
            //double f = fPowerLevel * Math.Pow(FullMass(fBatteryMass, fShipMass), 0.25) / Math.Pow(Math.Log(FullMass(fBatteryMass, fShipMass)), 0.1);
            double fFullMassSqrt = Math.Sqrt(FullMass(fBatteryMass, fShipMass));
            double f = fFullMassSqrt * (fPowerLevel / (fFullMassSqrt/1000 + 1/*1152*/) + 0.035 * fFullMassSqrt / Math.Exp(FullMass(fBatteryMass, fShipMass) * 0.0000007));
            if (double.IsPositiveInfinity(f))
                f = 10000;
            if (double.IsNegativeInfinity(f))
                f = -10000;
            return f;
        }

        /// <summary>
        /// Время, которое звездолёт может лететь без перезарядки (дни)
        /// </summary>
        /// <param name="fBatteryMass"></param>
        /// <param name="fShipMass"></param>
        /// <param name="fPowerLevel"></param>
        /// <returns></returns>
        private double ChargedTime(double fBatteryMass, double fShipMass, double fPowerLevel)
        {
            //double f = Math.Pow(FullMass(fBatteryMass, fShipMass), 0.25);
            double f = EnergyCapacity(fBatteryMass) / EnergyConsumption(fBatteryMass, fShipMass, fPowerLevel);
            //double f = Math.Pow(fBatteryMass, 0.5) / FuelConsumption(fBatteryMass, fShipMass, fPowerLevel);
            //double f = Math.Pow(fBatteryMass, 1.0/3) * Math.Pow(Math.Log(fBatteryMass), 0.5) / FuelConsumption(fBatteryMass, fShipMass, fPowerLevel);
            if (double.IsPositiveInfinity(f))
                f = 10000;
            if (double.IsNegativeInfinity(f))
                f = -10000;
            return f;
        }

        /// <summary>
        /// Скорость звездолёта (световых лет / день). 
        /// Лёгкие звездолёты летают гораздо быстрее тяжёлых.
        /// </summary>
        /// <param name="fBatteryMass">масса батареи для двигателя</param>
        /// <param name="fShipMass">масса звездолёта без батареи</param>
        /// <param name="fKSpeed">модификатор скорости</param>
        /// <returns></returns>
        private double MaxSpeed(double fBatteryMass, double fShipMass, double fKSpeed, double fPowerLevel)
        {
            //double f = fKSpeed / Math.Pow(FullMass(fBatteryMass, fShipMass), 0.25);// +1 / Math.Log(Math.Pow(FullMass(fBatteryMass, fShipMass), 0.02));//fKSpeed / (fBatteryMass + fShipMass);
            //double f = fKSpeed * Math.Log(FullMass(fBatteryMass, fShipMass)) / Math.Pow(FullMass(fBatteryMass, fShipMass), 0.5);// +1 / Math.Log(Math.Pow(FullMass(fBatteryMass, fShipMass), 0.02));//fKSpeed / (fBatteryMass + fShipMass);
            //double f = fKSpeed * (1000 / (FullMass(fBatteryMass, fShipMass) * (1 + 1152 / Math.Pow(FullMass(fBatteryMass, fShipMass), 0.5))) +
            //    0.035 / Math.Exp(FullMass(fBatteryMass, fShipMass) * 0.0000007));

            //TODO: Вообще-то логичнее было было наоборот - расход энергии считать через скорость...
            double f = fKSpeed * EnergyConsumption(fBatteryMass, fShipMass, fPowerLevel) / FullMass(fBatteryMass, fShipMass);
            if (double.IsPositiveInfinity(f))
                f = 10000;
            if (double.IsNegativeInfinity(f))
                f = -10000;
            return f;
        }

        /// <summary>
        /// Дистанция, которую звездолёт может покрыть без дозаправки. Должна мало отличаться в зависимости от класса звездолёта...
        /// </summary>
        /// <param name="fBatteryMass">масса батареи для двигателя</param>
        /// <param name="fShipMass">масса звездолёта без батареи</param>
        /// <param name="fKSpeed">модификатор скорости</param>
        /// <param name="fPowerLevel">текущая мощность двигателя (0..1 - штатный режим, >1 - форсаж)</param>
        /// <returns></returns>
        private double Distance(double fBatteryMass, double fShipMass, double fKSpeed, double fPowerLevel)
        {
            double f = MaxSpeed(fBatteryMass, fShipMass, fKSpeed, fPowerLevel) * ChargedTime(fBatteryMass, fShipMass, fPowerLevel);
            if (double.IsPositiveInfinity(f))
                f = 10000;
            if (double.IsNegativeInfinity(f))
                f = -10000;
            return f;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double fLength = double.MinValue;
            double fStep0 = double.MaxValue;
            bool bChecked = false;
            
            foreach (ListViewItem pItem in listView1.Items)
            {
                if (pItem != null && pItem.Checked)
                {
                    fLength = Math.Max(5 * double.Parse(pItem.Tag.ToString()), fLength);
                    fStep0 = Math.Min(0.1 * double.Parse(pItem.Tag.ToString()), fStep0);//fLength / 1000;
                    bChecked = true;
                }
            }

            chart1.Visible = bChecked;

            if (!bChecked)
                return;
            
            chart1.Series.Clear();

            double fMax = double.MinValue;
            foreach (ListViewItem pItem in listView1.Items)
            {
                if (pItem != null && pItem.Checked)
                {
                    KColor pColor = new KColor();
                    pColor.Hue = 360.0 * pItem.Index / listView1.Items.Count;
                    pColor.Saturation = 1;
                    pColor.Lightness = 0.4;

                    Series pSerie = new Series();
                    pSerie.ChartType = SeriesChartType.FastLine;
                    pSerie.BorderWidth = 2;
                    pSerie.LegendText = pItem.Text + " max distance";
                    pSerie.Color = pColor.RGB;
                    pSerie.ToolTip = "#LEGENDTEXT for fuel mass #VALX{F0} = #VALY{F2} l.y.";

                    pColor.Saturation = 0.5;

                    Series pSerie2 = new Series();
                    pSerie2.ChartType = SeriesChartType.FastLine;
                    pSerie2.BorderWidth = 2;
                    pSerie2.BorderDashStyle = ChartDashStyle.Dot;
                    pSerie2.LegendText = pItem.Text + " speed";
                    pSerie2.Color = pColor.RGB;
                    pSerie2.ToolTip = "#LEGENDTEXT for fuel mass #VALX{F0} = #VALY{F2} l.y./day";

                    pColor.Lightness = 0.5;

                    Series pSerie3 = new Series();
                    pSerie3.ChartType = SeriesChartType.FastLine;
                    pSerie3.BorderWidth = 2;
                    pSerie3.BorderDashStyle = ChartDashStyle.Dash;
                    pSerie3.LegendText = pItem.Text + " time";
                    pSerie3.Color = pColor.RGB;
                    pSerie3.YAxisType = AxisType.Secondary;
                    pSerie3.ToolTip = "#LEGENDTEXT for fuel mass #VALX{F0} = #VALY{F2} days";

                    pColor.Saturation = 0.4;

                    Series pSerie4 = new Series();
                    pSerie4.ChartType = SeriesChartType.FastLine;
                    pSerie4.BorderWidth = 2;
                    pSerie4.BorderDashStyle = ChartDashStyle.DashDotDot;
                    pSerie4.LegendText = pItem.Text + " time on 25 l.y.";
                    pSerie4.Color = pColor.RGB;
                    pSerie4.YAxisType = AxisType.Secondary;
                    pSerie4.ToolTip = "#LEGENDTEXT for fuel mass #VALX{F0} = #VALY{F2} days";

                    double fStep = fStep0;
                    for (double i = fStep; i <= fLength; i += fStep)
                    {
                        pSerie.Points.AddXY(i, Distance(i, double.Parse(pItem.Tag.ToString()), (double)numericUpDown1.Value, (double)numericUpDown2.Value));

                        pSerie2.Points.AddXY(i, MaxSpeed(i, double.Parse(pItem.Tag.ToString()), (double)numericUpDown1.Value, (double)numericUpDown2.Value));

                        pSerie3.Points.AddXY(i, ChargedTime(i, double.Parse(pItem.Tag.ToString()), (double)numericUpDown2.Value));

                        pSerie4.Points.AddXY(i, 25 / MaxSpeed(i, double.Parse(pItem.Tag.ToString()), (double)numericUpDown1.Value, (double)numericUpDown2.Value));

                        if (i > fStep * 10)
                            fStep *= 2;
                    }

                    chart1.Series.Add(pSerie);
                    chart1.Series.Add(pSerie2);
                    chart1.Series.Add(pSerie3);
                    chart1.Series.Add(pSerie4);

                    fMax = Math.Max(fMax, pSerie.Points.FindMaxByValue().YValues[0]);
                    fMax = Math.Max(fMax, pSerie2.Points.FindMaxByValue().YValues[0]);
                    //fMax = Math.Max(fMax, pSerie3.Points.FindMaxByValue().YValues[0]);
                    fMax = Math.Max(fMax, pSerie4.Points.FindMaxByValue().YValues[0]);
                }
            }

            chart1.ChartAreas[0].AxisY.Maximum = 100;//fMax;
            chart1.ChartAreas[0].AxisY.Minimum = 0;// -fMax / 10;

            chart1.ChartAreas[0].AxisY2.Maximum = 50;//fMax;
            chart1.ChartAreas[0].AxisY2.Minimum = 0;// -fMax / 10;

            int iGridStep = 10;// ((int)(fMax / 200) + 1) * 10;

            chart1.ChartAreas[0].AxisY.Interval = iGridStep;
            chart1.ChartAreas[0].AxisY.MinorGrid.Interval = iGridStep / 5;

            chart1.ChartAreas[0].AxisY2.Interval = 5;
            chart1.ChartAreas[0].AxisY2.MinorGrid.Interval = 1;

            chart1.ChartAreas[0].AxisX.MinorGrid.Interval = 1;
            //chart1.ChartAreas[0].AxisY.RoundAxisValues();
        }

        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if (listView1.Enabled)
                e.Graphics.FillRectangle(new SolidBrush(e.ItemIndex % 2 == 0 ? listView1.BackColor : SystemColors.Menu), e.Bounds);
            else
                e.Graphics.FillRectangle(SystemBrushes.Control, e.Bounds);

            if (listView1.CheckBoxes)
            {
                if (e.Item.Checked)
                    ControlPaint.DrawCheckBox(e.Graphics, e.Bounds.X, e.Bounds.Top + 1, 15, 15, ButtonState.Flat | ButtonState.Checked);
                else
                    ControlPaint.DrawCheckBox(e.Graphics, e.Bounds.X, e.Bounds.Top + 1, 15, 15, ButtonState.Flat);
            }
        }

        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            using (StringFormat sf = new StringFormat())
            {
                switch (e.Header.TextAlign)
                {
                    case HorizontalAlignment.Center:
                        sf.Alignment = StringAlignment.Center;
                        break;
                    case HorizontalAlignment.Right:
                        sf.Alignment = StringAlignment.Far;
                        break;
                }

                Rectangle pBounds = e.Bounds;
                pBounds.Y += 2;
                if (listView1.CheckBoxes && e.ColumnIndex == 0)
                    pBounds.X += 15;

                e.Graphics.DrawString(e.SubItem.Text,
                    listView1.Font, listView1.Enabled ? new SolidBrush(listView1.ForeColor) : SystemBrushes.GrayText, pBounds, sf);
            }
        }

        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.IsLogarithmic = checkBox1.Checked;
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            button1_Click(sender, new EventArgs());
        }
    }
}
