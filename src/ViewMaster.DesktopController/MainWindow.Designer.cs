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
            sequenceGrid = new DataGridView();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            loadSequenceToolStripMenuItem = new ToolStripMenuItem();
            saveSequenceToolStripMenuItem = new ToolStripMenuItem();
            splitContainer1 = new SplitContainer();
            ((System.ComponentModel.ISupportInitialize)sequenceGrid).BeginInit();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sequenceGrid
            // 
            sequenceGrid.AllowUserToOrderColumns = true;
            sequenceGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            sequenceGrid.Dock = DockStyle.Fill;
            sequenceGrid.Location = new Point(0, 0);
            sequenceGrid.Margin = new Padding(0);
            sequenceGrid.Name = "sequenceGrid";
            sequenceGrid.RowHeadersWidth = 62;
            sequenceGrid.RowTemplate.Height = 33;
            sequenceGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            sequenceGrid.Size = new Size(750, 955);
            sequenceGrid.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1430, 33);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loadSequenceToolStripMenuItem, saveSequenceToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(54, 29);
            fileToolStripMenuItem.Text = "File";
            // 
            // loadSequenceToolStripMenuItem
            // 
            loadSequenceToolStripMenuItem.Name = "loadSequenceToolStripMenuItem";
            loadSequenceToolStripMenuItem.Size = new Size(234, 34);
            loadSequenceToolStripMenuItem.Text = "Load Sequence";
            loadSequenceToolStripMenuItem.Click += this.LoadSequenceToolStripMenuItem_Click;
            // 
            // saveSequenceToolStripMenuItem
            // 
            saveSequenceToolStripMenuItem.Name = "saveSequenceToolStripMenuItem";
            saveSequenceToolStripMenuItem.Size = new Size(234, 34);
            saveSequenceToolStripMenuItem.Text = "Save Sequence";
            saveSequenceToolStripMenuItem.Click += this.SaveSequenceToolStripMenuItem_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 33);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(sequenceGrid);
            splitContainer1.Size = new Size(1430, 955);
            splitContainer1.SplitterDistance = 750;
            splitContainer1.SplitterWidth = 10;
            splitContainer1.TabIndex = 2;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1430, 988);
            this.Controls.Add(splitContainer1);
            this.Controls.Add(menuStrip1);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.MainMenuStrip = menuStrip1;
            this.Name = "MainWindow";
            this.Text = "View Master";
            this.FormClosed += this.MainWindow_FormClosed;
            this.Load += this.MainWindow_Load;
            ((System.ComponentModel.ISupportInitialize)sequenceGrid).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private DataGridView sequenceGrid;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem loadSequenceToolStripMenuItem;
        private ToolStripMenuItem saveSequenceToolStripMenuItem;
        private SplitContainer splitContainer1;
    }
}