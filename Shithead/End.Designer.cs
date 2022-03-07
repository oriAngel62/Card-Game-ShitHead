namespace Shithead
{
    partial class End
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
            this.newGame = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SaveScore = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // newGame
            // 
            this.newGame.Location = new System.Drawing.Point(763, 466);
            this.newGame.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.newGame.Name = "newGame";
            this.newGame.Size = new System.Drawing.Size(133, 64);
            this.newGame.TabIndex = 1;
            this.newGame.Text = "משחק חדש";
            this.newGame.UseVisualStyleBackColor = true;
            this.newGame.Click += new System.EventHandler(this.NewGame_Click);
            // 
            // textBox1
            // 
            this.textBox1.AccessibleName = "";
            this.textBox1.Location = new System.Drawing.Point(267, 468);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBox1.Size = new System.Drawing.Size(103, 22);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // SaveScore
            // 
            this.SaveScore.Location = new System.Drawing.Point(143, 468);
            this.SaveScore.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SaveScore.Name = "SaveScore";
            this.SaveScore.Size = new System.Drawing.Size(100, 28);
            this.SaveScore.TabIndex = 3;
            this.SaveScore.Text = "שמור ניקוד";
            this.SaveScore.UseVisualStyleBackColor = true;
            this.SaveScore.Click += new System.EventHandler(this.SaveScore_Click);
            // 
            // End
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Shithead.Properties.Resources.youWin;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(912, 642);
            this.Controls.Add(this.SaveScore);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.newGame);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "End";
            this.Text = "End";
            this.Load += new System.EventHandler(this.End_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.End_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button newGame;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button SaveScore;
    }
}