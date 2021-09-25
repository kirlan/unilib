using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NodaTime;

namespace PnPTime
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            dateTimePicker1_ValueChanged(this, new EventArgs());
        }

        Instant unitimeStart = Instant.FromUtc(2183, 5, 7, 17, 53); //гибель Старой Земли
        //Instant torumStart = Instant.FromUtc(-1471, 3, 27, 3, 18); //дата прибытия "ковчега" с Джуном на Ланки
        Instant torumStart = Instant.FromUtc(-1887, 3, 27, 3, 18); //дата прибытия "ковчега" с Джуном на Ланки
        Instant torumStart2 = Instant.FromUtc(-1888, 8, 12, 4, 30); //дата прибытия "ковчега" с Джуном на Ланки - с поправкой Риша
        Instant tianxiaStart = Instant.FromUtc(-2696, 2, 4, 0, 0); //первый год правления Жёлтого Императора

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Instant earth = Instant.FromUtc(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day, dateTimePicker1.Value.Hour, dateTimePicker1.Value.Minute);

            Duration unitime = earth - unitimeStart;
            double uminutes_full = TimeSpan.FromTicks(unitime.Ticks).TotalMinutes;
            long uyears = (long)uminutes_full / 1000000;
            long uhours = (long)(uminutes_full - uyears * 1000000) / 100;
            long uminutes = (long)uminutes_full % 100;

            textBox1.Text = string.Format("{0}-{1}:{2}", uyears, uhours, uminutes);

            Duration torumtime = earth - torumStart2;
            //Duration torumtime = unitimeStart - torumStart;
            double tminutes_full = TimeSpan.FromTicks(torumtime.Ticks).TotalMinutes / 4;
            long tages = (long)tminutes_full / 125000000;
            long tyears = (long)(tminutes_full - tages * 125000000) / 250000;
            long thours = (long)(tminutes_full - tages * 125000000 - tyears * 250000) / 500;
            long tminutes = (long)tminutes_full % 500;

            //long eminutest = (thours * 500 + tminutes) * 4;
            //long eminutesu = uhours * 100 + uminutes;
            //Instant realtorumStart = torumStart + Duration.FromMinutes(eminutest);
            //Duration d1 = torumStart - torumStart2;
            //Duration d2 = realtorumStart - torumStart;

            textBox2.Text = string.Format("{0}.{1}.{2}.{3}", tminutes,
                thours,
                tyears,
                tages);

            Duration oldtorumtime = earth - torumStart;
            double otminutes_full = TimeSpan.FromTicks(oldtorumtime.Ticks).TotalMinutes / 4.96;
            long otages = (long)otminutes_full / 89915392;
            long otyears = (long)(otminutes_full - otages * 89915392) / 200704;
            long othours = (long)(otminutes_full - otages * 89915392 - otyears * 200704) / 448;
            long otminutes = (long)otminutes_full % 448;

            //long eminutes = (thours * 500 + tminutes) * 4;
            //Instant realtorumStart = torumStart + Duration.FromMinutes(eminutes);
            //long otage_length =  445980344; //земных минут   (примерно 848.5 земных лет)
            //long otnoght_start = 296656568; //начало "ночи богов" после начала века - в земных минутах    (примерно 564.4 земных лет)
            //
            // 15н.127уд.105уб.5 - дата принятия календарной реформы Риша. Эта дата - точка пересечения старого и нового календарей. ...или нет?
            // нам нужны такие x и y, чтобы x % 200704 = y % 250000
            // или, переиначивая нам нужны такие целочисленные x и y, чтобы x * 200704 + z = y * 250000 + z

            // 995491.84 - столько земных минут длится год по старому календарю
            // 1000000 - столько земных минут длится год по новому календарю

            long otminutes1 = otminutes % 149;
            long otminutes2 = otminutes / 149;

            long othours1 = othours % 149;
            long othours2 = othours / 149;

            long otyears1 = otyears % 149;
            long otyears2 = otyears / 149;

            textBox4.Text = string.Format("{0}{1}.{2}{3}.{4}{5}.{6}", otminutes1, otminutes2 == 0 ? "у" : (otminutes2 == 1 ? "в" : "н"),
                othours1, othours2 == 0 ? "уд" : (othours2 == 1 ? "вд" : "нд"),
                otyears1, otyears2 == 0 ? "уб" : (otyears2 == 1 ? "вб" : "нб"),
                otages);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            listBox1.Items.Clear();
            long _othours = 0;
            long _thours = 0;
            long _otminutes = 0;
            long _tminutes = 0;

            for (long j = -32; j < -30; j++)
            {
                for (long i = 0; i < 995492; i++) //длительность года по старому календарю в земных минутах
                {
                    //2334428364.8 - начало год 105уб.5 по старому календарю в земных минутах, но от торумского начала
                    Duration oldtorumtime = Duration.FromTicks(TimeSpan.FromMinutes(2334428364.8 + j * 995492 + i).Ticks);
                    Instant earth = torumStart + oldtorumtime;
                    Duration unitime = earth - unitimeStart;
                    double uminutes_full = TimeSpan.FromTicks(unitime.Ticks).TotalMinutes;
                    long uminutes = (long)uminutes_full % 1000000; //сколько земных минут прошло с начала земного года

                    long othours = (long)((double)(i % 995492) / 4.96) / 448;
                    long otminutes = (long)((double)(i % 995492) / 4.96) % 448;

                    long thours = (long)((double)uminutes / 4) / 500;
                    long tminutes = (long)((double)uminutes / 4) % 500;

                    //if (_othours != othours || _thours != thours)
                    //{
                    //    listBox1.Items.Add(string.Format("{0}  -  {1}", othours, thours));
                    //    _othours = othours;
                    //    _thours = thours;
                    //}
                    if (othours == thours)
                    {
                        if (_otminutes != otminutes || _tminutes != tminutes)
                        {
                            listBox1.Items.Add(string.Format("{0}  -  {1}", otminutes, tminutes));
                            _otminutes = otminutes;
                            _tminutes = tminutes;
                        }
                    }


                    if (othours == thours && otminutes == tminutes)
                    {
                        label6.Text += string.Format("{0}\n\r", earth);

                        double tminutes_full = otminutes + othours * 500 + (105 + j) * 250000 + 5 * 125000000;
                        Duration torumtime = Duration.FromTicks(TimeSpan.FromMinutes(tminutes_full * 4).Ticks);
                        Instant _torumStart2 = earth - torumtime;
                        label6.Text += string.Format("{0}\n\r", _torumStart2);
                        break;
                    }
                }
            }
            label6.Text += string.Format("End\n\r");
        }
    }
}
