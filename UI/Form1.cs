
using Video_Editor;
using VideoEditor.Core;
using VideoEditor.Infrastructure;
namespace VideoEditor.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            RefreshRecent();
        }
        public void RefreshRecent()
        {
            listBox1.Items.Clear();
            var recentList = ConfigManager.LoadRecent();
            if (recentList != null)
            {
                foreach (var recent in recentList.Recent)
                {
                    listBox1.Items.Add($"{recent.Name} - {recent.LastSaved.ToString()}");
                }
            }
        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var createForm = new CreateProject();
            createForm.ProjectCreated += OpenMain;
            createForm.Show();

        }

        private void OpenMain()
        {
            if (ProjectContext.CurrentProject != null)
            {
                // hide start form so the user doesn’t see it
                this.Hide();

                var mainForm = new Main_Form();

                // when main form closes, close hidden one too
                mainForm.FormClosed += (s, e) =>
                {
                    this.Close(); // terminates the message loop properly
                };

                // if MainForm fails to show, StartForm will still be visible
                try
                {
                    mainForm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening project: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Show(); // show again if something went wrong
                }

            }
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void OpenProjButton_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Video Editor Project files (*.vep)|*.vep";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ProjectContext.Open(ProjectStorage.Load(dlg.FileName));

                    // Open the main editor
                    OpenMain();

                }
            }

        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int index = listBox1.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                var RecentProjects = ConfigManager.LoadRecent();
                var RecentProject = RecentProjects.Recent[index];
                string path = Path.Join(RecentProject.Path,$"{RecentProject.Name}.vep");
                try
                {
                    ProjectContext.Open(ProjectStorage.Load(path));

                    // Open the main editor
                    OpenMain();
                }
                catch 
                {
                    RecentProjects.Recent.Remove(RecentProject);
                    ConfigManager.SaveRecent(RecentProjects);
                    MessageBox.Show("Project not found");
                    RefreshRecent();
                }
            }
        }
    }
}
