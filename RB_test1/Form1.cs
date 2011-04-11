using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Random;

namespace RB_test1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            CFighter fighter1 = new CFighter(combatant1.TechTier, combatant1.Body, combatant1.BioTier, combatant1.Mind, combatant1.Weapon);
            CFighter fighter2 = new CFighter(combatant2.TechTier, combatant2.Body, combatant2.BioTier, combatant2.Mind, combatant2.Weapon);

            int rounds = 0;

            while (fighter1.DeathReason == DeathReason.None && fighter2.DeathReason == DeathReason.None)
            {
                if (Rnd.OneChanceFrom(2))
                {
                    fighter1.TakeHit(fighter2);
                    if (fighter1.DeathReason == DeathReason.None)
                        fighter2.TakeHit(fighter1);
                }
                else
                {
                    fighter2.TakeHit(fighter1);
                    if (fighter2.DeathReason == DeathReason.None)
                        fighter1.TakeHit(fighter2);
                }

                rounds++;
            }

            listBox1.Items.Add(string.Format("Получено T-ударов: {0}",  fighter1.Tech.Hits));
            listBox1.Items.Add(string.Format("Из них отражено: {0}",    fighter1.Tech.Parry));
            listBox1.Items.Add("");
            listBox1.Items.Add(string.Format("Получено B-ударов: {0}",  fighter1.Bio.Hits));
            listBox1.Items.Add(string.Format("Из них отражено: {0}",    fighter1.Bio.Parry));
            listBox1.Items.Add("");
            listBox1.Items.Add(string.Format("Причина смерти: {0}", fighter1.DeathLabel));
            listBox1.Items.Add("");
            listBox1.Items.Add(string.Format("Длительность поединка: {0}", rounds));

            listBox2.Items.Add(string.Format("Получено T-ударов: {0}",  fighter2.Tech.Hits));
            listBox2.Items.Add(string.Format("Из них отражено: {0}",    fighter2.Tech.Parry));
            listBox2.Items.Add("");
            listBox2.Items.Add(string.Format("Получено B-ударов: {0}",  fighter2.Bio.Hits));
            listBox2.Items.Add(string.Format("Из них отражено: {0}",    fighter2.Bio.Parry));
            listBox2.Items.Add("");
            listBox2.Items.Add(string.Format("Причина смерти: {0}", fighter2.DeathLabel));
            listBox2.Items.Add("");

            if (fighter1.DeathReason == DeathReason.None)
            {
                Name1.ForeColor = Color.Red;
                Name2.ForeColor = Color.Gray;
            }
            else
            {
                Name2.ForeColor = Color.Red;
                Name1.ForeColor = Color.Gray;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            CFighter fighter1 = new CFighter(combatant1.TechTier, combatant1.Body, combatant1.BioTier, combatant1.Mind, combatant1.Weapon);
            CFighter fighter2 = new CFighter(combatant2.TechTier, combatant2.Body, combatant2.BioTier, combatant2.Mind, combatant2.Weapon);

            int win1 = 0;
            int win2 = 0;

            Dictionary<DeathReason, int> death1 = new Dictionary<DeathReason, int>();
            Dictionary<DeathReason, int> death2 = new Dictionary<DeathReason, int>();

            death1[DeathReason.TechHit] = 0;
            death1[DeathReason.BioHit] = 0;
            death1[DeathReason.Wounds] = 0;
            death1[DeathReason.None] = 0;

            death2[DeathReason.TechHit] = 0;
            death2[DeathReason.BioHit] = 0;
            death2[DeathReason.Wounds] = 0;
            death2[DeathReason.None] = 0;

            int rounds = 0;
            for (int i = 0; i < 999; i++)
            {
                fighter1.RestoreHealth();
                fighter2.RestoreHealth();

                while (fighter1.DeathReason == DeathReason.None && fighter2.DeathReason == DeathReason.None)
                {
                    if (Rnd.OneChanceFrom(2))
                    {
                        fighter1.TakeHit(fighter2);
                        if (fighter1.DeathReason == DeathReason.None)
                            fighter2.TakeHit(fighter1);
                    }
                    else
                    {
                        fighter2.TakeHit(fighter1);
                        if (fighter2.DeathReason == DeathReason.None)
                            fighter1.TakeHit(fighter2);
                    }

                    rounds++;
                }

                death1[fighter1.DeathReason]++;
                death2[fighter2.DeathReason]++;

                if (fighter1.DeathReason == DeathReason.None)
                    win1++;
                else
                    win2++;
            }
            if (win1 > win2)
                win1++;
            else
                win2++;

            listBox1.Items.Add(string.Format("Получено T-ударов: {0}", fighter1.Tech.Hits));
            listBox1.Items.Add(string.Format("Из них отражено: {0}", fighter1.Tech.Parry));
            listBox1.Items.Add("");
            listBox1.Items.Add(string.Format("Получено B-ударов: {0}", fighter1.Bio.Hits));
            listBox1.Items.Add(string.Format("Из них отражено: {0}", fighter1.Bio.Parry));
            listBox1.Items.Add("");
            listBox1.Items.Add(string.Format("Побед в 1000 поединках: {0}", win1));
            listBox1.Items.Add("");
            listBox1.Items.Add(string.Format("Смертей от техники: {0}", death1[DeathReason.TechHit]));
            listBox1.Items.Add(string.Format("Смертей от бионики: {0}", death1[DeathReason.BioHit]));
            listBox1.Items.Add(string.Format("Смертей от ран: {0}", death1[DeathReason.Wounds]));
            listBox1.Items.Add("");
            listBox1.Items.Add(string.Format("Средняя длительность поединка: {0}", (double)rounds/1000));

            listBox2.Items.Add(string.Format("Получено T-ударов: {0}", fighter2.Tech.Hits));
            listBox2.Items.Add(string.Format("Из них отражено: {0}", fighter2.Tech.Parry));
            listBox2.Items.Add("");
            listBox2.Items.Add(string.Format("Получено B-ударов: {0}", fighter2.Bio.Hits));
            listBox2.Items.Add(string.Format("Из них отражено: {0}", fighter2.Bio.Parry));
            listBox2.Items.Add("");
            listBox2.Items.Add(string.Format("Побед в 1000 поединках: {0}", win2));
            listBox2.Items.Add("");
            listBox2.Items.Add(string.Format("Смертей от техники: {0}", death2[DeathReason.TechHit]));
            listBox2.Items.Add(string.Format("Смертей от бионики: {0}", death2[DeathReason.BioHit]));
            listBox2.Items.Add(string.Format("Смертей от ран: {0}", death2[DeathReason.Wounds]));

            if (win1 > win2)
            {
                Name1.ForeColor = Color.Red;
                Name2.ForeColor = Color.Gray;
            }
            else
            {
                Name2.ForeColor = Color.Red;
                Name1.ForeColor = Color.Gray;
            }
        }
    }
}