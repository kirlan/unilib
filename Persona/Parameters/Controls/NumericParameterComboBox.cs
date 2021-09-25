using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Collections;
using Persona.Parameters;

namespace Persona.Core.Controls
{
    public partial class NumericParameterComboBox : UserControl
    {
        public NumericParameterComboBox()
        {
            InitializeComponent();
        }

        //http://stackoverflow.com/questions/8158419/cannot-add-control-to-form
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Parameter Parameter
        {
            set 
            {
                if (!(value is NumericParameter))
                    throw new ArgumentException("Not NumericParameter!");

                comboBox1.SelectedItem = value;
            }
            get
            {
                return comboBox1.SelectedItem as NumericParameter;
            }
        }

        public event EventHandler ParameterChanged;

        public void Bind(Parameter pParameter)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add(pParameter);
        }

        public void Bind(Module pModule)
        {
            Bind(pModule, false);
        }

        public void Bind(Module pModule, bool bWritableOnly)
        {
            comboBox1.Items.Clear();
            if (bWritableOnly)
            {
                foreach (NumericParameter pParam in pModule.m_cNumericParameters)
                    if (pParam.m_pFunction == null)
                        comboBox1.Items.Add(pParam);
            }
            else
            {
                comboBox1.Items.AddRange(pModule.m_cNumericParameters.ToArray());
                foreach (ObjectsCollection pColl in pModule.m_cCollections)
                    comboBox1.Items.AddRange(pColl.m_cNumericParameters.ToArray());
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            EventHandler handler = ParameterChanged;
            if (handler != null)
            {
                // Invokes the delegates.
                handler(this, e);
            }
        }
    }
}
