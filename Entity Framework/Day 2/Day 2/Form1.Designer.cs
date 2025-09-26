namespace Day_2
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
            DGV = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            CityT = new TextBox();
            NameT = new TextBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            AgeT = new NumericUpDown();
            button4 = new Button();
            ((System.ComponentModel.ISupportInitialize)DGV).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AgeT).BeginInit();
            SuspendLayout();
            // 
            // DGV
            // 
            DGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DGV.Location = new Point(355, 12);
            DGV.Name = "DGV";
            DGV.Size = new Size(433, 426);
            DGV.TabIndex = 0;
            DGV.CellClick += DGV_CellClick;
            DGV.CellContentClick += dataGridView1_CellContentClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 70);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 1;
            label1.Text = "Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 139);
            label2.Name = "label2";
            label2.Size = new Size(28, 15);
            label2.TabIndex = 3;
            label2.Text = "Age";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 202);
            label3.Name = "label3";
            label3.Size = new Size(28, 15);
            label3.TabIndex = 4;
            label3.Text = "City";
            // 
            // CityT
            // 
            CityT.Location = new Point(98, 199);
            CityT.Name = "CityT";
            CityT.Size = new Size(164, 23);
            CityT.TabIndex = 2;
            // 
            // NameT
            // 
            NameT.Location = new Point(98, 67);
            NameT.Name = "NameT";
            NameT.Size = new Size(164, 23);
            NameT.TabIndex = 2;
            // 
            // button1
            // 
            button1.Location = new Point(12, 288);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 5;
            button1.Text = "Add";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(93, 288);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 5;
            button2.Text = "Update";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(174, 288);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 5;
            button3.Text = "Delete";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // AgeT
            // 
            AgeT.Location = new Point(98, 137);
            AgeT.Name = "AgeT";
            AgeT.Size = new Size(164, 23);
            AgeT.TabIndex = 7;
            // 
            // button4
            // 
            button4.Location = new Point(255, 288);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 8;
            button4.Text = "Reset";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button4);
            Controls.Add(AgeT);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(CityT);
            Controls.Add(NameT);
            Controls.Add(label1);
            Controls.Add(DGV);
            Name = "Form1";
            Text = "DB Approach";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)DGV).EndInit();
            ((System.ComponentModel.ISupportInitialize)AgeT).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView DGV;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox CityT;
        private TextBox NameT;
        private Button button1;
        private Button button2;
        private Button button3;
        private NumericUpDown AgeT;
        private Button button4;
    }
}
