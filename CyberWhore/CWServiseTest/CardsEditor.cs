using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CWServiseTest
{
    public partial class CardsEditor : Form
    {
        public CardsEditor()
        {
            InitializeComponent();

            listBox1.Items.Clear();
            listBox1.Items.Add(new DemandCard());

            propertyGrid1.SelectedObject = listBox1.Items[0];
        }
    }
}
