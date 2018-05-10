using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QualityDim
{
    public partial class ClustForm : Form
    {
        public double[][] matrLL;
        public ClustForm(string[] arrYName)
        {
            InitializeComponent();
            if (arrYName.Length == Properties.Settings.Default.matrLL.Count)
            {
                for (int i = 0; i < arrYName.Length; i++)
                {
                    dgv.Rows.Add(arrYName[i], Properties.Settings.Default.matrLL[i]);
                }
            }
            else
            {
                for (int i = 0; i < arrYName.Length; i++)
                {
                    dgv.Rows.Add(arrYName[i], "1 1 1 1 1");
                }
            }
        }
        void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                matrLL = new double[dgv.RowCount][];
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    string[] arrStr = dgv[1, i].Value.ToString().Split(new char[] { ' ' },
                        StringSplitOptions.RemoveEmptyEntries);
                    matrLL[i] = new double[arrStr.Length];
                    for (int j = 0; j < arrStr.Length; j++)
                    {
                        matrLL[i][j] = double.Parse(arrStr[j]);
                    }
                }
                Properties.Settings.Default.matrLL.Clear();
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    Properties.Settings.Default.matrLL.Add(dgv[1, i].Value.ToString());
                }
                Properties.Settings.Default.Save();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch
            {
                MessageBox.Show("Ошибка в данных");
            }
        }
        void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}