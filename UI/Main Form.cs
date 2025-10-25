using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

        private async void listBox1_DragDrop(object sender, DragEventArgs e)
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

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int index = listBox1.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                listBox1.DoDragDrop(index, DragDropEffects.Copy);
            }
        }

        private void LanePanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(int)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private async void LanePanel_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null & e.Data.GetData(typeof(int)) != null)
            {
                int index = (int)e.Data.GetData(typeof(int));
                if (index >= 0 & index < Project.MediaFiles.Count())
                {
                    Point relativePoint = LanePanel.PointToClient(new Point(e.X, e.Y));
                    Project.SelectionManager.SelectObject(relativePoint);
                    if (Project.SelectionManager.SelectedLane != null)
                    {
                        var operation = await AddNewFragmentFromFileOperation.CreateAsync(Project, Project.MediaFiles[index], Project.SelectionManager.SelectedLane);

                        Project.History.Execute(operation);


                        //LanePanel.Invalidate();
                        LanePanel.Update();
                        LanePanel.Refresh();
                    }
                    else
                    {
                        MessageBox.Show($"Lane not selected, point[{relativePoint.X},{relativePoint.Y}]");
                    }
                }
            }
        }

        private async void button13_Click(object sender, EventArgs e)
        {
            using (var dlg=new SaveFileDialog())
            {
                if (dlg != null) {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        await Project.engine.RenderAsync(dlg.FileName);
                    }
                }
            }
        }
    }
}
