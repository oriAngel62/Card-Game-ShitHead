﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shithead
{
    public partial class instructions : Form
    {
        public instructions()
        {
            InitializeComponent();
        }      

        private void instructions_MouseClick(object sender, MouseEventArgs e)
        {
          this.Close();
        }
        

      
    }
}
