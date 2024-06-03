using AdvancedEdit.Gfx;
using AdvancedLib;
using System.ComponentModel;

namespace AdvancedEdit
{
    public partial class Form1 : Form
    {
        Manager manager;
        TrackDrawer trackDrawer;

        public static bool loaded = false;
        public Form1()
        {
            InitializeComponent();
            manager = new Manager();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                manager.Open(openFileDialog.FileName);
                deserializeWorker.RunWorkerAsync();
            }
        }

        private void deserializeWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            manager.Deserialize();
        }

        private void deserializeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            trackDrawer = new TrackDrawer(gridBox1);
            trackDrawer.LoadTrack(manager.trackManager.tracks[0]);

            gridBox1.Enabled = true;
            loaded = true;
        }

        private void gridBox1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded) return;
            trackDrawer.DrawTrack(e.Graphics,0);
        }
    }
}
