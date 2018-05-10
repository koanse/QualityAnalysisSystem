using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QualityDim
{
    public partial class MainForm : Form
    {
        double[,] matrX, matrY;
        string[] arrXName, arrYName;
        double[] arrYMin, arrYMax, arrYAv, arrXAv, arrP;
        double[][] matrLL, matrL;
        string[] arrReport;
        public MainForm()
        {
            InitializeComponent();
            histToolStripMenuItem.Visible = false;
            hist3dToolStripMenuItem.Visible = false;
        }
        void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }        
        void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ImportForm form = new ImportForm();
                if (form.ShowDialog() != DialogResult.OK)
                    return;
                matrX = form.matrX;
                matrY = form.matrY;
                arrXName = form.arrXName;
                arrYName = form.arrYName;
                arrXAv = form.arrXAv;
                arrYAv = form.arrYAv;
                QMinMaxForm qf = new QMinMaxForm(arrYName);
                if (qf.ShowDialog() != DialogResult.OK)
                    return;
                arrYMin = qf.arrYMin;
                arrYMax = qf.arrYMax;
                for (int i = 0; i < arrYMin.Length; i++)
                {
                    arrYMin[i] /= arrYAv[i];
                    arrYMax[i] /= arrYAv[i];
                }
                ClustForm cf = new ClustForm(arrYName);
                if (cf.ShowDialog() != DialogResult.OK)
                    return;
                matrLL = cf.matrLL;
                arrReport = new string[14];
                arrReport[0] = "Наблюдения (в скобках указаны ненормированные значения)" +
                    Quality.DataTable2(arrXName, arrYName, matrX, matrY, arrXAv, arrYAv);
                arrReport[8] = "";
                wb.DocumentText = arrReport[0];
                if (arrYName.Length == 2)
                {
                    histToolStripMenuItem.Visible = true;
                    hist3dToolStripMenuItem.Visible = false;
                }
                else if (arrYName.Length == 3)
                {
                    histToolStripMenuItem.Visible = false;
                    hist3dToolStripMenuItem.Visible = true;
                }
                else
                {
                    histToolStripMenuItem.Visible = false;
                    hist3dToolStripMenuItem.Visible = false;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка импорта");
                return;
            }
            try
            {
                int[] arrF;
                int N;
                Quality.Clust(matrY, matrLL, arrYMin, arrYMax, out matrL, out arrF, out arrP, out N);
                arrReport[9] = string.Format("Общее количество наблюдений: {0}<br>" +
                    "Количество наблюдений, попавших в рассматриваемую область качества: {1}<br>",
                    matrX.GetLength(0), N);
                arrReport[10] = "Эмпирические вероятности попадания в кластеры (в скобках /.../ - ненормированные границы кластеров)" +
                    Quality.CTable(arrYName, matrL, arrYMin, arrYMax, arrF, arrP, arrYAv);
            }
            catch
            {
                MessageBox.Show("Ошибка кластеризации");
            }
        }
        void genToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GenForm form = new GenForm(matrX, matrY, arrXAv, arrXName, arrYName);
                if (form.ShowDialog() != DialogResult.OK)
                    return;
                matrX = form.matrXM;
                matrY = form.matrYM;
                arrReport[0] = "Наблюдения (в скобках указаны ненормированные значения)" +
                    Quality.DataTable2(arrXName, arrYName, matrX, matrY, arrXAv, arrYAv);
                arrReport[1] = form.repSmp;
                arrReport[2] = form.repHSmp;
                arrReport[3] = form.repId;
                arrReport[4] = form.repHId;
                arrReport[5] = form.repCorrXY;
                arrReport[6] = form.repCorr;
                arrReport[7] = form.repHCorr;
                arrReport[8] += form.repGen;
                wb.DocumentText = arrReport[0];
            }
            catch
            {
                MessageBox.Show("Ошибка создания модели");
            }
        }
        void lb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (arrReport != null && arrReport[lb.SelectedIndex] != "" && arrReport[lb.SelectedIndex] != null)
                    wb.DocumentText = arrReport[lb.SelectedIndex];
                else
                {
                    if (new List<int>(new int[] { 0, 9, 10 }).Contains(lb.SelectedIndex))
                        wb.DocumentText = "Загрузите исходные данные (\"Файл\", \"Открыть\")";
                    else if (new List<int>(new int[] { 1, 2,3,4,5,6,7,8 } ).Contains(lb.SelectedIndex))
                        wb.DocumentText = "Для получения информации выберете \"Анализ\", \"Модель технологии\" ";
                    else if (new List<int>(new int[] { 11, 12 } ).Contains(lb.SelectedIndex))
                        wb.DocumentText = "Для получения информации выберете \"Анализ\", \"Анализ качества для реплик\"";
                    else if (lb.SelectedIndex == 13)
                        wb.DocumentText = "Для получения информации выберете \"Анализ\", \"Идентификация рамочной технологии\" ";
                }
            }
            catch { }
        }
        void analizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int N;
                int[] arrF;
                double[] arrPC;
                string[] arrC;
                int[][] arrArrIC;
                double H;
                ReplForm rf = new ReplForm();
                if (rf.ShowDialog() != DialogResult.OK)
                    return;
                Quality.ClustRepl(matrY, matrLL, arrYMin, arrYMax, rf.arrRepl,
                    out matrL, out N, out arrF, out arrP, out arrC, out arrArrIC, out arrPC, out H);
                arrReport[9] = string.Format("Общее количество наблюдений: {0}<br>" +
                    "Количество наблюдений, попавших в рассматриваемую область качества: {1}<br>",
                    matrX.GetLength(0), N);
                arrReport[10] = Quality.CTable(arrYName, matrL, arrYMin, arrYMax, arrF, arrP, arrYAv);                
                arrReport[11] = Quality.RTable(arrC, arrPC);
                arrReport[12] = string.Format("H = {0:F3}", H);
            }
            catch
            {
                MessageBox.Show("Ошибка анализа");
            }
        }

        void identToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[] arrUMin, arrUMax;
            double Q0, Q;
            IdForm idf;
            try
            {
                idf = new IdForm(arrXName, arrYName, matrX, arrXAv);
                if (idf.ShowDialog() != DialogResult.OK)
                    return;
                double[] arrYMin2 = new double[arrYName.Length], arrYMax2 = new double[arrYName.Length];
                for (int i = 0; i < arrYName.Length; i++)
                {
                    if (idf.arrYMinNum[i] == 0)
                        arrYMin2[i] = arrYMin[i];
                    else
                        arrYMin2[i] = matrL[i][idf.arrYMinNum[i] - 1];
                    arrYMax2[i] = matrL[i][idf.arrYMaxNum[i]];
                }
                arrReport[13] = Quality.Ident(matrX, matrY, arrYMin2, arrYMax2,
                    idf.arrU0Min, idf.arrU0Max, idf.arrDU, idf.maxIter, idf.DQ,
                    out arrUMin, out arrUMax, out Q0, out Q);
                string[,] matr = new string[arrUMin.Length, 3];
                for (int i = 0; i < arrUMin.Length; i++)
                {
                    matr[i, 0] = arrXName[i];
                    matr[i, 1] = string.Format("{0:g4}", arrUMin[i]);
                    matr[i, 2] = string.Format("{0:g4}", arrUMax[i]);
                }
                arrReport[13] += "Результат идентификации" +
                    Quality.Table2(matr, new string[] { "Технологический фактор", "Min", "Max" });
                for (int i = 0; i < arrUMin.Length; i++)
                {
                    matr[i, 0] = arrXName[i];
                    matr[i, 1] = string.Format("{0:g4}", arrUMin[i] * arrXAv[i]);
                    matr[i, 2] = string.Format("{0:g4}", arrUMax[i] * arrXAv[i]);
                }
                arrReport[13] += "Результат идентификации (ненормированные значения)" +
                    Quality.Table2(matr, new string[] { "Технологический фактор", "Min", "Max" });
            }
            catch
            {
                MessageBox.Show("Ошибка идентификации");
                return;
            }

            try
            {
                if (matrLL.Length == 2)
                {
                    List<int> lInd = new List<int>();
                    for (int i = 0; i < matrX.GetLength(0); i++)
                    {
                        int j;
                        for (j = 0; j < arrUMin.Length; j++)
                            if (matrX[i, j] < arrUMin[j] ||
                                matrX[i, j] > arrUMax[j])
                                break;
                        if (j == arrUMin.Length)
                            lInd.Add(i);
                    }
                    double[,] matrYU = new double[lInd.Count, arrYMin.Length];
                    for (int i = 0; i < lInd.Count; i++)
                        for (int j = 0; j < arrYMin.Length; j++)
                            matrYU[i, j] = matrY[lInd[i], j];
                    int N;
                    int[] arrF;
                    double[] arrP;
                    double[][] matrL;
                    Quality.Clust(matrYU, matrLL, arrYMin, arrYMax,
                        out matrL, out arrF, out arrP, out N);
                    int iMin = idf.arrYMinNum[0], iMax = idf.arrYMaxNum[0];
                    int jMin = idf.arrYMinNum[1], jMax = idf.arrYMaxNum[1];
                    double[] arrLX = (double[])matrLL[0].Clone(), arrLY = (double[])matrLL[1].Clone();
                    double[,] matrZ = new double[arrLX.Length, arrLY.Length];
                    int[,] matrC = new int[arrLX.Length, arrLY.Length];
                    for (int i = 0; i < arrLX.Length; i++)
                        for (int j = 0; j < arrLY.Length; j++)
                        {
                            matrZ[i, j] = arrP[i * arrLY.Length + j];
                            if (i >= iMin && i <= iMax && j >= jMin && j <= jMax)
                                matrC[i, j] = Color.Red.ToArgb();
                            else
                                matrC[i, j] = Color.LemonChiffon.ToArgb();
                        }
                    H2DForm hf = new H2DForm(arrLX, arrLY, matrZ, matrC, 1);
                    hf.InitializeGraphics();
                    hf.Show();
                }
                if (matrLL.Length == 3)
                {
                    List<int> lInd = new List<int>();
                    for (int i = 0; i < matrX.GetLength(0); i++)
                    {
                        int j;
                        for (j = 0; j < arrUMin.Length; j++)
                            if (matrX[i, j] < arrUMin[j] ||
                                matrX[i, j] > arrUMax[j])
                                break;
                        if (j == arrUMin.Length)
                            lInd.Add(i);
                    }
                    double[,] matrYU = new double[lInd.Count, arrYMin.Length];
                    for (int i = 0; i < lInd.Count; i++)
                        for (int j = 0; j < arrYMin.Length; j++)
                            matrYU[i, j] = matrY[lInd[i], j];
                    int N;
                    int[] arrF;
                    double[] arrP;
                    double[][] matrL;
                    Quality.Clust(matrYU, matrLL, arrYMin, arrYMax,
                        out matrL, out arrF, out arrP, out N);
                    int iMin = idf.arrYMinNum[0], iMax = idf.arrYMaxNum[0];
                    int jMin = idf.arrYMinNum[1], jMax = idf.arrYMaxNum[1];
                    int kMin = idf.arrYMinNum[2], kMax = idf.arrYMaxNum[2];
                    double[] arrLX = (double[])matrLL[0].Clone(), arrLY = (double[])matrLL[1].Clone(),
                        arrLZ = (double[])matrLL[2].Clone();
                    double[,] matrXY = new double[arrLX.Length, arrLY.Length],
                        matrXZ = new double[arrLX.Length, arrLZ.Length],
                        matrYZ = new double[arrLY.Length, arrLZ.Length];
                    bool[,] matrTXY = new bool[arrLX.Length, arrLY.Length],
                        matrTXZ = new bool[arrLX.Length, arrLZ.Length],
                        matrTYZ = new bool[arrLY.Length, arrLZ.Length];
                    for (int i = 0; i < arrLX.Length; i++)
                        for (int j = 0; j < arrLY.Length; j++)
                        {
                            for (int k = 0; k < arrLZ.Length; k++)
                                matrXY[i, j] += arrP[i * arrLY.Length * arrLZ.Length + j * arrLZ.Length + k];
                            if (i >= iMin && i <= iMax && j >= jMin && j <= jMax)
                                matrTXY[i, j] = true;
                            else
                                matrTXY[i, j] = false;
                        }
                    for (int i = 0; i < arrLX.Length; i++)
                        for (int k = 0; k < arrLZ.Length; k++)
                        {
                            for (int j = 0; j < arrLY.Length; j++)
                                matrXZ[i, k] += arrP[i * arrLY.Length * arrLZ.Length + j * arrLZ.Length + k];
                            if (i >= iMin && i <= iMax && k >= kMin && k <= kMax)
                                matrTXZ[i, k] = true;
                            else
                                matrTXZ[i, k] = false;
                        }
                    for (int j = 0; j < arrLY.Length; j++)
                        for (int k = 0; k < arrLZ.Length; k++)
                        {
                            for (int i = 0; i < arrLX.Length; i++)
                                matrYZ[j, k] += arrP[i * arrLY.Length * arrLZ.Length + j * arrLZ.Length + k];
                            if (j >= jMin && j <= jMax && k >= kMin && k <= kMax)
                                matrTYZ[j, k] = true;
                            else
                                matrTYZ[j, k] = false;
                        }
                    H3DForm hf = new H3DForm(arrLX, arrLY, arrLZ, matrXY, matrXZ, matrYZ, matrTXY, matrTXZ, matrTYZ, 1);
                    hf.InitializeGraphics();
                    hf.Show();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка построения гистограммы");
            }
        }
        
        void d3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                D3PropForm d3p = new D3PropForm();
                if (d3p.ShowDialog() != DialogResult.OK)
                    return;
                D3Form d3f = new D3Form(d3p.matrMin, d3p.matrMax, d3p.arrTexture);
                d3f.InitializeGraphics();
                d3f.Show();
            }
            catch
            {
                MessageBox.Show("Ошибка построения");
            }
        }

        private void d2HistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (matrLL.Length != 2)
                    return;
                double[] arrLX = (double[])matrLL[0].Clone(), arrLY = (double[])matrLL[1].Clone();
                double[,] matrZ = new double[arrLX.Length, arrLY.Length];
                int[,] matrC = new int[arrLX.Length, arrLY.Length];
                for (int i = 0; i < arrLX.Length; i++)
                    for (int j = 0; j < arrLY.Length; j++)
                    {
                        matrZ[i, j] = arrP[i * arrLY.Length + j];
                        matrC[i, j] = Color.LemonChiffon.ToArgb();
                    }
                H2DForm hf = new H2DForm(arrLX, arrLY, matrZ, matrC, 1);
                hf.InitializeGraphics();
                hf.Show();
            }
            catch
            {
                MessageBox.Show("Ошибка построения гистограммы");
            }
        }

        private void d2HistZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (matrLL.Length != 2)
                    return;
                H2DPropForm hpf = new H2DPropForm();
                if (hpf.ShowDialog() != DialogResult.OK)
                    return;
                int iMin = int.Parse(hpf.tbY1Min.Text), iMax = int.Parse(hpf.tbY1Max.Text);
                int jMin = int.Parse(hpf.tbY2Min.Text), jMax = int.Parse(hpf.tbY2Max.Text);
                double[] arrLX = (double[])matrLL[0].Clone(), arrLY = (double[])matrLL[1].Clone();
                double[,] matrZ = new double[arrLX.Length, arrLY.Length];
                int[,] matrC = new int[arrLX.Length, arrLY.Length];
                for (int i = 0; i < arrLX.Length; i++)
                    for (int j = 0; j < arrLY.Length; j++)
                    {
                        matrZ[i, j] = arrP[i * arrLY.Length + j];
                        if (i >= iMin && i <= iMax && j >= jMin && j <= jMax)
                            matrC[i, j] = Color.Red.ToArgb();
                        else
                            matrC[i, j] = Color.LemonChiffon.ToArgb();
                    }
                H2DForm hf = new H2DForm(arrLX, arrLY, matrZ, matrC, 1);
                hf.InitializeGraphics();
                hf.Show();
            }
            catch
            {
                MessageBox.Show("Ошибка построения гистограммы");
            }
        }

        private void d3HistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (matrLL.Length != 3)
                    return;
                double[] arrLX = (double[])matrLL[0].Clone(), arrLY = (double[])matrLL[1].Clone(),
                    arrLZ = (double[])matrLL[2].Clone();
                double[,] matrXY = new double[arrLX.Length, arrLY.Length],
                    matrXZ = new double[arrLX.Length, arrLZ.Length],
                    matrYZ = new double[arrLY.Length, arrLZ.Length];
                bool[,] matrTXY = new bool[arrLX.Length, arrLY.Length],
                    matrTXZ = new bool[arrLX.Length, arrLZ.Length],
                    matrTYZ = new bool[arrLY.Length, arrLZ.Length];
                for (int i = 0; i < arrLX.Length; i++)
                    for (int j = 0; j < arrLY.Length; j++)
                    {
                        for (int k = 0; k < arrLZ.Length; k++)
                            matrXY[i, j] += arrP[i * arrLY.Length * arrLZ.Length + j * arrLZ.Length + k];
                        matrTXY[i, j] = false;
                    }
                for (int i = 0; i < arrLX.Length; i++)
                    for (int k = 0; k < arrLZ.Length; k++)
                    {
                        for (int j = 0; j < arrLY.Length; j++)
                            matrXZ[i, k] += arrP[i * arrLY.Length * arrLZ.Length + j * arrLZ.Length + k];
                        matrTXZ[i, k] = false;
                    }
                for (int j = 0; j < arrLY.Length; j++)
                    for (int k = 0; k < arrLZ.Length; k++)
                    {
                        for (int i = 0; i < arrLX.Length; i++)
                            matrYZ[j, k] += arrP[i * arrLY.Length * arrLZ.Length + j * arrLZ.Length + k];
                        matrTYZ[j, k] = false;
                    }
                H3DForm hf = new H3DForm(arrLX, arrLY, arrLZ, matrXY, matrXZ, matrYZ, matrTXY, matrTXZ, matrTYZ, 1);
                hf.InitializeGraphics();
                hf.Show();
            }
            catch
            {
                MessageBox.Show("Ошибка построения гистограммы");
            }
        }

        private void d3HistZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (matrLL.Length != 3)
                    return;
                H3DPropForm hpf = new H3DPropForm();
                if (hpf.ShowDialog() != DialogResult.OK)
                    return;
                int iMin = int.Parse(hpf.tbY1Min.Text), iMax = int.Parse(hpf.tbY1Max.Text);
                int jMin = int.Parse(hpf.tbY2Min.Text), jMax = int.Parse(hpf.tbY2Max.Text);
                int kMin = int.Parse(hpf.tbY3Min.Text), kMax = int.Parse(hpf.tbY3Max.Text);
                double[] arrLX = (double[])matrLL[0].Clone(), arrLY = (double[])matrLL[1].Clone(),
                    arrLZ = (double[])matrLL[2].Clone();
                double[,] matrXY = new double[arrLX.Length, arrLY.Length],
                    matrXZ = new double[arrLX.Length, arrLZ.Length],
                    matrYZ = new double[arrLY.Length, arrLZ.Length];
                bool[,] matrTXY = new bool[arrLX.Length, arrLY.Length],
                    matrTXZ = new bool[arrLX.Length, arrLZ.Length],
                    matrTYZ = new bool[arrLY.Length, arrLZ.Length];
                for (int i = 0; i < arrLX.Length; i++)
                    for (int j = 0; j < arrLY.Length; j++)
                    {
                        for (int k = 0; k < arrLZ.Length; k++)
                            matrXY[i, j] += arrP[i * arrLY.Length * arrLZ.Length + j * arrLZ.Length + k];
                        if (i >= iMin && i <= iMax && j >= jMin && j <= jMax)
                            matrTXY[i, j] = true;
                        else
                            matrTXY[i, j] = false;
                    }
                for (int i = 0; i < arrLX.Length; i++)
                    for (int k = 0; k < arrLZ.Length; k++)
                    {
                        for (int j = 0; j < arrLY.Length; j++)
                            matrXZ[i, k] += arrP[i * arrLY.Length * arrLZ.Length + j * arrLZ.Length + k];
                        if (i >= iMin && i <= iMax && k >= kMin && k <= kMax)
                            matrTXZ[i, k] = true;
                        else
                            matrTXZ[i, k] = false;
                    }
                for (int j = 0; j < arrLY.Length; j++)
                    for (int k = 0; k < arrLZ.Length; k++)
                    {
                        for (int i = 0; i < arrLX.Length; i++)
                            matrYZ[j, k] += arrP[i * arrLY.Length * arrLZ.Length + j * arrLZ.Length + k];
                        if (j >= jMin && j <= jMax && k >= kMin && k <= kMax)
                            matrTYZ[j, k] = true;
                        else
                            matrTYZ[j, k] = false;
                    }
                H3DForm hf = new H3DForm(arrLX, arrLY, arrLZ, matrXY, matrXZ, matrYZ, matrTXY, matrTXZ, matrTYZ, 1);
                hf.InitializeGraphics();
                hf.Show();
            }
            catch
            {
                MessageBox.Show("Ошибка построения гистограммы");
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Выполнил Кондауров А.С.");
        }
    }
}