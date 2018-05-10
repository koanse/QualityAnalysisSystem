using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QualityAnalysis
{
    public partial class MainForm : Form
    {
        double[,] matrX, matrY;
        string[] arrXName, arrYName;
        double[] arrYMin, arrYMax, arrYAv, arrXAv;
        double[][] matrLL, matrL;
        string clusters;
        string[] arrRep;
        public MainForm()
        {
            InitializeComponent();
        }
        void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }        
        void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportForm form = new ImportForm();
            if (form.ShowDialog() != DialogResult.OK)
                return;
            matrX = form.matrX;
            matrY = form.matrY;
            arrXName = form.arrXName;
            arrYName = form.arrYName;
            arrYMin = form.arrYMin;
            arrYMax = form.arrYMax;
            arrXAv = form.arrXAv;
            arrYAv = form.arrYAv;
            ClastForm cf = new ClastForm(arrYName);
            if (cf.ShowDialog() != DialogResult.OK)
                return;
            matrLL = cf.matrLL;
            clusters = cf.tbC.Text;
            arrRep = new string[11];
            arrRep[0] = Quality.DataTable2(arrXName, arrYName, matrX, matrY);
            wb.DocumentText = arrRep[0];
        }
        void genToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GenForm form = new GenForm(matrX, matrY, arrXAv, arrXName);
                if (form.ShowDialog() != DialogResult.OK)
                    return;
                matrX = form.matrXM;
                matrY = form.matrYM;
                string[] arrG, arrGU;
                int[][] arrArrI;
                int N, N1;
                Quality.SetID(matrY, arrYMin, arrYMax, out arrG, out arrGU, out arrArrI, out N, out N1);
                arrRep[0] = Quality.DataTable(arrXName, arrYName, matrX, matrY, arrG);
                arrRep[1] += form.rep + "<br><br>";
                wb.DocumentText = arrRep[0];
            }
            catch { }
        }
        void lb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                wb.DocumentText = arrRep[lb.SelectedIndex];
            }
            catch { }
        }
        void analizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int N;
                int[] arrF;
                double[] arrP, arrPC;
                double[][] matrL;
                string[] arrC;
                int[][] arrArrIC;
                double H;
                Quality.Clast(matrY, matrLL, arrYMin, arrYMax, clusters,
                    out matrL, out N, out arrF, out arrP, out arrC, out arrArrIC, out arrPC, out H);
                arrRep[2] = string.Format("Общее количество наблюдений: {0}<br>" +
                    "Количество наблюдений, попавших в рассматриваемую область качества: {1}<br>",
                    matrX.GetLength(0), N);
                arrRep[3] = Quality.CTable(arrYName, matrL, arrYMin, arrYMax, arrF, arrP);                
                arrRep[4] = Quality.RTable(arrC, arrPC);
                arrRep[5] = string.Format("H = {0:F3}", H);

                /*string[] arrG, arrGU;
                int[][] arrArrI;
                int N, N1;
                Quality.SetID(matrY, arrYMin, arrYMax, out arrG, out arrGU, out arrArrI, out N, out N1);
                int[] arrIS, arrFreq;
                string[] arrGS;
                int[][] arrArrIS;
                Quality.Sort(arrGU, arrArrI, out arrIS, out arrFreq, out arrGS, out arrArrIS);
                string[] arrGC;
                double[] arrP, arrP1, arrPM;
                int[] arrGFreq;
                double H;
                Quality.Clustering(arrGS, arrArrIS, N, N1, out arrGC, out arrGFreq, out arrP, out arrP1, out arrPM, out H);
                //arrRep[0] = Quality.DataTable(arrXName, arrYName, matrX, matrY, arrG);
                arrRep[2] = string.Format("Общее количество наблюдений: {0}<br>" +
                    "Количество качественных экземпляров: {1}<br>" +
                    "Количество бракованных экземпляров: {2}", N, N - N1, N1);
                arrRep[3] = Quality.GTable(arrGU, arrArrI);
                arrRep[4] = Quality.ITable(arrIS, arrYName, arrFreq);
                arrRep[5] = Quality.GTable(arrGS, arrArrIS);
                arrRep[6] = Quality.PTable(arrGC, arrGFreq, arrP, arrP1, arrPM);
                arrRep[7] = Quality.ID2dTable(arrGC, arrP);
                arrRep[8] = Quality.ID2dTable(arrGC, arrP1);
                arrRep[9] = Quality.ID2dTable(arrGC, arrPM);
                arrRep[10] = string.Format("H = {0:F3}", H);*/
            }
            catch
            {
                MessageBox.Show("Ошибка анализа");
            }
        }
    }
}