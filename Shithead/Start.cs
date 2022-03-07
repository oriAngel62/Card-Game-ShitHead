using Shithead.DatabaseCommunication;
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
    public partial class Start : Form
    {
        public Start()
        {
          
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //End end = new End(true);
            //end.Show();
            //לחצן התחל משחק
            Main_Game a = new Main_Game();
            a.Show();
            this.Hide();                 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //לחצן הוראות
            instructions a = new instructions();
            a.Show();

        }    

        private void TableOfRecords_Click(object sender, EventArgs e)
        {
            ManageTableOfRecords manageTableOfRecords = new ManageTableOfRecords("", true);
        }

        private void Start_MouseClick(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }
    }
}
