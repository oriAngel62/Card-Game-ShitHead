using System;
using System.IO;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unit4.CollectionsLib;
using System.Drawing;
using System.Windows.Forms;



namespace Shithead
{
    class MyText
    {

        private List<string> listNames = new List<string>();//רשימת שמות

        static Table_of_records table_of_records;//שימוש בפעולות הפורם
        static End end;//שימוש בפעולות הפורם
        private string score="";//ניקוד
        private string name = "";//שם משתמש נוכחי

        public static void SetTableOfRecords(Table_of_records table_of_records1)
        {
            //שימוש בפעולות הפורם
            table_of_records = table_of_records1;
        }

        public static void SetEnd(End end1)
        {
            //שימוש בפעולות הפורם
            end = end1;
        }

        public List<string> GetListNames()
        {
            //מחזירה רשימת שמות
            return this.listNames;
        }
        public void SetListNames(List<string> names1)
        {
            //מעדכנת רשימת שמות
            this.listNames = names1;
        }

        public string GetScore()
        {
            //מחיזרה ניקוד
            return score;
        }

        public void SetScore(string score1)
        {
            //מעדכנת ניקוד
            this.score = score1;
        }


        public string GetName()
        {
            //מחזירה שם משתמש נוכחי
            return name;
        }

        public void SetName(string name1)
        {
            //מעדכנת שם משתמש נוכחי
            this.name = name1;
        }

        public void Main1()
        {
            //פעולה ראשית שמטרתה לכתוב את הנתונים על המשתמש בטקסט חיצוני שבהמשך יהיה ניתן לקריאה ולהדפסה בטבלת שיאים
            if (end != null)
            {
               RefreshName();
               WritingText();
                
            }
            //ReadingText();
        }

        public void WritingText()
        {
            //הפעולה מוסיפה את השחקן הנוכחי לטבלת השיאים. אם השחקן שיחק פעם במשחק היא מעדכנת את הסטטיסטיקה שלו
            FileStream fs;
            string path = "MyTest.txt";

            if (File.Exists(path))
            {
                fs = new FileStream(path, FileMode.Append, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            }

            StreamWriter writer = new StreamWriter(fs);
            
            if (this.score.Equals(""))
            {
                writer.WriteLine(end.GetName());
                writer.WriteLine(end.GetNumberOfWins() + " " + end.GetNumberOfLosses() + " " + end.GetScoring()+" ");
            }
            else
            {
                writer.WriteLine(GetName());
                string numberOfWins="", numberOfLosses="", scoring="";
                int numberOfWins2=0, numberOfLosses2=0,scoring2=0;
                int i=0;
                if(score!=null)
                {
                    char ch = score[i];
                    while (!(ch.Equals(' ')))
                    {
                        numberOfWins = numberOfWins + score[i];
                        
                        i++;
                        ch = score[i];
                    }
                    int NumberOfWins1 = int.Parse(numberOfWins);
                    numberOfWins2 = NumberOfWins1 + end.GetNumberOfWins();

                    i++;
                    ch = score[i];
                    while (!(ch.Equals(' ')))
                    {
                        numberOfLosses = numberOfLosses + score[i];
                       
                        i++;
                        ch = score[i];
                    }
                    int NumberOfLosses1 = int.Parse(numberOfLosses);
                    numberOfLosses2 = NumberOfLosses1 + end.GetNumberOfLosses();
                    i++;
                    ch = score[i];
                    while (!(ch.Equals(' ')))
                    {

                        scoring = scoring + score[i];
                       
                        i++;
                        ch = score[i];
                    }
                    int scoring1 = int.Parse(scoring);
                    scoring2 = scoring1 + end.GetScoring();
                    writer.WriteLine(numberOfWins2 + " " + numberOfLosses2 + " " + scoring2+" ");
                  

                }
           

            }
            writer.Close();

            fs.Close();

        }

        public void RefreshName()
        {
            //הפעולה בודקת האם השחקן שיחק פעם במשחק
            string path = "MyTest.txt";
            using (StreamReader sr = File.OpenText(path))
            {
                string s = sr.ReadLine();             
                string name = end.GetName();
                while (s != null)
                {
                  
                    if (s.Equals(name))
                    {                        
                        SetName(s);                      
                        s = sr.ReadLine();                       
                        SetScore(s);                                               
                    }
                    
                    s = sr.ReadLine();

                }              
            }
        }
    }
}
