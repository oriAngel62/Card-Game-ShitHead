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

namespace Shithead
{
    public partial class TableOfRecords : Form
    {
        private Graphics _graphics;

        private SQLDatabase db;

        public List<string> NamesAndScores { get; set; }

        public TableOfRecords()
        {
            this.InitializeComponent();
            _graphics = CreateGraphics();
            MyText.TableOfRecords = this;
            NamesAndScores = new List<string>();            
            ReadRecords();


            var source = new BindingSource();


            source.DataSource = db.PlayerData;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = source;
            //string n = dataGridView1.Rows[0].Cells[0].Value.ToString();


        }




        private void TableOfRecords_Paint(object sender, PaintEventArgs e)
        {
            List<string> checkSameNames = new List<string>();
            Node<string> position = NamesAndScores.GetFirst();
            Font font = new Font("Arial", 20);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            int x = 200;
            int y = 160;
            bool minus = false;

            while (position != null)
            {
                if (!(NameInList(checkSameNames, position.GetNext().GetInfo())))
                {

                    string score = position.GetInfo();
                    string numberOfWins = "", numberOfLosses = "", scoring = "";
                    int i = 0;
                    if (score != "")
                    {
                        char ch = score[i];
                        while (!(ch.Equals(' ')))
                        {
                            numberOfWins = numberOfWins + score[i];

                            i++;
                            if (score.Length > i)
                                ch = score[i];
                            else
                                return;
                        }


                        i++;
                        ch = score[i];
                        while (!(ch.Equals(' ')))
                        {
                            numberOfLosses = numberOfLosses + score[i];

                            i++;
                            ch = score[i];
                        }
                        i++;
                        ch = score[i];
                        while (!(ch.Equals(' ')))
                        {
                            if (score[i].Equals('-'))
                            {
                                minus = true;
                            }
                            else
                            {
                                scoring = scoring + score[i];
                            }
                            i++;
                            ch = score[i];
                        }



                        if (!minus)
                        {                        
                             _graphics.DrawString(position.GetNext().GetInfo() + "                   " + numberOfWins + "                   " + numberOfLosses + "                   " + scoring, font, drawBrush, x, y);
                           
                        }
                           
                        else
                        {                            
                              _graphics.DrawString("-" + position.GetNext().GetInfo() + "                   " + numberOfWins + "                   " + numberOfLosses + "                   " + scoring, font, drawBrush, x, y);
                            
                        }                                           
                    }

                    minus = false;
                    y = y + 50;
                }
                position = position.GetNext();
                if (position == null)
                    return;
                position = position.GetNext();

            }
            

        }

        public bool NameInList(List<string> checkSameNames, string name)
        {
            Node<string> position = checkSameNames.GetFirst();

            while (position != null)
            {
                if (position.GetInfo().Equals(name))
                    return true;
                position = position.GetNext();
            }
            position = checkSameNames.Insert(null, name);
            return false;
            

        }

        private void ReadRecords()
        {

            db=new SQLDatabase();
            string path = MyText.RECORDS_FILE_NAME;
            using (StreamReader streamReader = File.OpenText(path))
            {
                string line = streamReader.ReadLine();

                Node<string> position = NamesAndScores.GetFirst();
                while (line != null)
                {
                    NamesAndScores.Insert(null, line);
                    line = streamReader.ReadLine();
                }
            }

           
          
        }

        private void TableOfRecords_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }
    }
}

    

