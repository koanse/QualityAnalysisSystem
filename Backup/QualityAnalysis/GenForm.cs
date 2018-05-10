using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QualityAnalysis
{
    public partial class GenForm : Form
    {
        double[,] matrX, matrY;
        double[] arrXAv;
        string[] arrXName;
        int[] arrI;
        public double[,] matrXM, matrYM;
        public string rep;
        public GenForm(double[,] matrX, double[,] matrY, double[] arrXAv, string[] arrXName)
        {
            InitializeComponent();
            this.matrX = matrX;
            this.matrY = matrY;
            this.arrXAv = arrXAv;
            this.arrXName = arrXName;
            for (int i = 0; i < arrXName.Length; i++)
                dgv.Rows.Add(new object[] { arrXName[i], "x" });
        }
        void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                Sample[] arrSX = new Sample[matrX.GetLength(1)];
                for (int j = 0; j < matrX.GetLength(1); j++)
                {
                    double[] arrX = new double[matrX.GetLength(0)];
                    for (int i = 0; i < matrX.GetLength(0); i++)
                        arrX[i] = matrX[i, j];
                    arrSX[j] = new TranSample(new Sample(arrXName[j],
                        string.Format("x<sub>{0}</sub>", j + 1), arrX),
                        dgv.Rows[j].Cells[1].Value.ToString());
                }
                Regression[] arrReg = new Regression[matrY.GetLength(1)];
                rep = "ПАРАМЕТРИЧЕСКАЯ ИДЕНТИФИКАЦИЯ<br>";
                for (int j = 0; j < matrY.GetLength(1); j++)
                {
                    double[] arrY = new double[matrY.GetLength(0)];
                    for (int i = 0; i < matrY.GetLength(0); i++)
                        arrY[i] = matrY[i, j];
                    arrReg[j] = new Regression(new Sample("y", string.Format("y<sub>{0}</sub>", j + 1), arrY), arrSX);
                    rep += arrReg[j].GetRegReport() + "<br>";
                }                
                Quality.GenErr(matrX, matrY, arrReg, int.Parse(tbN.Text), double.Parse(tbA.Text),
                    double.Parse(tbB.Text), out arrI, out matrXM, out matrYM);
                rep += string.Format("ПАРАМЕТРЫ ГЕНЕРАЦИИ<br>n = {0}<br>A = {1}<br>B = {2}<br><br>",
                    tbN.Text, tbA.Text, tbB.Text);
                rep += "ПЕРЕЧЕНЬ ИЗМЕНЕННЫХ НАБЛЮДЕНИЙ<br>";
                foreach (int i in arrI)
                    rep += string.Format("{0} ", i + 1);
                WBForm wbf = new WBForm("Параметрическая идентификация и генерация наблюдений", rep);
                DialogResult = DialogResult.OK;
                Close();
                wbf.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Ошибка генерации");
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}