using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Security.Permissions;
using System.ComponentModel;
using System.Windows.Forms.Design;
using PresentationControls;
using System.Windows.Forms;

namespace nsUniLibControls
{
    public interface IEnumCollection
    {
        Array AllEnumValues { get; }
        bool IsSelected(object value);
        void Select(object value, bool check);
    }

    // This UITypeEditor can be associated with any IEnumCollection
    // properties to provide a design-mode checked combobox interface.
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")] 
    public class EnumCollectionEditor : UITypeEditor
    {
        public EnumCollectionEditor()
        {

        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog, 
        // drop down dialog, or no UI outside of the properties window.
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        // Displays the UI for value selection.
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {            
            // Return the value if the value is not of type Int32, Double and Single.
            if (!(value is IEnumCollection))
                return value;

            IEnumCollection pEnumCollection = value as IEnumCollection;

            // Uses the IWindowsFormsEditorService to display a 
            // drop-down UI in the Properties window.
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if( edSvc != null )
            {
                // Display an angle selection control and retrieve the value.
                CheckedListBox pCBCControl = new CheckedListBox();
                pCBCControl.CheckOnClick = true;
                foreach (object eEnumValue in pEnumCollection.AllEnumValues)
                {
                    int iIndex = pCBCControl.Items.Add(eEnumValue);
                    pCBCControl.SetItemChecked(iIndex, pEnumCollection.IsSelected(eEnumValue));
                }

                edSvc.DropDownControl(pCBCControl);
                
                foreach (object eEnumValue in pEnumCollection.AllEnumValues)
                {
                    int iIndex = pCBCControl.Items.IndexOf(eEnumValue);
                    pEnumCollection.Select(eEnumValue, pCBCControl.GetItemChecked(iIndex));
                }

                // Return the value in the appropraite data format.
            }
            return value;
        }

        // Indicates whether the UITypeEditor supports painting a 
        // representation of a property's value.
        //public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
        //{
        //    return false;
        //}
    }
}
