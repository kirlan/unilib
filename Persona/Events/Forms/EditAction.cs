using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Persona
{
    public partial class EditAction : Form
    {
        public Action m_pAction;

        public EditAction(Action pAction)
        {
            InitializeComponent();

            m_pAction = pAction;

            textBox1.Text = pAction.m_sName;

            imageList1.Images.Clear();
            imageList1.ImageSize = new Size(40, 40);

            Bitmap pBitmap = new Bitmap(pictureBox2.Image);

            PixelFormat format = pBitmap.PixelFormat;

            for (int y = 0; y < pBitmap.Height - 40; y += 40)
                for (int x = 0; x < pBitmap.Width - 40; x += 40)
                {
                    Rectangle cloneRect = new Rectangle(x, y, 40, 40);

                    Bitmap pTile = pBitmap.Clone(cloneRect, format);

                    imageList1.Images.Add(pTile);

                    comboBox1.Items.Add(pTile);
                }

            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pAction.m_sName = textBox1.Text;

            DialogResult = DialogResult.OK;
        }

        private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if(e.Index != -1)
                e.Graphics.DrawImage(comboBox1.Items[e.Index] as Bitmap, e.Bounds);

            e.DrawFocusRectangle();
        }
    }
}
