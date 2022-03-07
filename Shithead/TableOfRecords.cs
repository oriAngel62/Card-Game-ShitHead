using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Unit4.CollectionsLib;
using System.IO;
using Shithead.DatabaseCommunication;
using System.Data.SqlClient;

namespace Shithead
{
    public partial class TableOfRecords : Form
    {
        private Graphics _graphics;

        public DataGridView DataGrid { set; get; }

        public TableOfRecords()
        {
            this.InitializeComponent();
            _graphics = CreateGraphics();
            DataGrid = this.dataGridView1;
            //ManageTableOfRecords.TableOfRecords = this;
        }




        private void TableOfRecords_Paint(object sender, PaintEventArgs e)
        {
            //List<string> checkSameNames = new List<string>();
            //Node<string> position = NamesAndScores.GetFirst();
            //Font font = new Font("Arial", 20);
            //SolidBrush drawBrush = new SolidBrush(Color.Black);
            //int x = 200;
            //int y = 160;
            //bool minus = false;

            //while (position != null)
            //{
            //    if (!(NameInList(checkSameNames, position.GetNext().GetInfo())))
            //    {

            //        string score = position.GetInfo();
            //        string numberOfWins = "", numberOfLosses = "", scoring = "";
            //        int i = 0;
            //        if (score != "")
            //        {
            //            char ch = score[i];
            //            while (!(ch.Equals(' ')))
            //            {
            //                numberOfWins = numberOfWins + score[i];

            //                i++;
            //                if (score.Length > i)
            //                    ch = score[i];
            //                else
            //                    return;
            //            }


            //            i++;
            //            ch = score[i];
            //            while (!(ch.Equals(' ')))
            //            {
            //                numberOfLosses = numberOfLosses + score[i];

            //                i++;
            //                ch = score[i];
            //            }
            //            i++;
            //            ch = score[i];
            //            while (!(ch.Equals(' ')))
            //            {
            //                if (score[i].Equals('-'))
            //                {
            //                    minus = true;
            //                }
            //                else
            //                {
            //                    scoring = scoring + score[i];
            //                }
            //                i++;
            //                ch = score[i];
            //            }



            //            if (!minus)
            //            {                        
            //                 _graphics.DrawString(position.GetNext().GetInfo() + "                   " + numberOfWins + "                   " + numberOfLosses + "                   " + scoring, font, drawBrush, x, y);
                           
            //            }
                           
            //            else
            //            {                            
            //                  _graphics.DrawString("-" + position.GetNext().GetInfo() + "                   " + numberOfWins + "                   " + numberOfLosses + "                   " + scoring, font, drawBrush, x, y);
                            
            //            }                                           
            //        }

            //        minus = false;
            //        y = y + 50;
            //    }
            //    position = position.GetNext();
            //    if (position == null)
            //        return;
            //    position = position.GetNext();

            //}
            

        }

   
        private void TableOfRecords_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }
    }
}

    

