using Core.Operations;
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
            //Select lane or Fragment
            Project.SelectionManager.SelectObject(e.X, e.Y);

            LanePanel.Update();
            LanePanel.Refresh();
            RefreshFragmentParametres();
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
            try
            {
                ProjectStorage.Save(Project, Project.Path);
            }
            catch(ArgumentException ex) {

                MessageBox.Show($"Save failed: {ex.Message}\n\n{ex.StackTrace}");
                throw;

            }
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
                    Point relativePoint = LanePanelXY(e.X, e.Y);
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
            using (var dlg = new SaveFileDialog())
            {
                if (dlg != null)
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show($"Started rendering {dlg.FileName}");
                        await Project.engine.RenderAsync(dlg.FileName);
                        MessageBox.Show($"Saved as {dlg.FileName}");
                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            IOperation AddLane = new AddNewLaneOperation(Project);
            Project.History.Execute(AddLane);

            LanePanel.Update();
            LanePanel.Refresh();
        }
        private Point LanePanelXY(int x, int y)
        {
            return LanePanel.PointToClient(new Point(x, y));
        }
        private void RefreshFragmentParametres()
        {
            var fragmentPlacement = Project.SelectionManager.SelectedFragment;
            if (fragmentPlacement == null) return;

            var fragment = fragmentPlacement.Fragment;
            //Name
            textBox1.Text = fragment.Name;
            //Duration
            label69.Text = fragment.Duration.ToString();
            //Start on line
            label68.Text = fragmentPlacement.Position.ToString();
            //End on line
            label9.Text = fragmentPlacement.EndPosition.ToString();
            //Trim start
            textBox2.Text = fragment.StartTime.ToString();
            //Trim end
            textBox3.Text = fragment.EndTime.ToString();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var fragmentPlacement = Project.SelectionManager.SelectedFragment;
            if (fragmentPlacement == null) return;

            fragmentPlacement.Fragment.Name = textBox1.Text;
            LanePanel.Update();
            LanePanel.Refresh();
        }

        private void Main_Form_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Delete
        }

        private void Main_Form_KeyDown(object sender, KeyEventArgs e)
        {
            //ctrl+c

            //ctrl+v

            //ctrl+x

            //ctrl+z

            //delete or backspace
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (Project.SelectionManager.SelectedLane != null)
            {
                IOperation deleteLane = new DeleteLaneOperation(Project, Project.SelectionManager.SelectedLane);
                Project.History.Execute(deleteLane);

                LanePanel.Update();
                LanePanel.Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //
            ZoomInTime();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ZoomOutTime();
        }

        public void ZoomOutTime()
        {
            if (Project.Configuration != null)
            {
                Project.Configuration.LaneTimeScale /= Config.TimeZoomRatio;

                LanePanel.Update();
                LanePanel.Refresh();
            }
        }
        public void ZoomInTime()
        {
            if (Project.Configuration != null)
            {
                Project.Configuration.LaneTimeScale *= Config.TimeZoomRatio;

                LanePanel.Update();
                LanePanel.Refresh();
            }
        }
        public void ZoomOutLane()
        {
            if (Project.Configuration != null)
            {
                Project.Configuration.LaneHeight = (int)(Project.Configuration.LaneHeight / Config.TimeZoomRatio);
                Project.Configuration.LaneSpacing = (int)(Project.Configuration.LaneSpacing / Config.TimeZoomRatio);

                LanePanel.Update();
                LanePanel.Refresh();
            }
        }
        public void ZoomInLane()
        {
            if (Project.Configuration != null)
            {
                Project.Configuration.LaneHeight = (int)(Project.Configuration.LaneHeight * Config.TimeZoomRatio);
                Project.Configuration.LaneSpacing = (int)(Project.Configuration.LaneSpacing * Config.TimeZoomRatio);

                LanePanel.Update();
                LanePanel.Refresh();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Fit timeline to screen for overview
            if (Project.Configuration != null)
            {
                if (Project.ProjectDuration > TimeSpan.Zero)
                {
                    Project.Configuration.LaneTimeScale =
                        (Project.Configuration.LanePanelWidth - Project.Configuration.LaneLabelWidth)
                        / (float)Project.ProjectDuration.TotalSeconds;
                }
                Project.Configuration.LanePanelScrollX = Project.ProjectStart.TotalSeconds * Project.Configuration.LaneTimeScale;

                LanePanel.Update();
                LanePanel.Refresh();
            }
        }

        private void LanePanel_SizeChanged(object sender, EventArgs e)
        {
            if (Project.Configuration != null)
            {
                Project.Configuration.LanePanelWidth = LanePanel.Width;
                Project.Configuration.LanePanelHeight = LanePanel.Height;
                UpdateScrolls();

                LanePanel.Update();
                LanePanel.Refresh();
            }
        }
        private void UpdateScrolls()
        {
            int TimeDifference = (int)(Project.ProjectEnd.TotalSeconds * Project.Configuration.LaneTimeScale) - Project.Configuration.LanePanelWidth + Project.Configuration.LaneLabelWidth;
            int HeightDifference = Project.Lanes.Count * (Project.Configuration.LaneHeight + Project.Configuration.LaneSpacing) - Project.Configuration.LanePanelHeight;
            //Time scroll
            hScrollBar1.Minimum = 0;
            hScrollBar1.Maximum = (TimeDifference > 0 ? TimeDifference : 0);
            var ScrollX = (int)Project.Configuration.LanePanelScrollX;
            hScrollBar1.Value = ScrollX > hScrollBar1.Maximum ? hScrollBar1.Maximum : ScrollX;
            //Lane scroll

            vScrollBar1.Minimum = 0;
            vScrollBar1.Maximum = (HeightDifference > 0 ? HeightDifference : 0);
            var ScrollY = (int)Project.Configuration.LanePanelScrollY;
            vScrollBar1.Value = ScrollY > vScrollBar1.Maximum ? vScrollBar1.Maximum : ScrollY;
        }

        private void button32_Click(object sender, EventArgs e)
        {
            //Increase lane height
            ZoomInLane();
            UpdateScrolls();
        }

        private void button35_Click(object sender, EventArgs e)
        {
            //Decrease lane height
            ZoomOutLane();
            UpdateScrolls();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            Project.Configuration.LanePanelScrollX = e.NewValue;

            LanePanel.Update();
            LanePanel.Refresh();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            Project.Configuration.LanePanelScrollY = e.NewValue;

            LanePanel.Update();
            LanePanel.Refresh();
        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            var fragmentPlacement = Project.SelectionManager.SelectedFragment;
            if (fragmentPlacement == null) return;

            if (TimeSpan.TryParse(textBox2.Text, out var ts))
            {
                fragmentPlacement.Fragment.StartTime = ts;

                LanePanel.Update();
                LanePanel.Refresh();
            }
            else
            {
                MessageBox.Show("Please enter a valid time (e.g., 00:01:23.456)");
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            var fragmentPlacement = Project.SelectionManager.SelectedFragment;
            if (fragmentPlacement == null) return;

            if (TimeSpan.TryParse(textBox3.Text, out var ts))
            {
                fragmentPlacement.Fragment.EndTime = ts;

                LanePanel.Update();
                LanePanel.Refresh();
            }
            else
            {
                MessageBox.Show("Please enter a valid time (e.g., 00:01:23.456)");
            }
        }

        private void button40_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //Zoom in time
            ZoomInTime();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //Zoom out time
            ZoomOutTime();
        }

        private void LanePanel_Click(object sender, EventArgs e)
        {

        }

        private void LanePanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Project.Graphics = g;
            Project.RenderPanel();
        }

        private void LanePanel_MouseDown_1(object sender, MouseEventArgs e)
        {
            Project.SelectionManager.SelectObject(e.X, e.Y);

            LanePanel.Update();
            LanePanel.Refresh();
            RefreshFragmentParametres();
        }

        private void hSc(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox2_Leave(sender, e);
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox3_Leave(sender, e);
            }
        }
    }
}
