namespace QualityDim
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.genToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.histToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.d2HistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.d2HistZoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hist3dToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.d3HistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.d3HistZoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.identToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.d3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lb = new System.Windows.Forms.ListBox();
            this.wb = new System.Windows.Forms.WebBrowser();
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.dataToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(594, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.fileToolStripMenuItem.Text = "Файл";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.importToolStripMenuItem.Text = "Открыть";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.exitToolStripMenuItem.Text = "Выход";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.genToolStripMenuItem,
            this.analizeToolStripMenuItem,
            this.histToolStripMenuItem,
            this.hist3dToolStripMenuItem,
            this.identToolStripMenuItem,
            this.d3ToolStripMenuItem});
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.dataToolStripMenuItem.Text = "Анализ";
            // 
            // genToolStripMenuItem
            // 
            this.genToolStripMenuItem.Name = "genToolStripMenuItem";
            this.genToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
            this.genToolStripMenuItem.Text = "Модель технологии";
            this.genToolStripMenuItem.Click += new System.EventHandler(this.genToolStripMenuItem_Click);
            // 
            // analizeToolStripMenuItem
            // 
            this.analizeToolStripMenuItem.Name = "analizeToolStripMenuItem";
            this.analizeToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
            this.analizeToolStripMenuItem.Text = "Анализ качества для реплик";
            this.analizeToolStripMenuItem.Click += new System.EventHandler(this.analizeToolStripMenuItem_Click);
            // 
            // histToolStripMenuItem
            // 
            this.histToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.d2HistToolStripMenuItem,
            this.d2HistZoneToolStripMenuItem});
            this.histToolStripMenuItem.Name = "histToolStripMenuItem";
            this.histToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
            this.histToolStripMenuItem.Text = "Двухмерная гистограмма";
            // 
            // d2HistToolStripMenuItem
            // 
            this.d2HistToolStripMenuItem.Name = "d2HistToolStripMenuItem";
            this.d2HistToolStripMenuItem.Size = new System.Drawing.Size(276, 22);
            this.d2HistToolStripMenuItem.Text = "Гистограмма";
            this.d2HistToolStripMenuItem.Click += new System.EventHandler(this.d2HistToolStripMenuItem_Click);
            // 
            // d2HistZoneToolStripMenuItem
            // 
            this.d2HistZoneToolStripMenuItem.Name = "d2HistZoneToolStripMenuItem";
            this.d2HistZoneToolStripMenuItem.Size = new System.Drawing.Size(276, 22);
            this.d2HistZoneToolStripMenuItem.Text = "Гистограмма с выделением области";
            this.d2HistZoneToolStripMenuItem.Click += new System.EventHandler(this.d2HistZoneToolStripMenuItem_Click);
            // 
            // hist3dToolStripMenuItem
            // 
            this.hist3dToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.d3HistToolStripMenuItem,
            this.d3HistZoneToolStripMenuItem});
            this.hist3dToolStripMenuItem.Name = "hist3dToolStripMenuItem";
            this.hist3dToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
            this.hist3dToolStripMenuItem.Text = "Трехмерная гистограмма";
            // 
            // d3HistToolStripMenuItem
            // 
            this.d3HistToolStripMenuItem.Name = "d3HistToolStripMenuItem";
            this.d3HistToolStripMenuItem.Size = new System.Drawing.Size(276, 22);
            this.d3HistToolStripMenuItem.Text = "Гистограмма";
            this.d3HistToolStripMenuItem.Click += new System.EventHandler(this.d3HistToolStripMenuItem_Click);
            // 
            // d3HistZoneToolStripMenuItem
            // 
            this.d3HistZoneToolStripMenuItem.Name = "d3HistZoneToolStripMenuItem";
            this.d3HistZoneToolStripMenuItem.Size = new System.Drawing.Size(276, 22);
            this.d3HistZoneToolStripMenuItem.Text = "Гистограмма с выделением области";
            this.d3HistZoneToolStripMenuItem.Click += new System.EventHandler(this.d3HistZoneToolStripMenuItem_Click);
            // 
            // identToolStripMenuItem
            // 
            this.identToolStripMenuItem.Name = "identToolStripMenuItem";
            this.identToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
            this.identToolStripMenuItem.Text = "Идентификация рамочной технологиии";
            this.identToolStripMenuItem.Click += new System.EventHandler(this.identToolStripMenuItem_Click);
            // 
            // d3ToolStripMenuItem
            // 
            this.d3ToolStripMenuItem.Name = "d3ToolStripMenuItem";
            this.d3ToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
            this.d3ToolStripMenuItem.Text = "Трехмерное пространство";
            this.d3ToolStripMenuItem.Click += new System.EventHandler(this.d3ToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.helpToolStripMenuItem.Text = "Справка";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.aboutToolStripMenuItem.Text = "О программе";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lb);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.wb);
            this.splitContainer1.Size = new System.Drawing.Size(594, 362);
            this.splitContainer1.SplitterDistance = 238;
            this.splitContainer1.TabIndex = 1;
            // 
            // lb
            // 
            this.lb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb.FormattingEnabled = true;
            this.lb.Items.AddRange(new object[] {
            "Наблюдения",
            "Выборочные характеристики",
            "Анализ выборочных характеристик",
            "Модель технологии",
            "Анализ модели технологии",
            "Корреляционный анализ",
            "Корреляционные матрицы для уравнений регрессии",
            "Анализ корреляционных матриц",
            "Модифицированные наблюдения",
            "Количество наблюдений",
            "Кластеры",
            "Реплики",
            "Энтропия распределения реплик",
            "Идентификация рамочной технологии"});
            this.lb.Location = new System.Drawing.Point(0, 0);
            this.lb.Name = "lb";
            this.lb.Size = new System.Drawing.Size(238, 355);
            this.lb.TabIndex = 0;
            this.lb.SelectedIndexChanged += new System.EventHandler(this.lb_SelectedIndexChanged);
            // 
            // wb
            // 
            this.wb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wb.Location = new System.Drawing.Point(0, 0);
            this.wb.MinimumSize = new System.Drawing.Size(20, 20);
            this.wb.Name = "wb";
            this.wb.Size = new System.Drawing.Size(352, 362);
            this.wb.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 386);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Анализ качества";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem genToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lb;
        private System.Windows.Forms.WebBrowser wb;
        private System.Windows.Forms.ToolStripMenuItem analizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem histToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem identToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hist3dToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem d3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem d2HistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem d2HistZoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem d3HistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem d3HistZoneToolStripMenuItem;
    }
}