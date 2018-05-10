using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QualityDim
{
    public partial class QMinMaxForm : Form
    {
        public double[] arrYMin, arrYMax;
        public QMinMaxForm(string[] arrYName)
        {
            InitializeComponent();
            if (arrYName.Length == Properties.Settings.Default.arrMin.Count)
            {
                for (int i = 0; i < arrYName.Length; i++)
                {
                    dgv.Rows.Add(arrYName[i], Properties.Settings.Default.arrMin[i], Properties.Settings.Default.arrMax[i]);
                }
            }
            else
            {
                for (int i = 0; i < arrYName.Length; i++)
                {
                    dgv.Rows.Add(arrYName[i], "0", "100");
                }
            }
            arrYMax = new double[arrYName.Length];
            arrYMin = new double[arrYName.Length];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string[] arrSYMin = new string[arrYMin.Length], arrSYMax = new string[arrYMin.Length];
                for (int i = 0; i < arrYMin.Length; i++)
                {
                    arrYMin[i] = double.Parse(dgv[1, i].Value.ToString());
                    arrYMax[i] = double.Parse(dgv[2, i].Value.ToString());
                }
                Properties.Settings.Default.arrMin.Clear();
                Properties.Settings.Default.arrMax.Clear();
                for (int i = 0; i < arrYMin.Length; i++)
                {
                    Properties.Settings.Default.arrMin.Add(dgv[1, i].Value.ToString());
                    Properties.Settings.Default.arrMax.Add(dgv[2, i].Value.ToString());
                }
                Properties.Settings.Default.Save();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch
            {
                MessageBox.Show("Ошибка ввода");
            }            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
