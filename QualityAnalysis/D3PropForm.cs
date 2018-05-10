using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QualityDim
{
    public partial class D3PropForm : Form
    {
        public double[,] matrMin, matrMax;
        public bool[] arrTexture;
        public D3PropForm()
        {
            InitializeComponent();
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                matrMin = new double[dgv.RowCount - 1, 3];
                matrMax = new double[dgv.RowCount - 1, 3];
                arrTexture = new bool[dgv.RowCount - 1];
                for (int i = 0; i < dgv.RowCount - 1; i++)
                {
                    matrMin[i, 1] = double.Parse(dgv[0, i].Value.ToString());
                    matrMax[i, 1] = double.Parse(dgv[1, i].Value.ToString());
                    matrMin[i, 0] = double.Parse(dgv[2, i].Value.ToString());
                    matrMax[i, 0] = double.Parse(dgv[3, i].Value.ToString());
                    matrMin[i, 2] = double.Parse(dgv[4, i].Value.ToString());
                    matrMax[i, 2] = double.Parse(dgv[5, i].Value.ToString());
                    if (dgv[6, i].Value == null)
                        arrTexture[i] = false;
                    else
                        arrTexture[i] = (bool)dgv[6, i].Value;
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch
            {
                MessageBox.Show("Ошибка ввода");
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
