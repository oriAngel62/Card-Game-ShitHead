﻿namespace Shithead
{
    partial class Table_of_records
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
            this.SuspendLayout();
            // 
            // Table_of_records
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Shithead.Properties.Resources.TableOfRecords;
            this.ClientSize = new System.Drawing.Size(959, 693);
            this.Name = "Table_of_records";
            this.Text = "Table_of_records";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Table_of_records_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Table_of_records_MouseClick);
            this.ResumeLayout(false);

        }

        #endregion


    }
}