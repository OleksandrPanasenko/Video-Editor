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
using VideoEditor.Core.Operations;

namespace Video_Editor
{
    public partial class ExportForm : Form
    {
        public ExportForm()
        {
            InitializeComponent();
            comboBox1.Text = "1920x1080";
            comboBox3.Text = "24";
            comboBox5.Text = "mp4";
            comboBox2.Text = "192k";
            comboBox4.Text = "aac";
            textBox1.Text = ProjectContext.CurrentProject.Name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var dialog=new SaveFileDialog())
            {
                dialog.FileName = $"{textBox1.Text}.{comboBox5.Text}";
                dialog.Filter = ".MP4|*.mp4|.MKV|*.mkv|.AVI|*.avi|.MOV|*.mov";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = Path.GetFileNameWithoutExtension(dialog.FileName);
                    textBox2.Text=Path.GetDirectoryName(dialog.FileName);
                    comboBox5.Text = Path.GetExtension(dialog.FileName).TrimStart('.');
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            var RenderArgs = new RenderParams();
            RenderArgs.Resolution = comboBox1.Text;
            RenderArgs.Fps = int.Parse(comboBox3.Text);
            RenderArgs.VideoFormat = comboBox5.Text;
            RenderArgs.AudioBitrate = comboBox2.Text;
            RenderArgs.AudioFormat = comboBox4.Text;
            if (ProjectContext.CurrentProject != null){

                new RenderProgress().Show();
                await ProjectContext.CurrentProject.engine.RenderOptimizedSCFGAsync(RenderArgs,Path.Join(textBox2.Text, $"{textBox1.Text}.{comboBox5.Text}"));

            }
        }
    }
}
