using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Parameters;
using Persona.Collections;

namespace Persona.Core.Controls
{
    public partial class DescriptionEdit : UserControl
    {
        public DescriptionEdit()
        {
            InitializeComponent();
        }

        public string Text
        {
            set { textBox2.Text = value; }
            get { return textBox2.Text; }
        }

        public void Bind(Module pModule)
        {
            InsertStringToolStripMenuItem.DropDownItems.Clear();
            foreach (StringParameter pParam in pModule.m_cStringParameters)
            {
                ToolStripMenuItem pItem = new ToolStripMenuItem(pParam.ToString());
                pItem.Tag = pParam;
                pItem.Click += new EventHandler(InsertString_Click);
                InsertStringToolStripMenuItem.DropDownItems.Add(pItem);
            }

            InsertNumericToolStripMenuItem.DropDownItems.Clear();
            foreach (NumericParameter pParam in pModule.m_cNumericParameters)
            {
                ToolStripMenuItem pItem = new ToolStripMenuItem(pParam.ToString());
                pItem.Tag = pParam;
                pItem.Click += new EventHandler(InsertNumber_Click);
                InsertNumericToolStripMenuItem.DropDownItems.Add(pItem);
            }

            foreach (ObjectsCollection pColl in pModule.m_cCollections)
            {
                foreach (NumericParameter pParam in pColl.m_cNumericParameters)
                {
                    ToolStripMenuItem pItem = new ToolStripMenuItem(pParam.ToString());
                    pItem.Tag = pParam;
                    pItem.Click += new EventHandler(InsertNumber_Click);
                    InsertNumericToolStripMenuItem.DropDownItems.Add(pItem);
                }
                foreach (StringParameter pParam in pColl.m_cStringParameters)
                {
                    ToolStripMenuItem pItem = new ToolStripMenuItem(pParam.ToString());
                    pItem.Tag = pParam;
                    pItem.Click += new EventHandler(InsertString_Click);
                    InsertStringToolStripMenuItem.DropDownItems.Add(pItem);
                }
            }
        }

        void InsertString_Click(object sender, EventArgs e)
        {

        }

        void InsertNumber_Click(object sender, EventArgs e)
        {

        }

    }
}
