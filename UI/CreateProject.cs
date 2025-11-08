using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Videoeditor.Core;
using VideoEditor.Core;
using VideoEditor.Infrastructure;
namespace Video_Editor
{
    public partial class CreateProject : Form
    {
        public event Action? ProjectCreated;
        public CreateProject()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Choose where to create the new project";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = dialog.SelectedPath;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //TODO: wrap errors
            ProjectContext.Open(ProjectStorage.Create(textBox2.Text, textBox1.Text));
            var RecentProjects = ConfigManager.LoadRecent();
            RecentProjects.AddProject(ProjectContext.CurrentProject);
            ConfigManager.SaveRecent(RecentProjects);

            ProjectCreated.Invoke();
            this.Close();
        }
    }
}
