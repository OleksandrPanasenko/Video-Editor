namespace Video_Editor
{
    partial class Settings
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
            label1 = new Label();
            checkBox1 = new CheckBox();
            colorDialog1 = new ColorDialog();
            numericUpDown1 = new NumericUpDown();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            button2 = new Button();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            label12 = new Label();
            button3 = new Button();
            button26 = new Button();
            button1 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            button8 = new Button();
            button9 = new Button();
            button10 = new Button();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(52, 76);
            label1.Name = "label1";
            label1.Size = new Size(70, 20);
            label1.TabIndex = 0;
            label1.Text = "Autosave";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(128, 79);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(18, 17);
            checkBox1.TabIndex = 1;
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(52, 122);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(61, 27);
            numericUpDown1.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(52, 99);
            label2.Name = "label2";
            label2.Size = new Size(170, 20);
            label2.TabIndex = 4;
            label2.Text = "Autosave time (minutes)";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(252, 167);
            label3.Name = "label3";
            label3.Size = new Size(116, 20);
            label3.TabIndex = 5;
            label3.Text = "Fragment colors";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(252, 187);
            label4.Name = "label4";
            label4.Size = new Size(48, 20);
            label4.TabIndex = 6;
            label4.Text = "Video";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(251, 207);
            label5.Name = "label5";
            label5.Size = new Size(49, 20);
            label5.TabIndex = 7;
            label5.Text = "Audio";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(251, 227);
            label6.Name = "label6";
            label6.Size = new Size(48, 20);
            label6.TabIndex = 8;
            label6.Text = "Photo";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(251, 247);
            label7.Name = "label7";
            label7.Size = new Size(36, 20);
            label7.TabIndex = 9;
            label7.Text = "Text";
            // 
            // button2
            // 
            button2.Location = new Point(299, 339);
            button2.Name = "button2";
            button2.Size = new Size(69, 32);
            button2.TabIndex = 10;
            button2.Text = "Apply";
            button2.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(52, 227);
            label8.Name = "label8";
            label8.Size = new Size(86, 20);
            label8.TabIndex = 15;
            label8.Text = "Label panel";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(52, 247);
            label9.Name = "label9";
            label9.Size = new Size(85, 20);
            label9.TabIndex = 14;
            label9.Text = "TimeStamp";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(52, 206);
            label10.Name = "label10";
            label10.Size = new Size(88, 20);
            label10.TabIndex = 13;
            label10.Text = "Background";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(53, 187);
            label11.Name = "label11";
            label11.Size = new Size(40, 20);
            label11.TabIndex = 12;
            label11.Text = "Lane";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(53, 167);
            label12.Name = "label12";
            label12.Size = new Size(125, 20);
            label12.TabIndex = 11;
            label12.Text = "Lane panel colors";
            // 
            // button3
            // 
            button3.Location = new Point(52, 339);
            button3.Name = "button3";
            button3.Size = new Size(69, 32);
            button3.TabIndex = 16;
            button3.Text = "Cancel";
            button3.UseVisualStyleBackColor = true;
            // 
            // button26
            // 
            button26.BackColor = Color.Black;
            button26.FlatAppearance.BorderColor = Color.White;
            button26.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 192, 255);
            button26.ForeColor = Color.White;
            button26.ImageKey = "dropper.ico";
            button26.Location = new Point(158, 187);
            button26.Name = "button26";
            button26.Size = new Size(20, 20);
            button26.TabIndex = 17;
            button26.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            button1.BackColor = Color.Black;
            button1.FlatAppearance.BorderColor = Color.White;
            button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 192, 255);
            button1.ForeColor = Color.White;
            button1.ImageKey = "dropper.ico";
            button1.Location = new Point(158, 206);
            button1.Name = "button1";
            button1.Size = new Size(20, 20);
            button1.TabIndex = 18;
            button1.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            button4.BackColor = Color.Black;
            button4.FlatAppearance.BorderColor = Color.White;
            button4.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 192, 255);
            button4.ForeColor = Color.White;
            button4.ImageKey = "dropper.ico";
            button4.Location = new Point(158, 227);
            button4.Name = "button4";
            button4.Size = new Size(20, 20);
            button4.TabIndex = 19;
            button4.UseVisualStyleBackColor = false;
            // 
            // button5
            // 
            button5.BackColor = Color.Black;
            button5.FlatAppearance.BorderColor = Color.White;
            button5.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 192, 255);
            button5.ForeColor = Color.White;
            button5.ImageKey = "dropper.ico";
            button5.Location = new Point(158, 247);
            button5.Name = "button5";
            button5.Size = new Size(20, 20);
            button5.TabIndex = 20;
            button5.UseVisualStyleBackColor = false;
            // 
            // button6
            // 
            button6.BackColor = Color.Black;
            button6.FlatAppearance.BorderColor = Color.White;
            button6.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 192, 255);
            button6.ForeColor = Color.White;
            button6.ImageKey = "dropper.ico";
            button6.Location = new Point(306, 187);
            button6.Name = "button6";
            button6.Size = new Size(20, 20);
            button6.TabIndex = 21;
            button6.UseVisualStyleBackColor = false;
            // 
            // button7
            // 
            button7.BackColor = Color.Black;
            button7.FlatAppearance.BorderColor = Color.White;
            button7.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 192, 255);
            button7.ForeColor = Color.White;
            button7.ImageKey = "dropper.ico";
            button7.Location = new Point(306, 206);
            button7.Name = "button7";
            button7.Size = new Size(20, 20);
            button7.TabIndex = 22;
            button7.UseVisualStyleBackColor = false;
            // 
            // button8
            // 
            button8.BackColor = Color.Black;
            button8.FlatAppearance.BorderColor = Color.White;
            button8.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 192, 255);
            button8.ForeColor = Color.White;
            button8.ImageKey = "dropper.ico";
            button8.Location = new Point(306, 227);
            button8.Name = "button8";
            button8.Size = new Size(20, 20);
            button8.TabIndex = 23;
            button8.UseVisualStyleBackColor = false;
            // 
            // button9
            // 
            button9.BackColor = Color.Black;
            button9.FlatAppearance.BorderColor = Color.White;
            button9.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 192, 255);
            button9.ForeColor = Color.White;
            button9.ImageKey = "dropper.ico";
            button9.Location = new Point(306, 247);
            button9.Name = "button9";
            button9.Size = new Size(20, 20);
            button9.TabIndex = 24;
            button9.UseVisualStyleBackColor = false;
            // 
            // button10
            // 
            button10.Location = new Point(174, 339);
            button10.Name = "button10";
            button10.Size = new Size(69, 32);
            button10.TabIndex = 25;
            button10.Text = "Defaults";
            button10.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(416, 450);
            Controls.Add(button10);
            Controls.Add(button9);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button1);
            Controls.Add(button26);
            Controls.Add(button3);
            Controls.Add(label8);
            Controls.Add(label9);
            Controls.Add(label10);
            Controls.Add(label11);
            Controls.Add(label12);
            Controls.Add(button2);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(numericUpDown1);
            Controls.Add(checkBox1);
            Controls.Add(label1);
            Name = "Settings";
            Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private CheckBox checkBox1;
        private ColorDialog colorDialog1;
        private NumericUpDown numericUpDown1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Button button2;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private Label label12;
        private Button button3;
        private Button button26;
        private Button button1;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
        private Button button9;
        private Button button10;
    }
}