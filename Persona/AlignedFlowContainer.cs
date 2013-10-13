using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms.Layout;
using System.Windows.Forms;
using System.Drawing;

namespace Persona
{
    public partial class AlignedFlowContainer : Panel
    {
        // This class demonstrates a simple custom layout engine.
        public class ActionsFlowLayout : LayoutEngine
        {
            public override bool Layout(
                object container,
                LayoutEventArgs layoutEventArgs)
            {
                Control parent = container as Control;

                // Use DisplayRectangle so that parent.Padding is honored.
                Rectangle parentDisplayRectangle = parent.DisplayRectangle;

                int iTotalHeight = 0;

                foreach (Control c in parent.Controls)
                {
                    // Only apply layout to visible controls.
                    if (!c.Visible)
                    {
                        continue;
                    }

                    // Set the autosized controls to their 
                    // autosized heights.
                    if (c.AutoSize)
                    {
                        c.Size = c.GetPreferredSize(parentDisplayRectangle.Size);
                    }

                    iTotalHeight += c.Size.Height + c.Margin.Top + c.Margin.Bottom;
                }

                parentDisplayRectangle.Y += (parentDisplayRectangle.Height - iTotalHeight) / 2;

                Point nextControlLocation = parentDisplayRectangle.Location;

                foreach (Control c in parent.Controls)
                {
                    // Only apply layout to visible controls.
                    if (!c.Visible)
                    {
                        continue;
                    }

                    // Respect the margin of the control:
                    // shift over the left and the top.
                    nextControlLocation.Offset(c.Margin.Left, c.Margin.Top);

                    // Set the location of the control.
                    c.Location = nextControlLocation;

                    // Set the autosized controls to their 
                    // autosized heights.
                    if (c.AutoSize)
                    {
                        c.Size = c.GetPreferredSize(parentDisplayRectangle.Size);
                    }

                    nextControlLocation.Offset((parentDisplayRectangle.Size.Width - c.Size.Width - c.Margin.Left - c.Margin.Right) / 2, 0);
                    c.Location = nextControlLocation;

                    // Move X back to the display rectangle origin.
                    nextControlLocation.X = parentDisplayRectangle.X;

                    // Increment Y by the height of the control 
                    // and the bottom margin.
                    nextControlLocation.Y += c.Height + c.Margin.Bottom;
                }

                // Optional: Return whether or not the container's 
                // parent should perform layout as a result of this 
                // layout. Some layout engines return the value of 
                // the container's AutoSize property.

                return false;
            }
        }

        private ActionsFlowLayout layoutEngine;
        
        public AlignedFlowContainer()
        {
            InitializeComponent();
        }

        public override LayoutEngine LayoutEngine
        {
            get
            {
                if (layoutEngine == null)
                {
                    layoutEngine = new ActionsFlowLayout();
                }

                return layoutEngine;
            }
        }
    }
}
