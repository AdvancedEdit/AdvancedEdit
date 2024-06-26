﻿using AdvancedEdit.Components;
namespace AdvancedEdit
{
    partial class Form1
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
            components = new System.ComponentModel.Container();
            menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            deserializeWorker = new System.ComponentModel.BackgroundWorker();
            splitContainer1 = new SplitContainer();
            panPanel1 = new PanPanel(components);
            gridBox1 = new GridBox(components);
            menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.SuspendLayout();
            panPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridBox1).BeginInit();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(877, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(180, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(180, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(180, 22);
            saveAsToolStripMenuItem.Text = "Save As";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog";
            // 
            // deserializeWorker
            // 
            deserializeWorker.DoWork += deserializeWorker_DoWork;
            deserializeWorker.RunWorkerCompleted += deserializeWorker_RunWorkerCompleted;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 24);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.AutoScroll = true;
            splitContainer1.Panel1.Controls.Add(panPanel1);
            splitContainer1.Size = new Size(877, 568);
            splitContainer1.SplitterDistance = 663;
            splitContainer1.TabIndex = 1;
            // 
            // panPanel1
            // 
            panPanel1.AutoScroll = true;
            panPanel1.Controls.Add(gridBox1);
            panPanel1.Dock = DockStyle.Fill;
            panPanel1.Location = new Point(0, 0);
            panPanel1.Name = "panPanel1";
            panPanel1.Size = new Size(663, 568);
            panPanel1.TabIndex = 0;
            // 
            // gridBox1
            // 
            gridBox1.AllowMultiSelection = false;
            gridBox1.BoxSize = new Size(2, 2);
            gridBox1.CanvasSize = new Size(338, 326);
            gridBox1.Enabled = false;
            gridBox1.HoverBox = true;
            gridBox1.HoverColor = Color.White;
            gridBox1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            gridBox1.Location = new Point(0, 0);
            gridBox1.Name = "gridBox1";
            gridBox1.Selectable = false;
            gridBox1.SelectedIndex = -1;
            gridBox1.SelectionColor = Color.Gold;
            gridBox1.SelectionRectangle = new Rectangle(0, 0, 1, 1);
            gridBox1.Size = new Size(338, 326);
            gridBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            gridBox1.TabIndex = 0;
            gridBox1.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(877, 592);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip);
            HelpButton = true;
            MainMenuStrip = menuStrip;
            Name = "Form1";
            Text = "Form1";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private System.ComponentModel.BackgroundWorker deserializeWorker;
        private SplitContainer splitContainer1;
        private GridBox gridBox1;
        private PanPanel panPanel1;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
    }
}
