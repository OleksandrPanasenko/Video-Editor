using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoEditor.Core;

namespace VideoEditor.UI
{
    public partial class Main_Form : Form
    {
        public Project currentProject;
        public static Main_Form Instance;
        public Main_Form()
        {
            //InitializeComponent();
            Instance = this;
            currentProject = new Project();

        }
        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel27_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LanePanel_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void LanePanel_MouseClick(object sender, MouseEventArgs e)
        {
            currentProject.SelectionManager.SelectObject(e.Location.X, e.Location.Y);
            currentProject.
        }
    }
}
