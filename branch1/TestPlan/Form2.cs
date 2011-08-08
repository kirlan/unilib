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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private class Combination
        { 
            public long[] Group = new long[23];

            public Combination(int sum)
            {
                for (int i = 0; i < 23; i++)
                    Group[i] = 0;

                for (int k = 0; k < 22 && sum > 0; k++)
                {
                    Group[k] = Rnd.Get(sum/2);
                    sum -= (int)Group[k];
                }
                Group[22] = sum;
            }

            public Combination(Combination copy)
            {
                for (int i = 0; i < 23; i++)
                    Group[i] = copy.Group[i];
            }

            public void Add(long A, Combination add)
            {
                for (int i = 0; i < 23; i++)
                    Group[i] += A*add.Group[i];
            }

            public int BuildMaxPossible(int min, long max, Combination sum)
            {
                Group[0] = min;
                Group[1] = min;
                Group[2] = min;
                Group[3] = min;

                long tt = (max - min * 4)/14;
                Group[4] = 5*tt;
                Group[5] = 3*tt;
                Group[6] = 2*tt;
                Group[7] = tt;
                Group[8] = tt;
                Group[9] = tt;
                Group[10] = max - min * 4 - Group[4] - Group[5] - Group[6] - Group[7] - Group[8] - Group[9];

                Group[11] = Group[0] + Group[4];
                Group[12] = Group[1] + Group[5];
                Group[13] = Group[2] + Group[6];
                Group[14] = Group[3] + Group[7];
                Group[15] = Group[8];
                Group[16] = Group[9];
                Group[17] = Group[10];

                Group[18] = Group[11] + Group[12];
                Group[19] = Group[13] + Group[14];
                Group[20] = Group[15];
                Group[21] = Group[16];
                Group[22] = Group[17];

                long maxDiff = 0;
                int maxDiffIndex = -1;

                for (int i = 0; i < 23; i++)
                {
                    if (sum.Group[i] - Group[i] > maxDiff)
                    {
                        maxDiff = sum.Group[i] - Group[i];
                        maxDiffIndex = i;
                    }
                }

                return maxDiffIndex;
            }

            public bool IsPossible()
            {
                return true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int[] A = new int[10];
            int[] a = new int[10];

            for (int i = 9; i >= 0; i--)
            {
                if (i == 9)
                    A[i] = 2 + Rnd.Get(8);
                else
                    A[i] = A[i + 1] + 1 + Rnd.Get(Math.Min(8, 50-A[i+1]));
            }

            int cnt = 0;
            long total = 0;
            for (int i = 0; i < 10; i++)
            {
                A[i] = A[i] * A[i];
                int add = Rnd.Get((300 - cnt) / (10 - i));
                if (i == 9)
                    add = 300 - cnt;

                if (i == 0)
                    a[i] = 1 + add;
                else
                {
                    a[i] = a[i - 1] + add;
                }

                cnt += add*(10-i);
                total += A[i] * a[i];
            }

            int min = (int)(total / 16);

            Combination[] arr = new Combination[10];
            Combination cSum = new Combination(0);
            Combination cMax = new Combination(0);
            for (int i = 0; i < 10; i++)
            {
                arr[i] = new Combination(a[i]);
                cSum.Add(A[i], arr[i]);
            }

            int maxIndex = cMax.BuildMaxPossible(min, total / 3, cSum);
            while (maxIndex != -1)
            { 
                long maxDiff = cSum.Group[maxIndex] - cMax.Group[maxIndex];

                long minDiff = 0;
                int minDiffIndex = -1;

                for (int i = 0; i < 23; i++)
                {
                    if (cMax.Group[i] - cSum.Group[i] > minDiff)
                    {
                        minDiff = cMax.Group[i] - cSum.Group[i];
                        minDiffIndex = i;
                    }
                }

                int moveIndex = -1;
                int count;
                long trueMin;
                if (minDiffIndex != -1)
                {
                    trueMin = Math.Min(maxDiff, minDiff);
                    for (int i = 0; i < 10; i++)
                    {
                        if (arr[i].Group[maxIndex] > 0 && A[i] <= trueMin)
                        {
                            moveIndex = i;
                            break;
                        }
                    }
                    if (moveIndex == -1)
                    {
                        for (int i = 0; i < 10 && moveIndex == -1; i++)
                        {
                            if (A[i] <= trueMin)
                            {
                                for (int j = 0; j < 23; j++)
                                    if (cSum.Group[j] - cMax.Group[j] >= A[i] && arr[i].Group[j] > 0)
                                    {
                                        moveIndex = i;
                                        maxIndex = j;
                                        trueMin = Math.Min(trueMin, cSum.Group[j] - cMax.Group[j]);
                                        break;
                                    }
                            }
                        }
                    }
                    if (moveIndex != -1)
                    {
                        count = Math.Min((int)arr[moveIndex].Group[maxIndex], (int)(trueMin / A[moveIndex]));

                        arr[moveIndex].Group[maxIndex] -= count;
                        arr[moveIndex].Group[minDiffIndex] += count;
                    }
                }
                cSum = new Combination(0);
                for (int i = 0; i < 10; i++)
                {
                    cSum.Add(A[i], arr[i]);
                }
                maxIndex = cMax.BuildMaxPossible(min, total / 3, cSum);
            }
            maxIndex = cMax.BuildMaxPossible(min, total / 3, cSum);
        }
    }
}
