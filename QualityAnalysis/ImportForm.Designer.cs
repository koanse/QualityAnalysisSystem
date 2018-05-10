namespace QualityDim
{
    partial class ImportForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.btnOpen = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tbRy = new System.Windows.Forms.TextBox();
            this.tbSy = new System.Windows.Forms.TextBox();
            this.tbRx = new System.Windows.Forms.TextBox();
            this.tbSx = new System.Windows.Forms.TextBox();
            this.tbF = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Файл MS Excel:";
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(346, 51);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 2;
            this.btnOpen.Text = "Изменить";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbRx);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbSx);
            this.groupBox1.Location = new System.Drawing.Point(12, 80);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Технологические факторы";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Диапазон:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Имя рабочего листа:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tbRy);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tbSy);
            this.groupBox2.Location = new System.Drawing.Point(221, 80);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 100);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Показатели качества";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Диапазон:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Имя рабочего листа:";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(265, 186);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(346, 186);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tbRy
            // 
            this.tbRy.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::QualityDim.Properties.Settings.Default, "ry", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbRy.Location = new System.Drawing.Point(6, 71);
            this.tbRy.Name = "tbRy";
            this.tbRy.Size = new System.Drawing.Size(188, 20);
            this.tbRy.TabIndex = 6;
            this.tbRy.Text = global::QualityDim.Properties.Settings.Default.ry;
            // 
            // tbSy
            // 
            this.tbSy.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::QualityDim.Properties.Settings.Default, "sy", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbSy.Location = new System.Drawing.Point(6, 32);
            this.tbSy.Name = "tbSy";
            this.tbSy.Size = new System.Drawing.Size(188, 20);
            this.tbSy.TabIndex = 4;
            this.tbSy.Text = global::QualityDim.Properties.Settings.Default.sy;
            // 
            // tbRx
            // 
            this.tbRx.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::QualityDim.Properties.Settings.Default, "rx", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbRx.Location = new System.Drawing.Point(6, 71);
            this.tbRx.Name = "tbRx";
            this.tbRx.Size = new System.Drawing.Size(188, 20);
            this.tbRx.TabIndex = 6;
            this.tbRx.Text = global::QualityDim.Properties.Settings.Default.rx;
            // 
            // tbSx
            // 
            this.tbSx.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::QualityDim.Properties.Settings.Default, "sx", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbSx.Location = new System.Drawing.Point(6, 32);
            this.tbSx.Name = "tbSx";
            this.tbSx.Size = new System.Drawing.Size(188, 20);
            this.tbSx.TabIndex = 4;
            this.tbSx.Text = global::QualityDim.Properties.Settings.Default.sx;
            // 
            // tbF
            // 
            this.tbF.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::QualityDim.Properties.Settings.Default, "file", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbF.Location = new System.Drawing.Point(12, 25);
            this.tbF.Name = "tbF";
            this.tbF.Size = new System.Drawing.Size(409, 20);
            this.tbF.TabIndex = 0;
            this.tbF.Text = global::QualityDim.Properties.Settings.Default.file;
            // 
            // ImportForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 221);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbF);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Импорт данных";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbF;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbRx;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbSx;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbRy;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbSy;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}