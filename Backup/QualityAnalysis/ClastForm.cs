using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QualityAnalysis
{
    public partial class ClastForm : Form
    {
        public double[][] matrLL;
        public ClastForm(string[] arrYName)
        {
            InitializeComponent();
            for (int i = 0; i < arrYName.Length; i++)
                dgv.Columns.Add(i.ToString(), arrYName[i]);            
        }
        void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                ReadLL();
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
        void btnAuto_Click(object sender, EventArgs e)
        {
            try
            {
                ReadLL();
                string s = "";
                int mult = 1;
                for (int i = 0; i < matrLL.Length; i++)
                    mult *= matrLL[i].Length;
                for (int i = 0; i < mult; i++)
                    s += i.ToString() + "\r\n";
                tbC.Text = s;
            }
            catch
            {
                MessageBox.Show("Ошибка в данных");
            }
        }
        void ReadLL()
        {
            matrLL = new double[dgv.Columns.Count][];
            for (int j = 0; j < matrLL.Length; j++)
            {
                List<double> ld = new List<double>();
                for (int i = 0; i < dgv.Rows.Count; i++)                
                {
                    if (dgv[j, i].Value == null ||
                        dgv[j, i].Value.ToString() == "")
                        break;
                    ld.Add(double.Parse(dgv[j, i].Value.ToString()));
                }
                if (ld.Count == 0)
                    throw new Exception();
                matrLL[j] = ld.ToArray();
            }
        }
    }
}