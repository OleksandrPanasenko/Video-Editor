using Core;
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
namespace Video_Editor
{
    public partial class Settings : Form
    {
        public AppSettings settings;
        public Settings()
        {
            InitializeComponent();
            settings = AppSettings.FromSettings();
            RefreshParametres();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button26_Click(object sender, EventArgs e)
        {
            //Lane Color
            SelectColor(button26, ()=> settings.LaneColor,c=> settings.LaneColor=c);
        }
        private void SelectColor(Button button, Func<Color> getter, Action<Color> setter)
        {
            colorDialog1.AllowFullOpen = true;
            colorDialog1.Color = getter();
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                setter(colorDialog1.Color);
            }
            button.BackColor = getter();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //Background Color
            SelectColor(button1, () => settings.LaneBackgroundColor, c => settings.LaneBackgroundColor = c);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Label Panel Color
            SelectColor(button4, () => settings.LabelPanelColor, c => settings.LabelPanelColor = c);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //TimeStamp Color
            SelectColor(button5, () => settings.TimeStamp, c => settings.TimeStamp = c);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Video Fragment Color
            SelectColor(button6, () => settings.VideoFragmentColor, c => settings.VideoFragmentColor = c);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Audio Fragment Color
            SelectColor(button7, () => settings.AudioFragmentColor, c => settings.AudioFragmentColor = c);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //Photo Fragment Color
            SelectColor(button8, () => settings.ImageFragmentColor, c => settings.ImageFragmentColor = c);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //Text Fragment Color
            SelectColor(button9, () => settings.TextFragmentColor, c => settings.TextFragmentColor = c);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Cancel
            Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //Back to defaults
            AppSettings.Default.ApplySettings();
            settings = AppSettings.FromSettings();
            RefreshParametres();
            ConfigManager.SaveSettings(settings);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Apply changes
            settings.ApplySettings();
            ConfigManager.SaveSettings(settings);

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            //Autosave Interval
            settings.AutoSaveTimeMinutes = (int)numericUpDown1.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //Changed Autosave
            settings.AutoSave=checkBox1.Checked;
        }
        private void RefreshParametres()
        {
            // Update the BackColor for all buttons that use SelectColor()
            button26.BackColor = settings.LaneColor;
            button1.BackColor = settings.LaneBackgroundColor;
            button4.BackColor = settings.LabelPanelColor;
            button5.BackColor = settings.TimeStamp;
            button6.BackColor = settings.VideoFragmentColor;
            button7.BackColor = settings.AudioFragmentColor;
            button8.BackColor = settings.ImageFragmentColor;
            button9.BackColor = settings.TextFragmentColor;

            // Update numericUpDown1 (Autosave Interval)
            numericUpDown1.Value = settings.AutoSaveTimeMinutes;

            // Update checkBox1 (Autosave Checked State)
            checkBox1.Checked = settings.AutoSave;
        }

    }
}
