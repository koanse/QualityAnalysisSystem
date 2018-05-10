using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QualityDim
{
    public partial class H3DPropForm : Form
    {
         public H3DPropForm()
        {
            InitializeComponent();
            tbY1Max.Text = tbY1Min.Text = tbY2Max.Text = tbY2Min.Text = tbY3Max.Text = tbY3Min.Text = "0";
        }
        void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
        void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
