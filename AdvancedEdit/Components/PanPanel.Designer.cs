namespace AdvancedEdit.Components
{
    partial class PanPanel
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // GridBox
            //
            this.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.PanPanel_ControlAdded);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanPanel_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanPanel_MouseUp);
            this.MouseLeave += new System.EventHandler(this.PanPanel_MouseLeave);
            this.MouseEnter += new System.EventHandler(this.PanPanel_MouseEnter);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanPanel_MouseMove);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
