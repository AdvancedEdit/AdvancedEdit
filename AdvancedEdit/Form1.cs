using AdvancedEdit.Gfx;
using AdvancedLib;
using System.ComponentModel;
using AdvancedEdit.Components;

namespace AdvancedEdit
{
    public partial class Form1 : Form
    {
        Manager manager;
        Gfx.TrackEditor trackEditor;

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
            trackEditor = new TrackEditor(gridBox1);
            trackEditor.LoadTrack(manager.trackManager.tracks[24]);

            gridBox1.Enabled = true;
            loaded = true;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.Save(null);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                manager.Save(saveFileDialog.FileName);
            }
        }
    }
}
