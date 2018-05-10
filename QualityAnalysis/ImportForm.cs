using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace QualityDim
{
    public partial class ImportForm : Form
    {
        public double[,] matrX, matrY;
        public string[] arrXName, arrYName;
        public double[] arrYAv, arrXAv;
        public ImportForm()
        {
            InitializeComponent();
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                Quality.ImportData(tbF.Text, tbSx.Text, tbSy.Text, tbRx.Text, tbRy.Text,
                    out matrX, out matrY, out arrXName, out arrYName);
                Quality.Norm(matrY, out arrYAv);
                Quality.Norm(matrX, out arrXAv);
                DialogResult = DialogResult.OK;
                Properties.Settings.Default.Save();
            }
            catch
            {
                MessageBox.Show("Ошибка импорта данных");
            }
        }
        void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() != DialogResult.OK)
                    return;
                tbF.Text = openFileDialog1.FileName;

            }
            catch
            {
                MessageBox.Show("Ошибка отрытия файла");
            }
        }
        void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}