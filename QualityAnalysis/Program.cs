using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Distributions;
using System.Collections;


namespace QualityDim
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
    static class Quality
    {
        public static void ImportData(string file, string sX, string sY, string rX, string rY,
            out double[,] matrX, out double[,] matrY, out string[] arrXName, out string[] arrYName)
        {
            string s = "provider = Microsoft.Jet.OLEDB.4.0;" +
                "data source = " + file + ";" +
                "extended properties = Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(s);
            conn.Open();
            s = string.Format("SELECT * FROM [{0}${1}]", sX.Replace('.', '#'), rX);
            OleDbDataAdapter da = new OleDbDataAdapter(s, conn);
            DataTable tX = new DataTable();
            da.Fill(tX);
            s = string.Format("SELECT * FROM [{0}${1}]", sY.Replace('.', '#'), rY);
            da = new OleDbDataAdapter(s, conn);
            DataTable tY = new DataTable();
            da.Fill(tY);
            conn.Close();
            if (tX.Rows.Count != tY.Rows.Count)
                throw new Exception();
            double tmp;
            List<int> lRem = new List<int>();
            for (int i = 0; i < tX.Rows.Count; i++)
            {
                int j;
                for (j = 0; j < tX.Columns.Count; j++)
                {
                    if (!double.TryParse(tX.Rows[i][j].ToString(), out tmp))
                    {
                        lRem.Add(i);
                        break;
                    }
                }
                if (j < tX.Columns.Count)
                    continue;
                for (j = 0; j < tY.Columns.Count; j++)
                {
                    if (!double.TryParse(tY.Rows[i][j].ToString(), out tmp))
                    {
                        lRem.Add(i);
                        break;
                    }
                }
            }
            lRem.Reverse();
            foreach (int i in lRem)
            {
                tX.Rows.RemoveAt(i);
                tY.Rows.RemoveAt(i);
            }
            matrX = new double[tX.Rows.Count, tX.Columns.Count];
            arrXName = new string[tX.Columns.Count];
            for (int i = 0; i < tX.Columns.Count; i++)
                arrXName[i] = tX.Columns[i].Caption;
            for (int i = 0; i < tX.Rows.Count; i++)
                for (int j = 0; j < tX.Columns.Count; j++)
                    matrX[i, j] = (double)tX.Rows[i][j];
            matrY = new double[tY.Rows.Count, tY.Columns.Count];
            arrYName = new string[tY.Columns.Count];
            for (int i = 0; i < tY.Columns.Count; i++)
                arrYName[i] = tY.Columns[i].Caption;
            for (int i = 0; i < tY.Rows.Count; i++)
                for (int j = 0; j < tY.Columns.Count; j++)
                    matrY[i, j] = (double)tY.Rows[i][j];
        }
        public static void Norm(double[,] matr, out double[] arrAv)
        {
            arrAv = new double[matr.GetLength(1)];
            for (int j = 0; j < matr.GetLength(1); j++)
            {
                arrAv[j] = 0;
                for (int i = 0; i < matr.GetLength(0); i++)
                    arrAv[j] += matr[i, j];
                arrAv[j] /= matr.GetLength(0);
                for (int i = 0; i < matr.GetLength(0); i++)
                    matr[i, j] /= arrAv[j];
            }
        }
        public static void GenErr(double[,] matrX, double[,] matrY, Regression[] arrReg,
            int n, double a, double b, out int[] arrI,
            out double[,] matrXM, out double[,] matrYM)
        {
            if (n > matrX.GetLength(0))
                throw new Exception();
            double[] arrXS = new double[matrX.GetLength(1)];
            double[] arrXAv = new double[matrX.GetLength(1)];
            for (int j = 0; j < matrX.GetLength(1); j++)
            {
                double sumX = 0, sumX2 = 0;
                for (int i = 0; i < matrX.GetLength(0); i++)
                {
                    sumX += matrX[i, j];
                    sumX2 += matrX[i, j] * matrX[i, j];
                }
                arrXAv[j] = sumX / matrX.GetLength(0);
                sumX2 /= matrX.GetLength(0);
                arrXS[j] = (sumX2 - arrXAv[j] * arrXAv[j]) * matrX.GetLength(0) /
                    (matrX.GetLength(0) - 1);
            }
            List<int> listAll = new List<int>();
            List<int> listI = new List<int>();
            for (int i = 0; i < matrX.GetLength(0); i++)
                listAll.Add(i);
            ContinuousUniformDistribution distUn = new ContinuousUniformDistribution(0, 1);
            for (int i = 0; i < n; i++)
            {                
                int li = (int)(distUn.NextDouble() * listAll.Count);
                listI.Add(listAll[li]);
                listAll.RemoveAt(li);
            }
            arrI = listI.ToArray();
            NormalDistribution dist = new NormalDistribution();
            matrXM = (double[,])matrX.Clone();
            for (int j = 0; j < matrX.GetLength(1); j++)
            {
                dist.SetDistributionParameters(a * arrXAv[j], b * arrXS[j]);
                foreach (int i in arrI)
                    matrXM[i, j] += dist.NextDouble();
            }
            matrYM = (double[,])matrY.Clone();
            double[] arrX = new double[matrX.GetLength(1)];
            foreach (int i in arrI)
            {
                for (int j = 0; j < matrX.GetLength(1); j++)
                    arrX[j] = matrXM[i, j];
                for (int j = 0; j < arrReg.Length; j++)
                    matrYM[i, j] = arrReg[j].GetRegValue(arrX);
            }
        }
        public static string DataTable2(string[] arrXName, string[] arrYName,
            double[,] matrX, double[,] matrY, double[] arrXAv, double[] arrYAv)
        {
            string res = "<TABLE border = 1 cellspacing = 0 align = center><TR><TD>i";
            for (int j = 0; j < arrXName.Length; j++)
                res += string.Format("<TD>{0}", arrXName[j]);
            for (int j = 0; j < arrYName.Length; j++)
                res += string.Format("<TD>{0}", arrYName[j]);
            for (int i = 0; i < matrX.GetLength(0); i++)
            {
                res += string.Format("<TR><TD>{0}", i + 1);
                for (int j = 0; j < matrX.GetLength(1); j++)
                    res += string.Format("<TD>{0:F3} ({1:G4})", matrX[i, j], matrX[i,j] * arrXAv[j]);
                for (int j = 0; j < matrY.GetLength(1); j++)
                    res += string.Format("<TD>{0:F3} ({1:G4})", matrY[i, j], matrY[i, j] * arrYAv[j]);
            }
            return res + "</TABLE>";
        }
        public static void Clust(double[,] matrY, double[][] matrLL, double[] arrYMin,
            double[] arrYMax, out double[][] matrL, out int[] arrF, out double[] arrP, out int N)
        {
            matrL = new double[matrLL.Length][];
            for (int i = 0; i < matrL.Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < matrLL[i].Length; j++)
                    sum += matrLL[i][j];
                matrL[i] = new double[matrLL[i].Length];
                double delta = arrYMax[i] - arrYMin[i];
                double sumL = arrYMin[i];
                for (int j = 0; j < matrL[i].Length; j++)
                {
                    sumL += matrLL[i][j] / sum * delta;
                    matrL[i][j] = sumL;
                }
            }
            int[] arrDim = new int[matrLL.Length];
            int mult = 1;
            for (int i = 0; i < arrDim.Length; i++)
            {
                arrDim[i] = matrLL[i].Length;
                mult *= arrDim[i];
            }
            Linearizer lin = new Linearizer(arrDim);
		    arrF = new int[mult];
            N = 0;
            for (int i = 0; i < matrY.GetLength(0); i++)
            {
                int[] arrInd = new int[matrY.GetLength(1)];
                int j;
                for (j = 0; j < matrY.GetLength(1); j++)
                {
                    if (matrY[i, j] < arrYMin[j] ||
                        matrY[i, j] > arrYMax[j])
                        break;
                    int k;
                    for (k = 0; k < matrL[j].Length - 1; k++)
                        if (matrY[i, j] < matrL[j][k])
                            break;
                    arrInd[j] = k;
                }
                if (j < matrY.GetLength(1))
                    continue;
                N++;
                arrF[lin.GetIndex(arrInd)]++;
            }
            arrP = new double[mult];
            for (int i = 0; i < mult; i++)
                arrP[i] = arrF[i] / (double)N;
        }
        public static void ClustRepl(double[,] matrY, double[][] matrLL, double[] arrYMin,
            double[] arrYMax, string[] arrRepl,
            out double[][] matrL, out int N, out int[] arrF, out double[] arrP,
            out string[] arrC, out int[][] arrArrIC, out double[] arrPC, out double H)
        {
            matrL = new double[matrLL.Length][];
            for (int i = 0; i < matrL.Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < matrLL[i].Length; j++)
                    sum += matrLL[i][j];
                matrL[i] = new double[matrLL[i].Length];
                double delta = arrYMax[i] - arrYMin[i];
                double sumL = arrYMin[i];
                for (int j = 0; j < matrL[i].Length; j++)
                {
                    sumL += matrLL[i][j] / sum * delta;
                    matrL[i][j] = sumL;
                }
            }
            int[] arrDim = new int[matrLL.Length];
            int mult = 1;
            for (int i = 0; i < arrDim.Length; i++)
            {
                arrDim[i] = matrLL[i].Length;
                mult *= arrDim[i];
            }
            Linearizer lin = new Linearizer(arrDim);
		    arrF = new int[mult];
            N = 0;
            for (int i = 0; i < matrY.GetLength(0); i++)
            {
                int[] arrInd = new int[matrY.GetLength(1)];
                int j;
                for (j = 0; j < matrY.GetLength(1); j++)
                {
                    if (matrY[i, j] < arrYMin[j] ||
                        matrY[i, j] > arrYMax[j])
                        break;
                    int k;
                    for (k = 0; k < matrL[j].Length - 1; k++)
                        if (matrY[i, j] < matrL[j][k])
                            break;
                    arrInd[j] = k;
                }
                if (j < matrY.GetLength(1))
                    continue;
                N++;
                arrF[lin.GetIndex(arrInd)]++;
            }
            arrP = new double[mult];
            for (int i = 0; i < mult; i++)
                arrP[i] = arrF[i] / (double)N;

            List<string> listC = new List<string>();
            List<List<int>> listListInd = new List<List<int>>();
            foreach (string s in arrRepl)
            {
                listC.Add(s);
                string[] arrStrInd = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List<int> li = new List<int>();
                foreach (string si in arrStrInd)
                        li.Add(int.Parse(si));
                listListInd.Add(li);
            }
            arrC = listC.ToArray();
            arrArrIC = new int[listListInd.Count][];
            arrPC = new double[listListInd.Count];
            H = 0;
            for (int i = 0; i < arrArrIC.Length; i++)
            {
                arrArrIC[i] = listListInd[i].ToArray();
                for (int j = 0; j < arrArrIC[i].Length; j++)
                    arrPC[i] += arrP[arrArrIC[i][j]];
                if (arrPC[i] != 0)
                    H -= arrPC[i] * Math.Log(arrPC[i]);
            }
        }
        public static string CTable(string[] arrYName, double[][] matrL,
            double[] arrYMin, double[] arrYMax, int[] arrF, double[] arrP, double[] arrYAv)
        {
            string res = "<TABLE border = 1 cellspacing = 0 align = center>";
            res += "<TR><TD>Номер кластера<TD>Идентификатор";
            for (int i = 0; i < arrYName.Length; i++)
                res += string.Format("<TD>{0}", arrYName[i]);
            int[] arrDim = new int[arrYName.Length];
            int mult = 1;
            for (int i = 0; i < arrDim.Length; i++)
            {
                arrDim[i] = matrL[i].Length;
                mult *= arrDim[i];
            }
            res += "<TD>Абс. частота<TD>Вероятность";
            Linearizer lin = new Linearizer(arrDim);
            for (int i = 0; i < mult; i++)
            {
                res += string.Format("<TR><TD>{0}", i);
                int[] arrInd = lin.GetIndexes(i);
                res += "<TD>( ";
                for (int j = 0; j < arrInd.Length; j++)
                {
                    res += string.Format("{0} ", arrInd[j]);
                }
                res += ")";
                for (int j = 0; j < arrInd.Length; j++)
                {
                    if (arrInd[j] == 0)
                        res += string.Format("<TD>[{0:F3}; {1:F3})  /[{2:g4}; {3:g4})/",
                            arrYMin[j], matrL[j][0], arrYMin[j] * arrYAv[j], matrL[j][0] * arrYAv[j]);
                    else if (arrInd[j] == arrDim[j] - 1)
                        res += string.Format("<TD>[{0:F3}; {1:F3}]  /[{2:g4}; {3:g4}]/",
                            matrL[j][arrInd[j] - 1], matrL[j][arrInd[j]],
                            matrL[j][arrInd[j] - 1] * arrYAv[j], matrL[j][arrInd[j]] * arrYAv[j]);
                    else
                        res += string.Format("<TD>[{0:F3}; {1:F3})  /[{2:g4}; {3:g4})/",
                            matrL[j][arrInd[j] - 1], matrL[j][arrInd[j]],
                            matrL[j][arrInd[j] - 1] * arrYAv[j], matrL[j][arrInd[j]] * arrYAv[j]);
                }
                res += string.Format("<TD>{0}<TD>{1:F3}", arrF[i], arrP[i]);
            }
            return res + "</TABLE>";
        }
        public static string RTable(string[] arrC, double[] arrPC)
        {
            string res = "<TABLE border = 1 cellspacing = 0 align = center>";
            res += "<TR><TD>Номера кластеров, образующих реплику<TD>Вероятность";
            for (int i = 0; i < arrC.Length; i++)
                res += string.Format("<TR><TD>{0}<TD>{1:F3}", arrC[i], arrPC[i]);
            return res + "</TABLE>";
        }
        public static string Ident(double[,] matrX, double[,] matrY, double[] arrYMin, double[] arrYMax,
            double[] arrU0Min, double[] arrU0Max, double[] arrDU, int maxIter, double DQ,
            out double[] arrUMin, out double[] arrUMax, out double Q0, out double Q)
        {
            double[,] matr1 = new double[arrU0Min.Length, 3];
            for (int i = 0; i < arrU0Min.Length; i++)
            {
                matr1[i, 0] = arrU0Min[i];
                matr1[i, 1] = arrU0Max[i];
                matr1[i, 2] = arrDU[i];
            }
            string s = "Технологическое пространство" +
                Table(matr1, new string[] { "Min", "Max", "Шаг" }, 4);
            double[,] matr2 = new double[arrYMin.Length, 2];
            for (int i = 0; i < arrYMin.Length; i++)
            {
                matr2[i, 0] = arrYMin[i];
                matr2[i, 1] = arrYMax[i];
            }
            s += "Пространство особого качества" +
                Table(matr2, new string[] { "Min", "Max" }, 4);
            s += string.Format("Точность: {0}<br>Максимальное количество итераций: {1}<br>", DQ, maxIter);
            List<int> lUY = new List<int>(), lUy = new List<int>();
            List<int> luY = new List<int>(), luy = new List<int>();
            List<int> ldUYOpt = null, ldUyOpt = null, lduYOpt = null, lduyOpt = null;
            bool increase = true;
            int N = matrX.GetLength(0), jOpt = -1;
            double QOpt, DUOpt = -1;
            double[] arrUOpt = null;
            arrUMin = (double[])arrU0Min.Clone();
            arrUMax = (double[])arrU0Max.Clone();
            for (int i = 0; i < N; i++)
            {
                int j, k;
                for (j = 0; j < arrUMin.Length; j++)
                    if (matrX[i, j] < arrUMin[j] || matrX[i, j] > arrUMax[j])
                        break;
                for (k = 0; k < arrYMin.Length; k++)
                    if (matrY[i, k] < arrYMin[k] || matrY[i, k] > arrYMax[k])
                        break;
                if (j < arrUMin.Length)
                    if (k < arrYMin.Length)
                        luy.Add(i);
                    else
                        luY.Add(i);
                else
                    if (k < arrYMin.Length)
                        lUy.Add(i);
                    else
                        lUY.Add(i);
            }
            double V = 1, VOpt = -1;
            for (int j = 0; j < arrUMin.Length; j++)
                V *= arrUMax[j] - arrUMin[j];
            Q0 = Q = Iuy(lUY.Count, lUy.Count, luY.Count, luy.Count, N) / V;
            s += "<table border = 1 cellspacing = 0 align = center>" +
                "<tr><td>Номер итерации<td>Объемная плотность вероятности<td>Изменение" +
                "<td>I<sub>UY</sub><td>V<td>n(U<sup>+</sup>Y<sup>+</sup>)" +
                "<td>n(U<sup>+</sup>Y<sup>-</sup>)<td>n(U<sup>-</sup>Y<sup>+</sup>)" +
                "<td>n(U<sup>-</sup>Y<sup>-</sup>)" +
                string.Format("<tr><td>0<td>{0:g5}<td>-<td>{1:g5}<td>{2:g5}<td>{3}<td>{4}<td>{5}<td>{6}",
                Q0, Q0 * V, V, lUY.Count, lUy.Count, luY.Count, luy.Count);
            for (int i = 0; i < maxIter; i++)
            {
                QOpt = Q;
                for (int j = 0; j < arrUMin.Length; j++)
                {
                    // UMin - d
                    List<int> lduY = new List<int>(), lduy = new List<int>();
                    double uOld = arrUMin[j];
                    arrUMin[j] -= arrDU[j];
                    foreach (int k in luY)
                    {
                        int l;
                        for (l = 0; l < arrUMin.Length; l++)
                            if (matrX[k, l] < arrUMin[l] ||
                                matrX[k, l] > arrUMax[l])
                                break;
                        if (l == arrUMin.Length)
                            lduY.Add(k);
                    }
                    foreach (int k in luy)
                    {
                        int l;
                        for (l = 0; l < arrUMin.Length; l++)
                            if (matrX[k, l] < arrUMin[l] ||
                                matrX[k, l] > arrUMax[l])
                                break;
                        if (l == arrUMin.Length)
                            lduy.Add(k);
                    }
                    arrUMin[j] = uOld;
                    double VNext = V / (arrUMax[j] - arrUMin[j]) * (arrUMax[j] - arrUMin[j] + arrDU[j]);
                    double QNext = Iuy(lUY.Count + lduY.Count, lUy.Count + lduy.Count,
                        luY.Count - lduY.Count, luy.Count - lduy.Count, N) / VNext;
                    if (QNext > QOpt)
                    {
                        QOpt = QNext;
                        VOpt = VNext;
                        increase = true;
                        lduYOpt = lduY;
                        lduyOpt = lduy;
                        arrUOpt = arrUMin;
                        jOpt = j;
                        DUOpt = -arrDU[j];
                    }
                    // UMin + d
                    List<int> ldUY = new List<int>(), ldUy = new List<int>();
                    foreach (int k in lUY)
                        if (matrX[k, j] < arrUMin[j] + arrDU[j])
                            ldUY.Add(k);
                    foreach (int k in lUy)
                        if (matrX[k, j] < arrUMin[j] + arrDU[j])
                            ldUy.Add(k);
                    VNext = V / (arrUMax[j] - arrUMin[j]) * (arrUMax[j] - arrUMin[j] - arrDU[j]);
                    QNext = Iuy(lUY.Count - ldUY.Count, lUy.Count - ldUy.Count,
                        luY.Count + ldUY.Count, luy.Count + ldUy.Count, N) / VNext;
                    if (QNext > QOpt)
                    {
                        QOpt = QNext;
                        VOpt = VNext;
                        increase = false;
                        ldUYOpt = ldUY;
                        ldUyOpt = ldUy;
                        arrUOpt = arrUMin;
                        jOpt = j;
                        DUOpt = arrDU[j];
                    }
                    // UMax + d
                    lduY = new List<int>();
                    lduy = new List<int>();
                    uOld = arrUMax[j];
                    arrUMax[j] += arrDU[j];
                    foreach (int k in luY)
                    {
                        int l;
                        for (l = 0; l < arrUMin.Length; l++)
                            if (matrX[k, l] < arrUMin[l] ||
                                matrX[k, l] > arrUMax[l])
                                break;
                        if (l == arrUMin.Length)
                            lduY.Add(k);
                    }
                    foreach (int k in luy)
                    {
                        int l;
                        for (l = 0; l < arrUMin.Length; l++)
                            if (matrX[k, l] < arrUMin[l] ||
                                matrX[k, l] > arrUMax[l])
                                break;
                        if (l == arrUMin.Length)
                            lduy.Add(k);
                    }
                    arrUMax[j] = uOld;
                    VNext = V / (arrUMax[j] - arrUMin[j]) * (arrUMax[j] - arrUMin[j] + arrDU[j]);
                    QNext = Iuy(lUY.Count + lduY.Count, lUy.Count + lduy.Count,
                        luY.Count - lduY.Count, luy.Count - lduy.Count, N) / VNext;
                    if (QNext > QOpt)
                    {
                        QOpt = QNext;
                        VOpt = VNext;
                        increase = true;
                        lduYOpt = lduY;
                        lduyOpt = lduy;
                        arrUOpt = arrUMax;
                        jOpt = j;
                        DUOpt = arrDU[j];
                    }
                    // UMax - d
                    ldUY = new List<int>();
                    ldUy = new List<int>();
                    foreach (int k in lUY)
                        if (matrX[k, j] > arrUMax[j] - arrDU[j])
                            ldUY.Add(k);
                    foreach (int k in lUy)
                        if (matrX[k, j] > arrUMax[j] - arrDU[j])
                            ldUy.Add(k);
                    VNext = V / (arrUMax[j] - arrUMin[j]) * (arrUMax[j] - arrUMin[j] - arrDU[j]);
                    QNext = Iuy(lUY.Count - ldUY.Count, lUy.Count - ldUy.Count,
                        luY.Count + ldUY.Count, luy.Count + ldUy.Count, N) / VNext;
                    if (QNext > QOpt)
                    {
                        QOpt = QNext;
                        VOpt = VNext;
                        increase = false;
                        ldUYOpt = ldUY;
                        ldUyOpt = ldUy;
                        arrUOpt = arrUMax;
                        jOpt = j;
                        DUOpt = -arrDU[j];
                    }
                }
                if (QOpt == Q || Math.Abs(QOpt - Q) < DQ)
                    return s + "</table>";
                if (increase)
                {
                    lUY.AddRange(lduYOpt);
                    lUy.AddRange(lduyOpt);
                    foreach (int k in lduYOpt)
                        luY.Remove(k);
                    foreach (int k in lduyOpt)
                        luy.Remove(k);
                    string str = "-Δu'";
                    if (arrUOpt == arrUMax)
                        str = "+Δu''";
                    s += string.Format("<tr><td>{0}<td>{1}<td>{2}<sub>{3}</sub><td>{4:g5}<td>{5:g5}<td>{6}<td>{7}<td>{8}<td>{9}",
                        i + 1, QOpt, str, jOpt, QOpt * VOpt, VOpt, lUY.Count, lUy.Count, luY.Count, luy.Count);
                }
                else
                {
                    luY.AddRange(ldUYOpt);
                    luy.AddRange(ldUyOpt);
                    foreach (int k in ldUYOpt)
                        lUY.Remove(k);
                    foreach (int k in ldUyOpt)
                        lUy.Remove(k);
                    string str = "+Δu'";
                    if (arrUOpt == arrUMax)
                        str = "-Δu''";
                    s += string.Format("<tr><td>{0}<td>{1:g5}<td>{2}<sub>{3}</sub><td>{4:g5}<td>{5:g5}<td>{6}<td>{7}<td>{8}<td>{9}",
                        i + 1, QOpt, str, jOpt, QOpt * VOpt, VOpt, lUY.Count, lUy.Count, luY.Count, luy.Count);
                }
                Q = QOpt;
                V = VOpt;
                arrUOpt[jOpt] += DUOpt;
            }
            return s + "</table>";
        }
        public static double Iuy(double lUY, double lUy, double luY, double luy, int N)
        {
            double[] arrP1 = { (lUY + lUy) / N, (luY + luy) / N, (lUY + luY) / N, (lUy + luy) / N };
            double[] arrP2 = { lUY / N, luY / N, lUy / N, luy / N };
            double s = 0;
            for (int i = 0; i < 4; i++)
            {
                if (arrP1[i] > 0)
                    s -= arrP1[i] * Math.Log(arrP1[i]);
                if (arrP2[i] > 0)
                    s += arrP2[i] * Math.Log(arrP2[i]);
            }
            return s;
        }
        public static string Table(double[,] matr,
            string[] arrHead, int d)
        {
            string res = "<TABLE border = 1 cellspacing = 0 align = center><TR>";
            string f = "<TD>{0:F" + d.ToString() + "}";
            for (int j = 0; j < matr.GetLength(1); j++)
                res += "<TD>" + arrHead[j];
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                res += "<TR>";
                for (int j = 0; j < matr.GetLength(1); j++)
                    res += string.Format(f, matr[i, j]);
            }
            return res + "</TABLE>";
        }
        public static string Table2(string[,] matr,
            string[] arrHead)
        {
            string res = "<TABLE border = 1 cellspacing = 0 align = center><TR>";
            for (int j = 0; j < matr.GetLength(1); j++)
                res += "<TD>" + arrHead[j];
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                res += "<TR>";
                for (int j = 0; j < matr.GetLength(1); j++)
                    res += "<TD>" + matr[i, j];
            }
            return res + "</TABLE>";
        }
    }
    static class Html
    {
        public static string MakeTable(double[,] matr,
            string[] arrHor, string[] arrVer, int d)
        {
            string res = "<TABLE border = 1><TR><TD>-";
            string f = "<TD>{0:F" + d.ToString() + "}";
            for (int j = 0; j < matr.GetLength(1); j++)
                res += "<TD>" + arrHor[j];
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                res += "<TR><TD>" + arrVer[i];
                for (int j = 0; j < matr.GetLength(1); j++)
                    res += string.Format(f, matr[i, j]);
            }
            return res + "</TABLE>";
        }
        public static string MakeTable(double[,] matr,
            string[] arrHead, int d)
        {
            string res = "<TABLE border = 1><TR>";
            string f = "<TD>{0:F" + d.ToString() + "}";
            for (int j = 0; j < matr.GetLength(1); j++)
                res += "<TD>" + arrHead[j];
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                res += "<TR>";
                for (int j = 0; j < matr.GetLength(1); j++)
                    res += string.Format(f, matr[i, j]);
            }
            return res + "</TABLE>";
        }
        public static string MakeIndexSumTable(double[,] matr,
            string[] arrHead, int d)
        {
            string res = "<TABLE border = 1 cellspacing = 0 align = center><TR><TD>i";
            string f = "<TD>{0:F" + d.ToString() + "}";
            for (int j = 0; j < matr.GetLength(1); j++)
                res += string.Format("<TD>{0}", arrHead[j]);
            double[] arrSum = new double[matr.GetLength(1)];
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                res += string.Format("<TR><TD>{0}", i + 1);
                for (int j = 0; j < matr.GetLength(1); j++)
                {
                    res += string.Format(f, matr[i, j]);
                    arrSum[j] += matr[i, j];
                }
            }
            res += "<TR><TD>Итого";
            for (int j = 0; j < arrSum.Length; j++)
                res += string.Format(f, arrSum[j]);
            return res + "</TABLE>";
        }
    }
    class XY : IComparable
    {
        public int x;
        public int y;
        public XY(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int CompareTo(object obj)
        {
            return x.CompareTo((obj as XY).x);
        }
    }
    class XY2 : IComparable
    {
        public string x;
        public int[] y;
        public XY2(string x, int[] y)
        {
            this.x = x;
            this.y = y;
        }
        public int CompareTo(object obj)
        {
            return x.CompareTo((obj as XY2).x);
        }
    }

    public class Sample : ICloneable
    {
        protected string name = "", mark = "";
        protected double[] arrX;
        int indexMin, indexMax;
        double min, max, average, averageXPow2;
        double sum, sumXPow2, dev, devStd, sigma, sigmaStd;
        double[] arrHistY, arrHistX;    // частоты и середины интервалов
        double var;                  // коэф. вариации
        double mc1, mc2, mc3, mc4;   // центр. моменты
        double mb1, mb2, mb3, mb4;   // нач. моменты
        double asym, exc;            // асим. и эксцесс
        double asymTheor, excTheor, sigmaAsym, sigmaExc;

        public Sample() { }
        public Sample(string name, string mark, double[] arrX)
        {
            this.name = name;
            this.mark = mark;
            this.arrX = arrX;
            Calculate();
        }
        public Sample(double[] arrX)
        {
            this.arrX = arrX;
            Calculate();
        }
        public double this[int index]
        {
            get { return arrX[index]; }
            set { arrX[index] = value; }
        }
        public void Calculate()
        {
            min = double.MaxValue;
            max = double.MinValue;
            sum = 0;
            sumXPow2 = 0;
            for (int i = 0; i < arrX.Length; i++)
            {
                if (arrX[i] < min)
                {
                    min = arrX[i];
                    indexMin = i;
                }
                if (arrX[i] > max)
                {
                    max = arrX[i];
                    indexMax = i;
                }
                sum += arrX[i];
                sumXPow2 += arrX[i] * arrX[i];
            }
            average = sum / arrX.Length;
            averageXPow2 = sumXPow2 / arrX.Length;
            dev = averageXPow2 - (average * average);
            devStd = dev / arrX.Length * (arrX.Length - 1);
            sigma = (double)Math.Sqrt(dev);
            sigmaStd = (double)Math.Sqrt(devStd);

            // моменты
            mb1 = average;
            mb2 = sumXPow2 / arrX.Length;
            mc2 = dev;
            mc1 = mc3 = mc4 = 0;
            mb1 = mb2 = mb3 = mb4 = 0;
            for (int i = 0; i < arrX.Length; i++)
            {
                mb3 += arrX[i] * arrX[i] * arrX[i];
                mb4 += arrX[i] * arrX[i] * arrX[i] * arrX[i];
                mc1 += arrX[i] - mb1;
                mc3 += (arrX[i] - mb1) * (arrX[i] - mb1) * (arrX[i] - mb1);
                mc4 += (arrX[i] - mb1) * (arrX[i] - mb1) * (arrX[i] - mb1) * (arrX[i] - mb1);
            }
            mb3 /= arrX.Length;
            mb4 /= arrX.Length;
            mc1 /= arrX.Length;
            mc3 /= arrX.Length;
            mc4 /= arrX.Length;

            // асим. и эксцесс
            var = sigmaStd / average;
            asym = mc3 / (sigmaStd * sigmaStd * sigmaStd);
            exc = mc4 / (sigmaStd * sigmaStd * sigmaStd * sigmaStd) - 3;

            double n = arrX.Length;
            double g1 = mc3 / (double)Math.Pow(mc2, 1.5);
            asymTheor = (double)Math.Sqrt(n * (n - 1)) / (n - 2) * g1;
            sigmaAsym = (double)Math.Sqrt((6 * n * (n - 1)) /
                ((n - 2) * (n + 1) * (n + 3)));
            double g2 = mc4 / (mc2 * mc2) - 3;
            excTheor = (n - 1) / ((n - 2) * (n - 3)) * ((n + 1) * g2 + 6);
            sigmaExc = (double)Math.Sqrt(24 * n * (n - 2) * (n - 3) /
                ((n + 1) * (n + 1) * (n + 3) * (n + 5)));
        }
        public double[] CloneArray()
        {
            return (double[])arrX.Clone();
        }
        public double[] CloneHistY()
        {
            return (double[])arrHistY.Clone();
        }
        public double[] CloneHistX()
        {
            return (double[])arrHistX.Clone();
        }
        public int[] DropoutErrorsTau(double alpha)
        {
            ArrayList listX = new ArrayList(arrX);
            ArrayList listIndexIgnore = new ArrayList();
            while (true)
            {
                double min = double.MaxValue, max = double.MinValue;
                double sum = 0, sumXPow2 = 0;
                int indexMin = -1, indexMax = -1;
                for (int i = 0; i < listX.Count; i++)
                {
                    if (listIndexIgnore.BinarySearch(i) >= 0)
                        continue;
                    double x = (double)listX[i];
                    if (x < min)
                    {
                        min = x;
                        indexMin = i;
                    }
                    if (x > max)
                    {
                        max = x;
                        indexMax = i;
                    }
                    sum += x;
                    sumXPow2 += x * x;
                }
                int n = listX.Count - listIndexIgnore.Count;
                double average = sum / n;
                double devStd = sumXPow2 / (n - 1) - (sum / (n - 1)) * (sum / (n - 1));
                double sigmaStd = (double)Math.Sqrt(devStd);
                double tauXMin, tauXMax;
                tauXMin = (double)Math.Abs(min - average) / sigmaStd;
                tauXMax = (double)Math.Abs(max - average) / sigmaStd;
                int index;
                double tauMax;
                if (tauXMin >= tauXMax)
                {
                    index = indexMin;
                    tauMax = tauXMin;
                }
                else
                {
                    index = indexMax;
                    tauMax = tauXMax;
                }
                if (tauMax > StatTables.GetTau(alpha, n))
                {
                    listIndexIgnore.Add(index);
                    listIndexIgnore.Sort();
                }
                else
                    break;
            }
            return (int[])listIndexIgnore.ToArray(typeof(int));
        }
        public int[] DropoutErrorsStud(double alphaMin, double alphaMax)
        {
            if (alphaMin == -1)
                alphaMin = 0.001f;
            if (alphaMax == -1)
                alphaMax = 0.05f;
            ArrayList listX = new ArrayList(arrX);
            ArrayList listIndexIgnore = new ArrayList();
            while (true)
            {
                double min = double.MaxValue, max = double.MinValue;
                double sum = 0, sumXPow2 = 0;
                int indexMin = -1, indexMax = -1;
                for (int i = 0; i < listX.Count; i++)
                {
                    if (listIndexIgnore.BinarySearch(i) >= 0)
                        continue;
                    double x = (double)listX[i];
                    if (x < min)
                    {
                        min = x;
                        indexMin = i;
                    }
                    if (x > max)
                    {
                        max = x;
                        indexMax = i;
                    }
                    sum += x;
                    sumXPow2 += x * x;
                }
                int n = listX.Count - listIndexIgnore.Count;
                double average = sum / n;
                double devStd = sumXPow2 / (n - 1) - (sum / (n - 1)) * (sum / (n - 1));
                double sigmaStd = (double)Math.Sqrt(devStd);
                double tauXMin, tauXMax;
                tauXMin = (double)Math.Abs(min - average) / sigmaStd;
                tauXMax = (double)Math.Abs(max - average) / sigmaStd;
                int index;
                double tauMax;
                if (tauXMin >= tauXMax)
                {
                    index = indexMin;
                    tauMax = tauXMin;
                }
                else
                {
                    index = indexMax;
                    tauMax = tauXMax;
                }

                double tMin = StatTables.GetStudDistrInv(listX.Count - 2, alphaMin);
                double tMax = StatTables.GetStudDistrInv(listX.Count - 2, alphaMax);
                double critMin = tMin * (double)Math.Sqrt(listX.Count - 1) /
                    (double)Math.Sqrt(listX.Count - 2 + tMin * tMin);
                double critMax = tMax * (double)Math.Sqrt(listX.Count - 1) /
                    (double)Math.Sqrt(listX.Count - 2 + tMin * tMin);
                if (tauMax > critMin)
                {
                    listIndexIgnore.Add(index);
                    listIndexIgnore.Sort();
                }
                else
                    break;
            }
            return (int[])listIndexIgnore.ToArray(typeof(int));
        }
        public void DoHistogram(bool useSturgess)
        {
            int k;
            if (useSturgess)
                k = 1 + (int)(3.32 * Math.Log10(arrX.Length));
            else
                k = (int)Math.Sqrt(arrX.Length);
            if (k < 6 && arrX.Length >= 6)
                k = 6;
            else if (k > 20)
                k = 20;

            arrHistY = new double[k];
            double h = (max - min) / k;
            for (int i = 0; i < arrX.Length; i++)
            {
                int j;
                for (j = 0; j < arrHistY.Length - 1; j++)
                    if (arrX[i] < min + h * (j + 1))
                        break;
                arrHistY[j]++;
            }

            arrHistX = new double[k];
            for (int i = 0; i < arrHistX.Length; i++)
                arrHistX[i] = min + h * (i + 0.5);
        }
        public void AddValue(double value)
        {
            double[] arrXNew = new double[arrX.Length + 1];
            arrX.CopyTo(arrXNew, 0);
            arrXNew[arrX.Length] = value;
            arrX = arrXNew;
        }
        public void RemoveValues(int[] arrIndex)
        {
            double[] arrXNew = new double[arrX.Length - arrIndex.Length];
            ArrayList listIndex = new ArrayList(arrIndex);
            listIndex.Sort();
            for (int i = 0, j = 0; i < arrX.Length; i++)
            {
                if (listIndex.BinarySearch(i) >= 0)
                    continue;
                arrXNew[j++] = arrX[i];
            }
            arrX = arrXNew;
        }
        public string GetName()
        {
            return name;
        }
        public virtual string GetMark()
        {
            return mark;
        }
        public double GetAverage()
        {
            return average;
        }
        public double GetSigma()
        {
            return sigma;
        }
        public double GetDevStd()
        {
            return devStd;
        }
        public int GetLength()
        {
            return arrX.Length;
        }
        public string GetReport()
        {
            int d = 3;
            string s = string.Format("<P>ВЫБОРОЧНЫЕ ХАРАКТЕРИСТИКИ {0} [={1}]<BR>" +
                "Минимум: {1}<SUB>min</SUB> = {2}<BR>" +
                "Максимум: {1}<SUB>max</SUB> = {3}<BR>" +
                "Размах выборки: w = {4}<BR>" +
                "Среднее: {1}<SUB>ср</SUB> = {5}<BR>" +
                "Средний квадрат: {1}<SUP>2</SUP><SUB>ср</SUB> = {6}<BR>" +
                "Дисперсия: s<SUP>2</SUP> = {7}<BR>" +
                "Среднее квадр. откл.: s = {8}<BR>" +
                "Испр. дисперсия: s<SUP>2</SUP><SUB>испр</SUB> = {9}<BR>" +
                "Испр. среднее квадр. откл.: s<SUB>испр</SUB> = {10}<BR>" +
                "Асимметрия: A = {11}<BR>" +
                "Эксцесс: E = {12}<BR>" +
                "Коэффициент вариации: v = {13}<BR></P>",
                name, mark, Math.Round(min, d), Math.Round(max, d),
                Math.Round(min - max, d), Math.Round(average, d),
                Math.Round(averageXPow2, d), Math.Round(dev, d), Math.Round(sigma, d),
                Math.Round(devStd, d), Math.Round(sigmaStd, d), Math.Round(asym, d),
                Math.Round(exc, d), Math.Round(var, d));
            return s;
        }
        public string GetHypReport()
        {
            int d = 3;
            string asymRepr = "|A| < 3s<SUB>A</SUB> => " +
                "гипотеза о нормальности не отвергается";
            string excRepr = "|E| < 5s<SUB>E</SUB> => " +
                "гипотеза о нормальности не отвергается";
            string varRepr = "v < 0.33 => гипотеза о нормальности не отвергается";
            if (Math.Abs(asym) >= 3 * sigmaAsym)
                asymRepr = "|A| ≥ 3s<SUB>A</SUB> => " +
                    "гипотеза о нормальности отвергается";
            if (Math.Abs(exc) >= 5 * sigmaExc)
                excRepr = "|E| ≥ 5s<SUB>E</SUB> => " +
                    "гипотеза о нормальности отвергается";
            if (var >= 0.33)
                varRepr = "v ≥ 0.33 => гипотеза о нормальности отвергается";

            string s = string.Format("<P>ПРИЗНАК: {0} [={1}]<BR>", name, mark);
            s += string.Format("<P>Показатель асимметрии:<BR>" +
            "наблюдаемый: A = {0}<BR>" +
            "ошибка: s<SUB>A</SUB> = {1}<BR>{2}</P>",
            Math.Round(asym, 3), Math.Round(sigmaAsym, d), asymRepr);
            s += string.Format("<P>Показатель эксцесса:<BR>" +
                "наблюдаемый: E = {0}<BR>" +
                "ошибка: s<SUB>E</SUB> = {1}<BR>{2}</P>",
                Math.Round(exc, d), Math.Round(sigmaExc, d), excRepr);
            s += string.Format("<P>Коэффициент вариации:<BR>" +
                "наблюдаемый: v = {0}<BR>{1}</P>",
                Math.Round(var, d), varRepr);
            return s;
        }
        public void SetName(string name)
        {
            this.name = name;
        }
        public object Clone()
        {
            return new Sample(name, mark, arrX);
        }
    }
    public class TranSample : Sample
    {
        string transform;
        public TranSample(Sample s, string transform)
        {
            this.transform = transform;
            arrX = new double[s.GetLength()];
            name = s.GetName();
            mark = s.GetMark();
            switch (transform)
            {
                case "x^2":
                    for (int i = 0; i < arrX.Length; i++)
                        arrX[i] = s[i] * s[i];
                    break;
                case "x^3":
                    for (int i = 0; i < arrX.Length; i++)
                        arrX[i] = s[i] * s[i] * s[i];
                    break;
                case "1/x":
                    for (int i = 0; i < arrX.Length; i++)
                        arrX[i] = 1 / s[i];
                    break;
                case "1/x^2":
                    for (int i = 0; i < arrX.Length; i++)
                        arrX[i] = 1 / (s[i] * s[i]);
                    break;
                case "sqrt(x)":
                    for (int i = 0; i < arrX.Length; i++)
                        arrX[i] = Math.Sqrt(s[i]);
                    break;
                case "1/sqrt(x)":
                    for (int i = 0; i < arrX.Length; i++)
                        arrX[i] = 1 / Math.Sqrt(s[i]);
                    break;
                case "ln(x)":
                    for (int i = 0; i < arrX.Length; i++)
                        arrX[i] = Math.Log(s[i]);
                    break;
                case "exp(x)":
                    for (int i = 0; i < arrX.Length; i++)
                        arrX[i] = Math.Exp(s[i]);
                    break;
                case "x":
                    for (int i = 0; i < arrX.Length; i++)
                        arrX[i] = s[i];
                    break;
                default:
                    throw new Exception();
            }
            for (int i = 0; i < arrX.Length; i++)
                if (double.IsInfinity(arrX[i]) || double.IsNaN(arrX[i]))
                {
                    this.transform = "x";
                    arrX = s.CloneArray();
                }
            Calculate();
        }
        public string GetTransform()
        {
            return transform;
        }
        public override string ToString()
        {
            return transform;
        }
        public override string GetMark()
        {
            switch (transform)
            {
                case "x^2":
                    return string.Format("{0}<sup>2</sup>", base.GetMark());
                case "x^3":
                    return string.Format("{0}<sup>3</sup>", base.GetMark());
                case "1/x":
                    return string.Format("{0}<sup>-1</sup>", base.GetMark());
                case "1/x^2":
                    return string.Format("{0}<sup>-2</sup>", base.GetMark());
                case "sqrt(x)":
                    return string.Format("{0}<sup>1/2</sup>", base.GetMark());
                case "1/sqrt(x)":
                    return string.Format("{0}<sup>-1/2</sup>", base.GetMark());
                case "ln(x)":
                    return string.Format("ln({0})", base.GetMark());
                case "exp(x)":
                    return string.Format("exp({0})", base.GetMark());
                case "x":
                    return base.GetMark();
                default:
                    throw new Exception();
            }
        }
    }
    public class Regression
    {
        double[] arrYMod;
        public double[] arrB;      // коэффициенты регрессии
        double[,] matrC;    // матр. сист. норм. ур.
        double[,] matrCInv; // обр. матр. C
        double[,] matrR;    // матрица коэф. кор.
        double[,] matrRPart;// матр. част. коэфф. кор.
        double R, sigmaR;   // коэф. множ. кор.
        double fishRegr, fishRegrTheor;    // крит. Фиш. при пров. знач. регр.
        double studR, studRTheor;   // крит. Стьюд. при пров. R
        double fishR, fishRTheor; // крит. Фиш. при пров. R
        double devEnd;      // ост. дисп.
        double[] arrSigmaB; // дисп. и ср. кв. откл. коэф. B
        double[] arrStudB;  // крит. Стьюд. при пров. знач. B
        double studBTheor;
        double[] arrStudRxy;    // крит. Стьюд. при пров. знач. парн. коэф. кор. y и xi
        double[] arrSigmaRxy;
        double studRxyTheor;
        int n, p;           // кол. набл. и кол. факторов
        bool isRegrRepr, isRReprStud, isRReprFish;  // результаты пров. гип.
        bool[] arrIsBRepr;
        bool[] arrIsRxyRepr;
        string[] arrMark;   // назв. перем. (снач. y, потом все x)

        public Regression(Sample smpY, Sample[] arrSmpX)
        {
            double[] arrY = smpY.CloneArray();
            n = arrY.Length;
            p = arrSmpX.Length;
            double[][] matrX = new double[p + 1][];
            matrX[0] = new double[n];
            for (int j = 0; j < matrX[0].Length; j++)
                matrX[0][j] = 1;
            for (int i = 1; i < p + 1; i++)
                matrX[i] = arrSmpX[i - 1].CloneArray();
            Matrix mX = Matrix.Create(matrX);
            mX.Transpose();
            Matrix mY = Matrix.Create(new double[][] { arrY });
            mY.Transpose();
            Matrix mXTr = mX.Clone();
            mXTr.Transpose();
            Matrix mB = mXTr * mX;
            matrC = mB.CopyToArray();
            mB = mB.Inverse();
            matrCInv = mB.CopyToArray();
            Matrix mXTrY = mXTr * mY;
            mB = mB * mXTrY;
            arrB = new double[mB.RowCount];
            for (int i = 0; i < mB.RowCount; i++)
                arrB[i] = mB[i, 0];

            // матр. коэф. кор.
            int smpCount = p + 1;
            ArrayList listSmp = new ArrayList(arrSmpX);
            listSmp.Insert(0, smpY);
            matrR = new double[smpCount, smpCount];
            for (int i = 0; i < smpCount; i++)
                for (int j = 0; j <= i; j++)
                {
                    if (i == j)
                    {
                        matrR[i, j] = 1;
                        continue;
                    }
                    Sample smp1 = (Sample)listSmp[i];
                    Sample smp2 = (Sample)listSmp[j];
                    double sum = 0;
                    for (int k = 0; k < n; k++)
                        sum += smp1[k] * smp2[k];
                    matrR[i, j] = (sum / n - smp1.GetAverage() *
                        smp2.GetAverage()) / (smp1.GetSigma() *
                        smp2.GetSigma());
                }
            for (int i = 0; i < smpCount; i++)
                for (int j = i + 1; j < smpCount; j++)
                    matrR[i, j] = matrR[j, i];

            // пров. знач. коэф. кор.
            arrStudRxy = new double[p];
            arrSigmaRxy = new double[p];
            for (int i = 0; i < arrStudRxy.Length; i++)
            {
                double r = matrR[0, i + 1];
                arrSigmaRxy[i] = (1 - r) / Math.Sqrt(n - 1);
                arrStudRxy[i] = r / arrSigmaRxy[i];
            }

            // матр. част. коэф. кор.
            matrRPart = new double[smpCount, smpCount];
            Matrix mR = Matrix.Create(matrR);
            double minor11 = GetMinor(mR, 0, 0);
            for (int i = 0; i < mR.RowCount; i++)
                for (int j = 0; j < i; j++)
                    matrRPart[i, j] = GetMinor(mR, 1, j) /
                        Math.Sqrt(minor11 * GetMinor(mR, j, j));
            for (int i = 0; i < smpCount; i++)
                for (int j = i + 1; j < smpCount; j++)
                    matrRPart[i, j] = matrRPart[j, i];

            // коэф. множ. кор.
            //R = Math.Sqrt(1 - devEnd / smpY.GetDevStd());
            R = Math.Sqrt(1 - mR.Determinant() / minor11);
            sigmaR = (1 - R * R) / Math.Sqrt(n - p - 1);
            fishR = R * R * (n - p - 1) /
                (1 - R * R) * p;
            studR = R / sigmaR;

            // эмп. знач. y
            arrYMod = new double[n];
            double[] arrX = new double[p];
            for (int i = 0; i < arrYMod.Length; i++)
            {
                for (int j = 0; j < arrX.Length; j++)
                    arrX[j] = arrSmpX[j][i];
                arrYMod[i] = GetRegValue(arrX);
            }

            // ост. дисп.
            devEnd = 0;
            for (int i = 0; i < n; i++)
                devEnd += (arrY[i] - arrYMod[i]) * (arrY[i] - arrYMod[i]);
            devEnd /= n - p - 1;

            // пров. знач. ур. регр. и коэф. ур. регр.
            fishRegr = smpY.GetDevStd() / devEnd;
            arrSigmaB = new double[p + 1];
            arrStudB = new double[p + 1];
            for (int i = 0; i < arrSigmaB.Length; i++)
            {
                arrSigmaB[i] = Math.Sqrt(devEnd * matrCInv[i, i]);
                arrStudB[i] = arrB[i] / arrSigmaB[i];
            }

            // названия переменных
            arrMark = new string[p + 1];
            arrMark[0] = smpY.GetMark();
            for (int i = 1; i < arrMark.Length; i++)
                arrMark[i] = arrSmpX[i - 1].GetMark();
        }
        public void CheckHypothesises(double alpha)
        {
            // знач. регр.
            fishRegrTheor = StatTables.GetFishDistrInv(n - 1, n - p - 1, alpha);
            if (fishRegr > fishRegrTheor)
                isRegrRepr = true;
            else
                isRegrRepr = false;

            // знач. коэф. регр.
            studBTheor = StatTables.GetStudDistrInv(n - p - 1, alpha);
            arrIsBRepr = new bool[p + 1];
            for (int i = 0; i < arrIsBRepr.Length; i++)
                if (arrStudB[i] > studBTheor)
                    arrIsBRepr[i] = true;
                else
                    arrIsBRepr[i] = false;

            // знач. коэф. кор.
            studRxyTheor = StatTables.GetStudDistrInv(n - 2, alpha);
            arrIsRxyRepr = new bool[p];
            for (int i = 0; i < arrIsRxyRepr.Length; i++)
                if (arrStudRxy[i] > studRxyTheor)
                    arrIsRxyRepr[i] = true;
                else
                    arrIsRxyRepr[i] = false;

            // знач. множ. коэф. кор.
            studRTheor = StatTables.GetStudDistrInv(n - p - 1, alpha);
            if (studR > studRTheor)
                isRReprStud = true;
            else
                isRReprStud = false;

            fishRTheor = StatTables.GetFishDistrInv(n - p - 1, p, alpha);
            if (fishR > fishRTheor)
                isRReprFish = true;
            else
                isRReprFish = false;

        }
        public double GetRegValue(double[] arrX)
        {
            double res = arrB[0];
            for (int i = 0; i < arrX.Length; i++)
                res += arrX[i] * arrB[i + 1];
            return res;
        }
        public double[] CloneArray()
        {
            return (double[])arrYMod.Clone();
        }
        public string GetRegReport()
        {
            int d = 3;
            string s = string.Format("<P>ПОКАЗАТЕЛЬ: {0}<BR>", arrMark[0]);
            s += string.Format("<P>ПОСТРОЕНИЕ УРАВНЕНИЯ РЕГРЕССИИ<BR>" +
                "Матрица системы нормальных уравнений:<BR>{1}<BR>" +
                "Обратная матрица системы нормальных уравнений:<BR>{2}<BR>" +
                "Уравнение регрессии:<BR> {3} = ", arrMark[0],
                MatrixToHtml(matrC, d), MatrixToHtml(matrCInv, d), arrMark[0]);
            s += Math.Round(arrB[0], d).ToString();
            for (int i = 1; i < arrB.Length; i++)
            {
                if (arrB[i] >= 0)
                    s += "+";
                s += Math.Round(arrB[i], d).ToString() + arrMark[i];
            }
            s += string.Format("<BR>Остаточная дисперсия: s<SUB>ост</SUB> = {0}<BR>" +
                "Коэффициент множественной корреляции: R = {1}</P>",
                Math.Round(devEnd, d), Math.Round(R, d));
            return s;
        }
        public string GetHypRegrReport()
        {
            int d = 3;
            string repr = "F ≤ F<SUB>теор</SUB> => уравнение регрессии не значимо";
            if (isRegrRepr)
                repr = "F > F<SUB>теор</SUB> => уравнение регрессии значимо";
            string s = string.Format("<P>ПОКАЗАТЕЛЬ: {0}<BR>", arrMark[0]);
            s += string.Format("<P>ПРОВЕРКА ЗНАЧИМОСТИ УРАВНЕНИЯ РЕГРЕССИИ<BR>" +
                "Наблюдаемое значение критерия Фишера:<BR>F = {0}<BR>" +
                "Теоретическое значение критерия Фишера:<BR>F<SUB>теор</SUB> = {1}<BR>" +
                "{2}</P>", Math.Round(fishRegr, d), Math.Round(fishRegrTheor, d), repr, arrMark[0]);
            s += string.Format("<P>ПРОВЕРКА ЗНАЧИМОСТИ КОЭФФИЦИЕНТОВ УРАВНЕНИЯ РЕГРЕССИИ<BR>" +
                "Теоретическое значение критерия Стьюдента:<BR>t<SUB>теор</SUB> = {0}<BR>",
                Math.Round(studBTheor, d));
            for (int i = 0; i < arrIsBRepr.Length; i++)
            {
                repr = "коэффициент не значим";
                if (arrIsBRepr[i])
                    repr = "коэффициент значим";
                s += string.Format("b<SUB>{0}</SUB>: t<SUB>набл</SUB> = {1} => {2}<BR>",
                    i, Math.Round(arrStudB[i], d), repr);
            }
            return s;
        }
        public string GetCorrReport()
        {
            int d = 3;
            string s = string.Format("<P>ПОКАЗАТЕЛЬ: {0}<BR>", arrMark[0]);
            s += string.Format("<P>КОРРЕЛЯЦИОННЫЙ АНАЛИЗ<BR>{0}",
                 MatrixToHtmlHeaders(matrR, d, arrMark),
                 MatrixToHtmlHeaders(matrRPart, d, arrMark),
                 Math.Round(R, d), Math.Round(sigmaR, d), arrMark[0]);
            return s;
        }
        public string GetHypCorrReport()
        {
            int d = 3;
            string repr = "t < t<SUB>теор</SUB> => R не значим";
            if (isRReprStud)
                repr = "t ≥ t<SUB>теор</SUB> => R значим";
            string s = string.Format("<P>ПОКАЗАТЕЛЬ: {0}<BR>", arrMark[0]);
            s += string.Format("<P>ПРОВЕРКА ЗНАЧИМОСТИ R ПО КРИТЕРИЮ СТЬЮДЕНТА<BR>" +
                "Наблюдаемое значение:<BR>t = {0}<BR>" +
                "Теоретическое значение:<BR>t<SUB>теор</SUB> = {1}<BR>" +
                "{2}</P>", Math.Round(studR, d), Math.Round(studRTheor, d), repr);

            repr = "F ≤ F<SUB>теор</SUB> => R не значим";
            if (isRReprFish)
                repr = "F > F<SUB>теор</SUB> => R значим";
            s += string.Format("<P>ПРОВЕРКА ЗНАЧИМОСТИ R ПО КРИТЕРИЮ ФИШЕРА<BR>" +
                "Наблюдаемое значение:<BR>F = {0}<BR>" +
                "Теоретическое значение:<BR>F<SUB>теор</SUB> = {1}<BR>" +
                "{2}</P>", Math.Round(fishR, d), Math.Round(fishRTheor, d), repr);

            s += string.Format("<P>ПРОВЕРКА ЗНАЧИМОСТИ КОЭФФИЦИЕНТОВ КОРРЕЛЯЦИИ<BR>" +
                "Теоретическое значение критерия Стьюдента: t<SUB>теор</SUB> = {0}<BR>",
                Math.Round(studRxyTheor, d));
            for (int i = 0; i < arrIsRxyRepr.Length; i++)
            {
                repr = "коэффициент не значим";
                if (arrIsRxyRepr[i])
                    repr = "коэффициент значим";
                s += string.Format("r<SUB>{0}{1}</SUB>: t<SUB>набл</SUB> = {2} => {3}<BR>",
                    arrMark[0], arrMark[i + 1], Math.Round(arrStudRxy[i], d), repr);
            }
            s += "</P>";
            return s;
        }
        public double GetR()
        {
            return R;
        }
        public double GetR_yx1()
        {
            return matrR[0, 1];
        }
        double GetMinor(Matrix m, int rowIndex, int colIndex)
        {
            Matrix mTmp = new Matrix(m.RowCount - 1, m.ColumnCount - 1);
            for (int i = 0; i < mTmp.RowCount; i++)
                for (int j = 0; j < mTmp.ColumnCount; j++)
                {
                    int i1 = i, j1 = j;
                    if (i >= rowIndex)
                        i1++;
                    if (j >= colIndex)
                        j1++;
                    mTmp[i, j] = m[i1, j1];
                }
            return mTmp.Determinant();
        }
        string MatrixToHtml(double[,] matr, int d)
        {
            string s = "";
            s += "<TABLE border = 1 cellspacing = 0 align = center>";
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                s += "<TR>";
                for (int j = 0; j < matr.GetLength(1); j++)
                    s += "<TD>" + Math.Round(matr[i, j], d).ToString() + "</TD>";
                s += "</TR>";
            }
            s += "</TABLE>";
            return s;
        }
        string MatrixToHtmlHeaders(double[,] matr, int d, string[] arrHeader)
        {
            string s = "";
            s += "<TABLE border = 1 cellspacing = 0 align = center><TR><TD></TD>";
            for (int i = 0; i < arrMark.Length; i++)
                s += "<TD>" + arrMark[i] + "</TD>";
            s += "</TR>";
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                s += "<TR><TD>" + arrMark[i] + "</TD>";
                for (int j = 0; j < matr.GetLength(1); j++)
                    s += "<TD>" + Math.Round(matr[i, j], d).ToString() + "</TD>";
                s += "</TR>";
            }
            s += "</TABLE>";
            return s;
        }
    }
    public class StatTables
    {
        static double epsilon = 0.0000001;
        static StudentsTDistribution distStud = new StudentsTDistribution();
        static FisherSnedecorDistribution distFish = new FisherSnedecorDistribution();
        static public double GetStudDistrInv(int n, double alpha)
        {
            distStud.DegreesOfFreedom = n;
            return GetX(1 - alpha, distStud);
        }
        static public double GetFishDistrInv(int n1, int n2, double alpha)
        {
            distFish.Alpha = n1;
            distFish.Beta = n2;
            return GetX(1 - alpha, distFish);
        }
        public static double GetTau(double p, double v)
        {
            int i, j;
            for (i = arrP.Length - 1; i >= 0; i--)
                if (arrP[i] >= p)
                    break;
            for (j = arrV.Length - 1; j >= 0; j--)
                if (arrV[j] <= v)
                    break;
            return arrTau[j, i];
        }
        static double GetX(double p, ContinuousDistribution dist)
        {
            double x, xNext = 1;
            do
            {
                x = xNext;
                xNext = x - (dist.CumulativeDistribution(x) - p) /
                    dist.ProbabilityDensity(x);
            }
            while (Math.Abs(x - xNext) > epsilon);
            return xNext;
        }

        static double[] arrP = new double[]
                {
                    0.10f, 0.05f, 0.025f, 0.01f
                };
        static int[] arrV = new int[]
                {
                    3, 4, 5, 6, 7, 8, 9, 10,
                    11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                    21, 22, 23
                };
        static double[,] arrTau = new double[,]
                {
                    { 1.41f, 1.41f, 1.41f, 1.41f },
                    { 1.65f, 1.69f, 1.71f, 1.72f },
                    { 1.79f, 1.87f, 1.92f, 1.96f },
                    { 1.89f, 2.00f, 2.07f, 2.13f },
                    { 1.97f, 2.09f, 2.18f, 2.27f },
                    { 2.04f, 2.17f, 2.27f, 2.37f },
                    { 2.10f, 2.24f, 2.35f, 2.46f },
                    { 2.15f, 2.29f, 2.41f, 2.54f },                    
                    { 2.19f, 2.34f, 2.47f, 1.41f },
                    { 2.23f, 2.39f, 2.52f, 1.72f },
                    { 2.26f, 2.43f, 2.56f, 1.96f },
                    { 2.30f, 2.46f, 2.60f, 2.13f },                    
                    { 2.33f, 2.49f, 2.64f, 2.80f },
                    { 2.35f, 2.52f, 2.67f, 2.84f },
                    { 2.38f, 2.55f, 2.70f, 2.87f },
                    { 2.40f, 2.58f, 2.73f, 2.90f },
                    { 2.43f, 2.60f, 2.75f, 2.93f },
                    { 2.45f, 2.62f, 2.78f, 2.96f },
                    { 2.47f, 2.64f, 2.80f, 2.98f },
                    { 2.49f, 2.66f, 2.82f, 3.01f },
                    { 2.50f, 2.68f, 2.84f, 3.03f },
                    { 2.52f, 2.70f, 2.86f, 3.05f },
                    { 2.54f, 2.72f, 2.88f, 3.07f }
                };
    }
    public class Linearizer
    {
        int[] dims;
        int maxIndex;
        public Linearizer(int[] dims)
        {
            this.dims = dims;
            maxIndex = 1;
            for (int i = 0; i < dims.Length; i++)
                maxIndex *= (int)dims[i];
        }
        public int GetIndex(int[] indexes)
        {
            if (indexes.Length != dims.Length)
                throw new Exception();
            int index = 0, dim = 1;
            for (int i = 0; i < dims.Length; i++)
            {
                if (indexes[i] >= dims[i])
                    throw new Exception();
                index += indexes[i] * dim;
                dim *= dims[i];
            }
            return (int)index;
        }
        public int[] GetIndexes(int index)
        {
            int[] indexes = new int[dims.Length];
            int dim = maxIndex;
            for (int i = dims.Length - 1; i >= 0; i--)
            {
                dim /= (int)dims[i];
                int j = (int)(index / dim);
                if (j >= dims[i])
                    throw new Exception();
                indexes[i] = j;
                index -= (int)(j * dim);
            }
            return indexes;
        }
    }

    public class Settings
    {
        public double[] arrYMin, arrMax;
        public void Read()
        {
        }
    }
}