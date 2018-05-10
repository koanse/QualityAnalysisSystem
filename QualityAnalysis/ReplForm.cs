using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QualityDim
{
    public partial class ReplForm : Form
    {
        public string[] arrRepl;
        public ReplForm()
        {
            InitializeComponent();
            for (int i = 0; i < Properties.Settings.Default.arrRepl.Count; i++)
			{
                dgv.Rows.Add(Properties.Settings.Default.arrRepl[i]);			 
			}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dgv.RowCount - 1; i++)
                {
                    
                    string[] arrStr = dgv[0, i].Value.ToString().Split(new char[] { ' ' },
                        StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < arrStr.Length; j++)
                    {
                        int.Parse(arrStr[j]);
                    }
                }
                arrRepl = new string[dgv.RowCount - 1];
                Properties.Settings.Default.arrRepl.Clear();
                for (int i = 0; i < dgv.RowCount - 1; i++)
                {
                    Properties.Settings.Default.arrRepl.Add(dgv[0, i].Value.ToString());
                    arrRepl[i] = dgv[0, i].Value.ToString();
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
