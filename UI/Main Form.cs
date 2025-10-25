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
using VideoEditor.Infrastructure;

namespace VideoEditor.UI
{
    public partial class Main_Form : Form
    {
        public Project Project { get { return ProjectContext.CurrentProject; } }
        public event Action? MainOpened;
        public Main_Form()
        {
            InitializeComponent();
         
            RefreshFiles();

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
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (fileList != null & fileList.Length > 0)
            {
                IOperation addFiles = new AddMultipleFilesOperation(Project, fileList);
                Project.History.Execute(addFiles);
                RefreshFiles();
                //MessageBox.Show("You dropped some files");
                //Test fragment addition

                if (!File.Exists(fileList[0]))
                {
                    MessageBox.Show($"File not found {fileList[0]}");
                    return;
                }
                IOperation Test = new AddNewFragmentFromFileOperation(Project, fileList[0], Project.Lanes[0]);
                Project.History.Execute(Test);

                LanePanel.Invalidate();
                LanePanel.Update();
                LanePanel.Refresh();
                
            }
            //MessageBox.Show("You drag and dropped");
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

            Graphics g = e.Graphics;
            Project.Graphics = g;
            Project.RenderPanel();
            /*using(Brush brush=new SolidBrush(Color.CornflowerBlue))
            {
                g.FillRectangle(brush, 0, 0, 100, 48);
            }
            using(Pen pen=new Pen(Color.Yellow))
            {
                g.DrawRectangle(pen, 0, 0, 100, 16);
            }
            g.DrawString("1h", new Font("Segoe UI", 9), Brushes.Red, new PointF(0, 0));*/

        }

        private void LanePanel_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void LanePanel_MouseClick(object sender, MouseEventArgs e)
        {
            //currentProject.SelectionManager.SelectObject(e.Location.X, e.Location.Y);
            //currentProject.
        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void Main_Form_Shown(object sender, EventArgs e)
        {

        }

        private void Main_Form_Load(object sender, EventArgs e)
        {
            MainOpened?.Invoke();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            ProjectStorage.Save(Project, Project.Path);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void RefreshFiles()
        {
            listBox1.Items.Clear();
            foreach (string filePath in Project.MediaFiles)
            {
                listBox1.Items.Add(System.IO.Path.GetFileName(filePath));
            }
        }

        private void Main_Form_DragDrop(object sender, DragEventArgs e)
        {
            
        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy; // allows drop
            else
                e.Effect = DragDropEffects.None; // not allowed
        }
    }
}
