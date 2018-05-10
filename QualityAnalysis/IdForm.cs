using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QualityDim
{
    public partial class IdForm : Form
    {
        public double[] arrU0Min, arrU0Max, arrDU;
        public int[] arrYMinNum, arrYMaxNum;
        public int maxIter;
        public double DQ;
        double[] arrXAv;
        public IdForm(string[] arrXName, string[] arrYName, double[,] matrX, double[] arrXAv)
        {
            InitializeComponent();
            this.arrXAv = arrXAv;
            for (int i = 0; i < arrXName.Length; i++)
            {
                double min = double.MaxValue, max = double.MinValue;
                for (int j = 0; j < matrX.GetLength(0); j++)
                {
                    if (matrX[j, i] < min)
                        min = matrX[j, i];
                    if (matrX[j, i] > max)
                        max = matrX[j, i];

                }
                dgvU.Rows.Add(arrXName[i], Math.Round(min * arrXAv[i], 4), Math.Round(max * arrXAv[i], 4),
                    (max - min) / 50 * arrXAv[i]);
            }
            for (int i = 0; i < arrYName.Length; i++)
                dgvY.Rows.Add(arrYName[i], 0, 1);
            tbMI.Text = "5000";
            tbDQ.Text = "0";
        }
        void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                arrU0Min = new double[dgvU.Rows.Count];
                arrU0Max = new double[dgvU.Rows.Count];
                arrDU = new double[dgvU.Rows.Count];
                for (int i = 0; i < dgvU.Rows.Count; i++)
                {
                    arrU0Min[i] = double.Parse(dgvU[1, i].Value.ToString()) / arrXAv[i];
                    arrU0Max[i] = double.Parse(dgvU[2, i].Value.ToString()) / arrXAv[i];
                    arrDU[i] = double.Parse(dgvU[3, i].Value.ToString()) / arrXAv[i];
                }
                arrYMinNum = new int[dgvY.Rows.Count];
                arrYMaxNum = new int[dgvY.Rows.Count];
                for (int i = 0; i < dgvY.Rows.Count; i++)
                {
                    arrYMinNum[i] = int.Parse(dgvY[1, i].Value.ToString());
                    arrYMaxNum[i] = int.Parse(dgvY[2, i].Value.ToString());
                }
                maxIter = int.Parse(tbMI.Text);
                DQ = double.Parse(tbDQ.Text);
            }
            catch
            {
                MessageBox.Show("Ошибка ввода");
            }
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
