﻿namespace QualityDim
{
    partial class H2DPropForm
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
            this.tbY1Min = new System.Windows.Forms.TextBox();
            this.tbY1Max = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbY2Max = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbY2Min = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(371, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Номер интервала начала зоны особого качества по первому признаку:";
            // 
            // tbY1Min
            // 
            this.tbY1Min.Location = new System.Drawing.Point(12, 25);
            this.tbY1Min.Name = "tbY1Min";
            this.tbY1Min.Size = new System.Drawing.Size(375, 20);
            this.tbY1Min.TabIndex = 1;
            // 
            // tbY1Max
            // 
            this.tbY1Max.Location = new System.Drawing.Point(12, 64);
            this.tbY1Max.Name = "tbY1Max";
            this.tbY1Max.Size = new System.Drawing.Size(375, 20);
            this.tbY1Max.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(366, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Номер интервала конца зоны особого качества по первому признаку:";
            // 
            // tbY2Max
            // 
            this.tbY2Max.Location = new System.Drawing.Point(12, 142);
            this.tbY2Max.Name = "tbY2Max";
            this.tbY2Max.Size = new System.Drawing.Size(375, 20);
            this.tbY2Max.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(365, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Номер интервала конца зоны особого качества по второму признаку:";
            // 
            // tbY2Min
            // 
            this.tbY2Min.Location = new System.Drawing.Point(12, 103);
            this.tbY2Min.Name = "tbY2Min";
            this.tbY2Min.Size = new System.Drawing.Size(375, 20);
            this.tbY2Min.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(370, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Номер интервала начала зоны особого качества по второму признаку:";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(231, 168);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(312, 168);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // H2DPropForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 203);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tbY2Max);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbY2Min);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbY1Max);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbY1Min);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "H2DPropForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Параметры построения гистограммы";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.TextBox tbY1Min;
        public System.Windows.Forms.TextBox tbY1Max;
        public System.Windows.Forms.TextBox tbY2Max;
        public System.Windows.Forms.TextBox tbY2Min;
    }
}