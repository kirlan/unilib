using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Random;

namespace TestPlan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private long Calk(int[] A, int[] a, int x)
        {
            if (A.Length != a.Length)
                throw new ArgumentException();

            long sum = 0;
            for (int i = 0; i < A.Length; i++)
            {
                sum += (A[i] * A[i] - 3 * A[i] * x) * a[i];
            }

            return sum;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //List<string> box = new List<string>();
            listBox1.Items.Clear();
            button1.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private class Dispersion
        {
            public int[][] o = new int[4][];

            public int[][] r = new int[7][];

            public int[][] x = new int[4][];

            public int[][] y = new int[14][];

            public void Init(int len)
            {
                for (int i = 0; i < o.GetLength(0); i++)
                {
                    o[i] = new int[len];
                    for (int j = 0; j < len; j++)
                        o[i][j] = 0;
                }
                for (int i = 0; i < r.GetLength(0); i++)
                {
                    r[i] = new int[len];
                    for (int j = 0; j < len; j++)
                        r[i][j] = 0;
                }
                for (int i = 0; i < x.GetLength(0); i++)
                {
                    x[i] = new int[len];
                    for (int j = 0; j < len; j++)
                        x[i][j] = 0;
                }
                for (int i = 0; i < y.GetLength(0); i++)
                {
                    y[i] = new int[len];
                    for (int j = 0; j < len; j++)
                        y[i][j] = 0;
                }
            }

            private int GetHighIndex(int[] A, int[] a, int prod)
            {
                int index = 0;
                int counter = 0;
                do
                {
                    if (counter++ > 4 * A.Length * A.Length)
                        return -1;
                    index = (int)(Math.Sqrt(Rnd.Get(A.Length * A.Length)));
                }
                while (a[index] == 0 || A[index] * A[index] > prod);

                return index;
            }

            private int GetLowIndex(int[] A, int[] a, int prod)
            {
                int index = 0;
                //int counter = 0;
                //do
                //{
                //    if (counter++ > A.Length * A.Length)
                //        return -1;
                //    index = A.Length - 1 - (int)(Math.Sqrt(Rnd.Get(A.Length * A.Length)));
                //}
                //while (a[index] == 0 || A[index] * A[index] > prod);

                index = A.Length;
                do
                {
                    index--;
                }
                while (index >= 0 && a[index] == 0);

                return index;
            }

            public Dispersion(int[] A, int[] b)
            {
                Init(A.Length);

                int[] a = new int[b.Length];
                int pop = 0;
                int prod = 0;
                for (int i = 0; i < b.Length; i++)
                {
                    a[i] = b[i];
                    pop += a[i];
                    prod += A[i] * A[i] * a[i];
                }

                prod = prod / 3; //объём производства одной стадии (ресурс/материал/продукт)
                prod -= pop * 4; //объём производства одной стадии, приходящийся на предметы роскоши

                for (int i = 0; i < x.GetLength(0); i++)
                {
                    prod = pop;
                    while (prod > 0)
                    {
                        int index = GetHighIndex(A, a, prod);
                        if (index != -1)
                        {
                            x[i][index]++;
                            a[index]--;
                            prod -= A[index] * A[index];
                        }
                        else
                            break;
                    }
                }

                for (int i = 0; i < x.GetLength(0); i++)
                {
                    prod = pop;
                    while (prod > 0)
                    {
                        int index = GetHighIndex(A, a, prod);
                        if (index != -1)
                        {
                            r[i][index]++;
                            a[index]--;
                            prod -= A[index] * A[index];
                        }
                        else
                            break;
                    }
                }

            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            int counter = 0;

            int x = 4;
            for (int range = 13; range < 50; range++)
            {
                int[] A = new int[range];
                int[] a = new int[range];

                for (int i = 0; i < range; i++)
                {
                    A[i] = 1 + range - i;
                    a[i] = 0;
                }

                //worker.ReportProgress(range);
                counter = 0;
                int counter2 = 0;
                int rr = 0;
                do
                {
                    a[0] = Rnd.Get(300 / A[0]);
                    int max = a[0];
                    int maxSize = A[0];
                    int minSize = A[0];
                    long sum = (A[0] * A[0] - 3 * A[0] * x) * a[0];
                    long popBig = A[0] * a[0];
                    long popSmall = 0;
                    int cntBig = a[0];
                    int cntSmall = 0;
                    long prodBig = A[0] * A[0] * a[0];
                    long prodSmall = 0;
                    int gap = 0;
                    int maxGap = 0;
                    for (int i = 1; i < range && sum >= 0; i++)
                    {
                        long k = A[i] * A[i] - 3 * A[i] * x;
                        long add = 0;
                        int cel = 300 / A[i];
                        if (k < 0)
                            cel = (int)(sum / (-k));
                        do
                        {
                            if (Rnd.Get(3) >= 1 || max > cel)
                            {
                                a[i] = 0;
                            }
                            else
                            {
                                a[i] = max + Rnd.Get(cel-max);
                                max = a[i];
                            }
                            add = k * a[i];
                        }
                        while (sum + add < 0);
                        sum += add;

                        gap++;
                        if (a[i] != 0)
                        {
                            if (A[i] > maxSize)
                                maxSize = A[i];
                            if (A[i] < minSize)
                                minSize = A[i];
                            if (gap > maxGap)
                                maxGap = gap;
                            gap = 0;
                        }


                        if (k > 0)
                        {
                            popBig += A[i] * a[i];
                            prodBig += A[i] * A[i] * a[i];
                            cntBig += a[i];
                        }
                        else
                        {
                            popSmall += A[i] * a[i];
                            prodSmall += A[i] * A[i] * a[i];
                            cntSmall += a[i];
                        }
                    }

                    counter2++;

                    //long ProdY = Calk(A, a, x);
                    long pop = popBig + popSmall;
                    long colProd = pop * x + sum / 3;
                    if (prodSmall > colProd &&
                        prodSmall*3 < colProd*4 &&
                        maxGap < range / 3 && 
                        maxSize > minSize*2 && 
                        //pop > 2500 && 
                        //pop < 3500 && 
                        cntBig + cntSmall > 250 &&
                        cntBig + cntSmall < 350 &&
                        sum / 3 > popBig / 2 &&
                        sum / 3 < popBig)
                    {
                        long rem = 0;
                        Math.DivRem(sum, 3, out rem);
                        if (rem == 0)
                        {
                            string s = string.Format("Pop {0}/{1} Count {2}/{3} Prod {4}/{5} ({6} + {7} + {8}): ", popBig, popSmall, cntBig, cntSmall, prodBig, prodSmall, pop*x, sum/3, prodSmall - pop*x - sum/3);
                            for (int j = 0; j < range; j++)
                            {
                                long k = A[j] * A[j] - 3 * A[j] * x;
                                if (a[j] > 0)
                                {
                                    if(k > 0)
                                        s += string.Format(" +{0}x{1}", A[j], a[j]);
                                    else
                                        s += string.Format(" -{0}x{1}", A[j], a[j]);
                                }
                            }
//                            s += string.Format(" + {0}", sum / 3);
                            worker.ReportProgress(counter++, s);
                        }
                        //                        box1.Add(s);
                    }
                }
                while (rr < range && counter < 10 && counter2 < 30000);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label1.Text = e.ProgressPercentage.ToString();
            if (e.UserState != null)
                listBox1.Items.Add(e.UserState.ToString());
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.Show();
        }
    }
}
