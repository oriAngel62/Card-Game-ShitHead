using System;
//using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Unit4.CollectionsLib;
using System.IO;






namespace Shithead
{
    public partial class Table_of_records : Form
    {
        private Graphics g;//גרפיקה
        private List<string> namesAndScore;//רשימת המשתמשים ששיחקו במשחק והתוצאות שלהם
     


        public Table_of_records()
        {
           //פעולה בונה המעדכנת את טבלת השיאים
            InitializeComponent();
            g = this.CreateGraphics();
            MyText.SetTableOfRecords(this);
            namesAndScore = new List<string>();            
           ReadingText();


        }


        private void Table_of_records_Paint(object sender, PaintEventArgs e)
        {
            //ציור טבלת השיאים          
            List<string> checkSameNames = new List<string>();
            Node<string> pos = namesAndScore.GetFirst();
            Font f = new Font("Arial", 20);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            int x = 200;
            int y = 160;
            bool minus=false;

            while (pos != null)
            {
                if (!(NameInList(checkSameNames, pos.GetNext().GetInfo())))
                {

                    string score = pos.GetInfo();
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
                             g.DrawString(pos.GetNext().GetInfo() + "                   " + numberOfWins + "                   " + numberOfLosses + "                   " + scoring, f, drawBrush, x, y);
                           
                        }
                           
                        else
                        {                            
                              g.DrawString("-" + pos.GetNext().GetInfo() + "                   " + numberOfWins + "                   " + numberOfLosses + "                   " + scoring, f, drawBrush, x, y);
                            
                        }                                           
                    }

                    minus = false;
                    y = y + 50;
                }
                pos = pos.GetNext();
                if (pos == null)
                    return;
                pos = pos.GetNext();

            }
            

        }


        public List<string> GetListNames()
        {
            // ממחזירה את רשימת השמות והתוצאות
            return this.namesAndScore;
        }
        public void SetListNames(List<string> names1)
        {
            //מעדכנת את רשימת השמות והתוצאות
            this.namesAndScore = names1;
        }


        public bool NameInList(List<string> checkSameNames, string name)
        {
            //הפעולה מקבלת רשימת שמות ושם ובודקת האם השם נמצא ברשימה אם השם לא נמצא הפעולה תכניס אותו לרשימה
            Node<string> pos = checkSameNames.GetFirst();

            while (pos != null)
            {
                if (pos.GetInfo().Equals(name))
                    return true;
                pos = pos.GetNext();
            }
            pos = checkSameNames.Insert(null, name);
            return false;
            

        }

        public void ReadingText()
        {
            //קריאת הטקסט החיצוני
            string path = "MyTest.txt";
            using (StreamReader sr = File.OpenText(path))
            {
                string s = sr.ReadLine();

                Node<string> pos = GetListNames().GetFirst();
                while (s!= null)
                {
               
                    GetListNames().Insert(null, s);
                    s = sr.ReadLine();
                                  
                }
            }
        }

        private void Table_of_records_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }

    }
}

    

