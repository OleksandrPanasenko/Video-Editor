namespace Video_Editor
{
    partial class RenderProgress
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            progressBar1 = new ProgressBar();
            progressBar2 = new ProgressBar();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            SuspendLayout();
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(46, 271);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(473, 31);
            progressBar1.TabIndex = 0;
            // 
            // progressBar2
            // 
            progressBar2.Location = new Point(46, 124);
            progressBar2.Name = "progressBar2";
            progressBar2.Size = new Size(473, 31);
            progressBar2.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(47, 82);
            label1.Name = "label1";
            label1.Size = new Size(144, 20);
            label1.TabIndex = 2;
            label1.Text = "File: cache/Lane1/83";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(47, 102);
            label2.Name = "label2";
            label2.Size = new Size(114, 20);
            label2.TabIndex = 3;
            label2.Text = "Speed: 603 kb/s";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(47, 229);
            label3.Name = "label3";
            label3.Size = new Size(37, 20);
            label3.TabIndex = 4;
            label3.Text = "83%";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(47, 249);
            label4.Name = "label4";
            label4.Size = new Size(168, 20);
            label4.TabIndex = 5;
            label4.Text = "Remaining time: 0m 43s";
            // 
            // RenderProgress
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(562, 363);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(progressBar2);
            Controls.Add(progressBar1);
            Name = "RenderProgress";
            Text = "Render Progress";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ProgressBar progressBar1;
        private ProgressBar progressBar2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}