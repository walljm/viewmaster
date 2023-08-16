﻿namespace ViewMaster.DesktopController
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            grdCues = new DataGridView();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            loadSequenceToolStripMenuItem = new ToolStripMenuItem();
            saveSequenceToolStripMenuItem = new ToolStripMenuItem();
            splitContainer1 = new SplitContainer();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            lblOperations = new Label();
            ctlOperations = new ListView();
            ctlLabel = new TextBox();
            lblLabel = new Label();
            ctlOrdinal = new NumericUpDown();
            lblOrdinal = new Label();
            tabPage2 = new TabPage();
            grdCameras = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)grdCues).BeginInit();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ctlOrdinal).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grdCameras).BeginInit();
            this.SuspendLayout();
            // 
            // grdCues
            // 
            grdCues.AllowUserToOrderColumns = true;
            grdCues.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grdCues.Dock = DockStyle.Fill;
            grdCues.Location = new Point(0, 0);
            grdCues.Margin = new Padding(0);
            grdCues.Name = "grdCues";
            grdCues.RowHeadersWidth = 62;
            grdCues.RowTemplate.Height = 45;
            grdCues.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdCues.Size = new Size(969, 1050);
            grdCues.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(10, 3, 0, 3);
            menuStrip1.Size = new Size(1832, 56);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loadSequenceToolStripMenuItem, saveSequenceToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(95, 50);
            fileToolStripMenuItem.Text = "&File";
            // 
            // loadSequenceToolStripMenuItem
            // 
            loadSequenceToolStripMenuItem.Image = Properties.Resources.icons8_open_128;
            loadSequenceToolStripMenuItem.Name = "loadSequenceToolStripMenuItem";
            loadSequenceToolStripMenuItem.Size = new Size(416, 54);
            loadSequenceToolStripMenuItem.Text = "&Load Sequence";
            loadSequenceToolStripMenuItem.Click += this.LoadSequenceToolStripMenuItem_Click;
            // 
            // saveSequenceToolStripMenuItem
            // 
            saveSequenceToolStripMenuItem.Image = Properties.Resources.icons8_save_90;
            saveSequenceToolStripMenuItem.Name = "saveSequenceToolStripMenuItem";
            saveSequenceToolStripMenuItem.Size = new Size(416, 54);
            saveSequenceToolStripMenuItem.Text = "&Save Sequence";
            saveSequenceToolStripMenuItem.Click += this.SaveSequenceToolStripMenuItem_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.Location = new Point(0, 56);
            splitContainer1.Margin = new Padding(5);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(grdCues);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tabControl1);
            splitContainer1.Size = new Size(1832, 1050);
            splitContainer1.SplitterDistance = 969;
            splitContainer1.SplitterWidth = 17;
            splitContainer1.TabIndex = 2;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(5);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(846, 1050);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(lblOperations);
            tabPage1.Controls.Add(ctlOperations);
            tabPage1.Controls.Add(ctlLabel);
            tabPage1.Controls.Add(lblLabel);
            tabPage1.Controls.Add(ctlOrdinal);
            tabPage1.Controls.Add(lblOrdinal);
            tabPage1.Location = new Point(10, 58);
            tabPage1.Margin = new Padding(5);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(5);
            tabPage1.Size = new Size(826, 982);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Cue Details";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblOperations
            // 
            lblOperations.AutoSize = true;
            lblOperations.Location = new Point(18, 154);
            lblOperations.Name = "lblOperations";
            lblOperations.Size = new Size(165, 41);
            lblOperations.TabIndex = 6;
            lblOperations.Text = "Operations";
            // 
            // ctlOperations
            // 
            ctlOperations.Location = new Point(17, 222);
            ctlOperations.Name = "ctlOperations";
            ctlOperations.Size = new Size(796, 276);
            ctlOperations.TabIndex = 4;
            ctlOperations.UseCompatibleStateImageBehavior = false;
            // 
            // ctlLabel
            // 
            ctlLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ctlLabel.Location = new Point(120, 90);
            ctlLabel.Name = "ctlLabel";
            ctlLabel.Size = new Size(694, 47);
            ctlLabel.TabIndex = 3;
            // 
            // lblLabel
            // 
            lblLabel.AutoSize = true;
            lblLabel.Location = new Point(18, 91);
            lblLabel.Name = "lblLabel";
            lblLabel.Size = new Size(95, 41);
            lblLabel.TabIndex = 2;
            lblLabel.Text = "Label:";
            // 
            // ctlOrdinal
            // 
            ctlOrdinal.Location = new Point(274, 21);
            ctlOrdinal.Name = "ctlOrdinal";
            ctlOrdinal.Size = new Size(99, 47);
            ctlOrdinal.TabIndex = 1;
            // 
            // lblOrdinal
            // 
            lblOrdinal.AutoSize = true;
            lblOrdinal.Location = new Point(17, 21);
            lblOrdinal.Name = "lblOrdinal";
            lblOrdinal.Size = new Size(254, 41);
            lblOrdinal.TabIndex = 0;
            lblOrdinal.Text = "Ordinal (Position):";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(grdCameras);
            tabPage2.Location = new Point(10, 58);
            tabPage2.Margin = new Padding(5);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(5);
            tabPage2.Size = new Size(826, 982);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Cameras";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // grdCameras
            // 
            grdCameras.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grdCameras.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grdCameras.Dock = DockStyle.Fill;
            grdCameras.Location = new Point(5, 5);
            grdCameras.Margin = new Padding(5);
            grdCameras.Name = "grdCameras";
            grdCameras.RowHeadersWidth = 62;
            grdCameras.RowTemplate.Height = 45;
            grdCameras.Size = new Size(816, 972);
            grdCameras.TabIndex = 0;
            grdCameras.CellDoubleClick += this.GrdCameras_CellDoubleClick;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new SizeF(17F, 41F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1832, 1106);
            this.Controls.Add(splitContainer1);
            this.Controls.Add(menuStrip1);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.MainMenuStrip = menuStrip1;
            this.Margin = new Padding(5);
            this.Name = "MainWindow";
            this.Text = "View Master";
            this.Load += this.MainWindow_Load;
            ((System.ComponentModel.ISupportInitialize)grdCues).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ctlOrdinal).EndInit();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grdCameras).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private DataGridView grdCues;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem loadSequenceToolStripMenuItem;
        private ToolStripMenuItem saveSequenceToolStripMenuItem;
        private SplitContainer splitContainer1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private DataGridView grdCameras;
        private TextBox ctlLabel;
        private Label lblLabel;
        private NumericUpDown ctlOrdinal;
        private Label lblOrdinal;
        private Label lblOperations;
        private ListView ctlOperations;
    }
}