using Microsoft.VisualBasic.Devices;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdvancedEdit.Components
{
    public partial class PanPanel : Panel
    {
        private Point clickPosition;
        private Point scrollPosition;
        private Point lastPosition;

        private bool panDown;
        private bool inPanel;

        public PanPanel()
        {
            InitializeComponent();
        }

        public PanPanel(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                foreach (Control control in Controls)
                {
                    control.Size *= 2;
                    control.Invalidate();
                }
            }
            else
            {
                foreach (Control control in Controls)
                {
                    control.Size /= 2;
                    control.Invalidate();
                }
            }
            
        }
        public void PanPanel_ControlAdded(object? sender, ControlEventArgs e)
        {
            e.Control.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanPanel_MouseMove);
            e.Control.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanPanel_MouseDown);
            e.Control.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanPanel_MouseUp);
        }
        public void PanPanel_MouseDown(object sender, MouseEventArgs mouse)
        {
            if (mouse.Button == MouseButtons.Middle) panDown = true;
            clickPosition.X = Control.MousePosition.X;
            clickPosition.Y = Control.MousePosition.Y;
        }
        public void PanPanel_MouseUp(object sender, MouseEventArgs mouse)
        {
            Cursor = Cursors.Default;
            lastPosition.X = AutoScrollPosition.X;
            lastPosition.Y = AutoScrollPosition.Y;
            panDown = false;
        }
        public void PanPanel_MouseMove(object sender, MouseEventArgs mouse)
        {
            if (mouse.Button == MouseButtons.Middle)
            {
                Cursor = Cursors.Hand;
                scrollPosition.X = clickPosition.X - Control.MousePosition.X - lastPosition.X;
                scrollPosition.Y = clickPosition.Y - Control.MousePosition.Y - lastPosition.Y;
                AutoScrollPosition = scrollPosition;
            }
        }
        public void PanPanel_MouseLeave(object sender, EventArgs e)
        {
            inPanel = false;
        }
        public void PanPanel_MouseEnter(object sender, EventArgs e)
        {
            inPanel = true;
        }
    }
}
