using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shithead
{
    public partial class End : Form
    {
       public string Name { get; set; }
       public bool Win { get;set; }

        private Graphics g;//גרפיקה

        public End(bool win)
        {            
         
            InitializeComponent();
            g = this.CreateGraphics();           
            Win = win;            
        }
        private void End_Load(object sender, EventArgs e)
        {
           
        }

        private void NewGame_Click(object sender, EventArgs e)
        {
            Main_Game a = new Main_Game();
            a.Show();
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Name = textBox1.Text;
        }

        private void SaveScore_Click(object sender, EventArgs e)
        {
            ManageTableOfRecords manageTableOfRecords = new ManageTableOfRecords(Name,Win);
            this.Close();
        }

        private void End_Paint(object sender, PaintEventArgs e)
        {
            Font f = new Font("Arial", 20);
            SolidBrush drawBrush = new SolidBrush(Color.Green);
            g.DrawString("הקלד את שמך", f, drawBrush, 285, 370);
        }
    }
}
