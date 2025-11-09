using Core.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Video_Editor;
using VideoEditor.Core;
using VideoEditor.Core.Operations;
using VideoEditor.Infrastructure;
using EditAction = VideoEditor.Core.SelectionManager.EditAction;

namespace VideoEditor.UI
{
    public partial class Main_Form : Form
    {
        public Project Project { get { return ProjectContext.CurrentProject; } }
        public event Action? MainOpened;

        public enum LaneInteractionMode { Select, Edit, Navigate }
        public LaneInteractionMode CurrentMode { get; set; } = LaneInteractionMode.Select;
        public EditAction PendingEdit => Project.SelectionManager.PendingEdit;
        public int SecondsSinceAutoSave { get; set; }
        public Main_Form()
        {
            InitializeComponent();

            RefreshFiles();

            Project.Configuration.LanePanelWidth = LanePanel.Width;
            Project.Configuration.LanePanelHeight = LanePanel.Height;
            UpdateScrolls();
            SecondsSinceAutoSave = 0;
            var autosaveTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000
            };
            autosaveTimer.Tick += AutoSaveTimer_Tick;
            autosaveTimer.Start();

            tableLayoutPanel24.AutoScroll = false;
            tableLayoutPanel24.HorizontalScroll.Enabled = false;
            tableLayoutPanel24.HorizontalScroll.Visible = false;
            tableLayoutPanel24.AutoScroll = true;
        }
        private void AutoSaveTimer_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel3.Text = $"Autosave: {TimeSpan.FromSeconds(SecondsSinceAutoSave).ToString()}";
            SecondsSinceAutoSave++;
            if (SecondsSinceAutoSave >= Config.AutoSaveIntervalMinutes * 60)
            {
                try
                {
                    ProjectStorage.Save(Project, Project.Path);

                    var RecentProjects = ConfigManager.LoadRecent();
                    RecentProjects.AddProject(Project);
                    ConfigManager.SaveRecent(RecentProjects);
                }
                catch (ArgumentException ex)
                {

                    MessageBox.Show($"AutoSave failed: {ex.Message}\n\n{ex.StackTrace}");
                    throw;

                }
                finally
                {
                    SecondsSinceAutoSave = 0;
                }
            }
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
            //else if ()
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
            UpdateScrolls();
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
            if (CurrentMode == LaneInteractionMode.Select)
            {
                Project.SelectionManager.SelectObject(e.X, e.Y);

                LanePanel.Update();
                LanePanel.Refresh();
                RefreshFragmentParametres();
            }
            if (CurrentMode == LaneInteractionMode.Edit)
            {
                Project.SelectionManager.SelectObject(e.X, e.Y);
                Project.SelectionManager.DragStartTime = Project.SelectionManager.SelectedTime;

                LanePanel.Update();
                LanePanel.Refresh();

                if (Project.SelectionManager.SelectedFragment != null)
                {
                    Project.SelectionManager.PendingEdit = EditAction.Move;
                }
                else
                {
                    Project.SelectionManager.ClearSelection();
                }
            }
            else
            {
                Project.SelectionManager.SelectObject(e.X, e.Y);
            }

            //Move Fragment

            //Trim Fragment

            //Navigate
        }

        private void LanePanel_MouseClick(object sender, MouseEventArgs e)
        {
            //currentProject.SelectionManager.SelectObject(e.Location.X, e.Location.Y);
            //currentProject.
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

                var RecentProjects = ConfigManager.LoadRecent();
                RecentProjects.AddProject(Project);
                ConfigManager.SaveRecent(RecentProjects);
            }
            catch (ArgumentException ex)
            {

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
            /*using (var dlg = new SaveFileDialog())
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
            }*/
            new ExportForm().Show();
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
            //Sound
            checkBox1.Checked = fragment.Muted;
            if (fragment.Muted)
            {
                trackBar1.Enabled= false;
            }
            else
            {
                trackBar1.Enabled = true;
                trackBar1.Value = (int)(fragment.Volume * 100);
            }
            //Visibility
            checkBox2.Checked = fragment.Hidden;
            trackBar1.Value = (int)(fragment.Volume * 100);
            if (fragment.Hidden)
            {
                trackBar2.Enabled = false;
            }
            else
            {
                trackBar2.Enabled = true;
                trackBar2.Maximum = 100;
                trackBar2.Value = (int)(fragment.Opacity * 100);
            }
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
            //Reset trim
            var FragmentPlacement = Project.SelectionManager.SelectedFragment;
            if (Project.SelectionManager.SelectedFragment != null)
            {
                var Trim = new TrimOperation(Project, FragmentPlacement, TimeSpan.Zero, FragmentPlacement.Fragment.FileDuration);
                Project.History.Execute(Trim);
                RefreshFragmentParametres();

                LanePanel.Update();
                LanePanel.Refresh();
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            //Trim start
            var fragmentPlacement = Project.SelectionManager.SelectedFragment;
            if (fragmentPlacement == null) return;

            if (TimeSpan.TryParse(textBox2.Text, out var ts))
            {
                if (ts >= fragmentPlacement.Fragment.EndTime)
                {
                    RefreshFragmentParametres();
                    MessageBox.Show("Start time should be less than end");
                }
                else
                {
                    var Trim = new TrimOperation(Project, fragmentPlacement, ts, fragmentPlacement.Fragment.EndTime);
                    Project.History.Execute(Trim);
                    RefreshFragmentParametres();

                    LanePanel.Update();
                    LanePanel.Refresh();
                }

            }
            else
            {
                MessageBox.Show("Please enter a valid time (e.g., 00:01:23.456)");
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            //Trim end
            var fragmentPlacement = Project.SelectionManager.SelectedFragment;
            if (fragmentPlacement == null) return;

            if (TimeSpan.TryParse(textBox3.Text, out var te))
            {
                if (te <= fragmentPlacement.Fragment.StartTime)
                {
                    MessageBox.Show("End time should be greater than start");
                }
                else
                {
                    var Trim = new TrimOperation(Project, fragmentPlacement, fragmentPlacement.Fragment.StartTime, te);
                    Project.History.Execute(Trim);
                    RefreshFragmentParametres();

                    LanePanel.Update();
                    LanePanel.Refresh();
                }
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
            UpdateScrolls();
        }

        private void LanePanel_MouseDown_1(object sender, MouseEventArgs e)
        {

        }

        private void hSc(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            //Move lane up
            if (Project.SelectionManager.SelectedLane != null)
            {
                var MoveUp = new MoveLaneUpOperation(Project, Project.SelectionManager.SelectedLane);
                Project.History.Execute(MoveUp);

                LanePanel.Update();
                LanePanel.Refresh();
            }


        }

        private void button11_Click(object sender, EventArgs e)
        {
            //Move lane down
            if (Project.SelectionManager.SelectedLane != null)
            {
                var MoveDown = new MoveLaneDownOperation(Project, Project.SelectionManager.SelectedLane);
                Project.History.Execute(MoveDown);

                LanePanel.Update();
                LanePanel.Refresh();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //Double lane
            if (Project.SelectionManager.SelectedLane != null)
            {
                var DoubleLane = new DoubleLaneOperation(Project, Project.SelectionManager.SelectedLane);
                Project.History.Execute(DoubleLane);

                LanePanel.Update();
                LanePanel.Refresh();
            }
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

        private void button6_Click(object sender, EventArgs e)
        {
            //Copy
            if (Project.SelectionManager.SelectedFragment != null)
            {
                var Copy = new CopyOperation(Project, Project.SelectionManager.SelectedFragment);
                Project.History.Execute(Copy);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Cut
            if (Project.SelectionManager.SelectedFragment != null && Project.SelectionManager.SelectedLane != null)
            {
                var Cut = new CutOperation(Project, Project.SelectionManager.SelectedLane, Project.SelectionManager.SelectedFragment);
                Project.History.Execute(Cut);

                LanePanel.Update();
                LanePanel.Refresh();
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            //Paste
            if (Project.SelectionManager.MemoryFragment != null && Project.SelectionManager.SelectedLane != null && Project.SelectionManager.SelectedTime != null)
            {
                var Paste = new PasteOperation(Project.SelectionManager.MemoryFragment, Project.SelectionManager.SelectedLane, (TimeSpan)Project.SelectionManager.SelectedTime);
                Project.History.Execute(Paste);

                LanePanel.Update();
                LanePanel.Refresh();
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            //Delete
            if (Project.SelectionManager.SelectedFragment != null && Project.SelectionManager.SelectedLane != null)
            {
                var Delete = new DeleteFragmentOperation(Project.SelectionManager.SelectedFragment, Project.SelectionManager.SelectedLane);
                Project.History.Execute(Delete);

                LanePanel.Update();
                LanePanel.Refresh();
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            //Split
            if (Project.SelectionManager.SelectedFragment != null && Project.SelectionManager.SelectedTime != null && Project.SelectionManager.SelectedLane != null)
            {
                var SelectedTime = Project.SelectionManager.SelectedTime;
                var SelectedFragment = Project.SelectionManager.SelectedFragment;
                if (SelectedTime > SelectedFragment.Position && SelectedTime < SelectedFragment.EndPosition)
                {
                    var CutOffset = Project.SelectionManager.SelectedTime - SelectedFragment.Position + SelectedFragment.Fragment.StartTime;

                    var Split = new SplitOperation(Project, Project.SelectionManager.SelectedLane, SelectedFragment, (TimeSpan)CutOffset);
                    Project.History.Execute(Split);

                    LanePanel.Update();
                    LanePanel.Refresh();
                }
            }
        }



        private void LanePanel_DragOver(object sender, DragEventArgs e)
        {

        }

        private void LanePanel_MouseUp(object sender, MouseEventArgs e)
        {
            //Drop fragment to new place
            if (CurrentMode == LaneInteractionMode.Edit)
            {
                if (PendingEdit == EditAction.Move && Project.SelectionManager.SelectedFragment != null && Project.SelectionManager.SelectedTime != null)
                {
                    var GrabbedTime = Project.SelectionManager.DragStartTime;
                    var MovedFragment = Project.SelectionManager.SelectedFragment;
                    var MovedFragmentLane = Project.SelectionManager.SelectedLane;

                    Project.SelectionManager.SelectObject(e.X, e.Y);
                    if (Project.SelectionManager.SelectedLane != null)
                    {

                        var Move = new MoveFragmentOperation(MovedFragmentLane, Project.SelectionManager.SelectedLane, (TimeSpan)GrabbedTime, (TimeSpan)Project.SelectionManager.SelectedTime);
                        Project.History.Execute(Move);


                        Project.SelectionManager.PendingEdit = EditAction.None;

                        LanePanel.Update();
                        LanePanel.Refresh();
                    }
                }
            }
            else if (CurrentMode == LaneInteractionMode.Navigate)
            {
                var FormerTime = Project.SelectionManager.SelectedTime;
                Project.SelectionManager.SelectObject(e.X, e.Y);
                var RecentTime = Project.SelectionManager.SelectedTime;
                if (FormerTime != null && RecentTime != null)
                {
                    Project.Configuration.LanePanelScrollX += ((TimeSpan)(FormerTime - RecentTime)).TotalSeconds * Project.Configuration.LaneTimeScale;
                    if (Project.Configuration.LanePanelScrollX < 0)
                    {
                        Project.Configuration.LanePanelScrollX = 0;
                    }

                    LanePanel.Update();
                    LanePanel.Refresh();
                }
            }
        }

        private void tabControl5_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Set new mode
            switch (tabControl5.SelectedIndex)
            {
                case 0:
                    CurrentMode = LaneInteractionMode.Select;
                    break;
                case 1:
                    CurrentMode = LaneInteractionMode.Edit;
                    break;
                case 2:
                    CurrentMode = LaneInteractionMode.Navigate;
                    break;
                default:
                    CurrentMode = LaneInteractionMode.Select;
                    break;
            }
        }

        private void LanePanel_MouseMove(object sender, MouseEventArgs e)
        {
            switch (CurrentMode)
            {
                case LaneInteractionMode.Select:
                    LanePanel.Cursor = Cursors.Default;
                    break;
                case LaneInteractionMode.Navigate:
                    LanePanel.Cursor = Cursors.Hand;
                    break;
                case LaneInteractionMode.Edit:
                    LanePanel.Cursor = Cursors.SizeAll;
                    Project.SelectionManager.SelectTime(e.X, e.Y);

                    //Lane???
                    var lane = Project.SelectionManager.SelectedLane;
                    var time = Project.SelectionManager.SelectedTime;
                    if (lane != null && time != null)
                    {
                        var selectLeft = lane[(TimeSpan)time - TimeSpan.FromSeconds(5 / Project.Configuration.LaneTimeScale)];
                        var selectRight = lane[(TimeSpan)time + TimeSpan.FromSeconds(5 / Project.Configuration.LaneTimeScale)];
                        if (selectLeft != null || selectRight != null)
                        {
                            double LeftEndDistance = 10;
                            double RightStartDistance = 10;
                            if (selectLeft != null)
                            {
                                LeftEndDistance = Math.Abs((selectLeft.EndPosition.TotalSeconds - ((TimeSpan)time).TotalSeconds) * Project.Configuration.LaneTimeScale);

                            }

                            if (selectRight != null)
                            {
                                RightStartDistance = Math.Abs((selectRight.Position.TotalSeconds - ((TimeSpan)time).TotalSeconds) * Project.Configuration.LaneTimeScale);
                            }

                            if (LeftEndDistance <= 5 || RightStartDistance <= 5)
                            {
                                LanePanel.Cursor = Cursors.SizeWE;
                            }
                            //Proximity


                        }

                    }
                    LanePanel.Update();
                    LanePanel.Refresh();
                    break;
                default:
                    Cursor = Cursors.Default;
                    break;
            }
        }

        private async void button41_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            axWindowsMediaPlayer1.URL = ""; // release the file
            await Task.Delay(100); // short pause to ensure file is unlocked

            if (Project.SelectionManager.SelectedTime != null)
            {
                await Project.engine.RenderPreviewAsync("preview.mp4", ((TimeSpan)Project.SelectionManager.SelectedTime).TotalSeconds, 5.0);
                axWindowsMediaPlayer1.URL = Path.GetFullPath("preview.mp4");
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            //MessageBox.Show(GetAudioFormat(Config.FfprobePath, Project.SelectionManager.SelectedFragment.Fragment.FilePath));
        }


        private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index != -1 && (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back))
            {
                Project.MediaFiles.RemoveAt(index);
                RefreshFiles();
            }
        }
        string GetAudioFormat(string ffprobePath, string filePath)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = ffprobePath,
                Arguments = $"-v error -select_streams a:0 -show_entries stream=sample_rate,channels,channel_layout -of default=nw=1 \"{filePath}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using var proc = Process.Start(startInfo);
            string output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            return output; // example: "sample_rate=48000\nchannels=2\nchannel_layout=stereo\n"
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //Mute
            if ((Project.SelectionManager.SelectedFragment != null))
            {
                Project.SelectionManager.SelectedFragment.Fragment.Muted = checkBox1.Checked;
                if (checkBox1.Checked)
                {
                    trackBar1.Enabled = false;
                }
                else
                {
                    trackBar1.Enabled = true;
                    trackBar1.Value = (int)(Project.SelectionManager.SelectedFragment.Fragment.Volume * 100);
                }
            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //Hide
            if ((Project.SelectionManager.SelectedFragment != null))
            {
                Project.SelectionManager.SelectedFragment.Fragment.Hidden = checkBox2.Checked;
                if (checkBox2.Checked)
                {
                    trackBar2.Enabled = false;
                }
                else
                {
                    trackBar2.Enabled = true;
                    trackBar2.Value = (int)(Project.SelectionManager.SelectedFragment.Fragment.Opacity * 100);
                }
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            //Sound
            if ((Project.SelectionManager.SelectedFragment != null))
            {
                Project.SelectionManager.SelectedFragment.Fragment.Volume = ((float)trackBar1.Value)/100;
            }
        }

        private void trackBar4_ValueChanged(object sender, EventArgs e)
        {
            //Opacity
            if ((Project.SelectionManager.SelectedFragment != null))
            {
                Project.SelectionManager.SelectedFragment.Fragment.Opacity = ((float)trackBar2.Value) / 100;
            }
        }
    }
}
