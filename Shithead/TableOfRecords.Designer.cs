﻿namespace Shithead
{
    partial class TableOfRecords
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Shithead.Properties.Resources.TableOfRecords;
            this.ClientSize = new System.Drawing.Size(959, 693);
            this.Name = "TableOfRecords";
            this.Text = "TableOfRecords";
            this.Paint += new System.Windows.Forms.PaintEventHandler(TableOfRecords_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(TableOfRecords_MouseClick);
            this.ResumeLayout(false);

        }

        #endregion


    }
}