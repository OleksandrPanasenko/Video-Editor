namespace VideoEditor.UI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            NewProjButton = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            OpenProjButton = new Button();
            listBox1 = new ListBox();
            label1 = new Label();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // NewProjButton
            // 
            NewProjButton.Anchor = AnchorStyles.Bottom;
            NewProjButton.Location = new Point(258, 136);
            NewProjButton.Name = "NewProjButton";
            NewProjButton.Size = new Size(163, 71);
            NewProjButton.TabIndex = 0;
            NewProjButton.Text = "New Project";
            NewProjButton.UseVisualStyleBackColor = true;
            NewProjButton.Click += button1_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(NewProjButton, 0, 0);
            tableLayoutPanel1.Controls.Add(OpenProjButton, 0, 1);
            tableLayoutPanel1.Controls.Add(listBox1, 0, 3);
            tableLayoutPanel1.Controls.Add(label1, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 74F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 37F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 182F));
            tableLayoutPanel1.Size = new Size(679, 503);
            tableLayoutPanel1.TabIndex = 1;
            tableLayoutPanel1.Paint += tableLayoutPanel1_Paint;
            // 
            // OpenProjButton
            // 
            OpenProjButton.Anchor = AnchorStyles.None;
            OpenProjButton.Location = new Point(258, 213);
            OpenProjButton.Name = "OpenProjButton";
            OpenProjButton.Size = new Size(163, 68);
            OpenProjButton.TabIndex = 1;
            OpenProjButton.Text = "Open project";
            OpenProjButton.UseVisualStyleBackColor = true;
            OpenProjButton.Click += OpenProjButton_Click;
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top;
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(153, 324);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(372, 104);
            listBox1.TabIndex = 2;
            listBox1.MouseClick += listBox1_MouseClick;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.Location = new Point(264, 292);
            label1.Name = "label1";
            label1.Size = new Size(150, 20);
            label1.TabIndex = 3;
            label1.Text = "Last opened projects:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(679, 503);
            Controls.Add(tableLayoutPanel1);
            Name = "Form1";
            Text = "Video Editor";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button NewProjButton;
        private TableLayoutPanel tableLayoutPanel1;
        private Button OpenProjButton;
        private ListBox listBox1;
        private Label label1;
    }
}
