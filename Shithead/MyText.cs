using System;
using System.IO;
using System.Linq;
using System.Text;
using Unit4.CollectionsLib;
using System.Drawing;
using System.Windows.Forms;

namespace Shithead
{
    public class MyText
    {
        public List<string> Names { get; set;  } = new List<string>();

        public static DatabaseCommunication.SQLDatabase SQLDatabase { get; set; }
        public static TableOfRecords TableOfRecords { get; set; }
        public static End End { get; set; }
        public string Score { get; set; } = "";
        public string CurrentName { get; set; } = "";

        public static readonly string RECORDS_FILE_NAME = "Images/MyTest.txt";




        public void WriteToForm()
        {
            SQLDatabase = new DatabaseCommunication.SQLDatabase();
        }
        public void WriteRecordsToFile()
        {
            if (End != null)
            {
               UpdateNameByRecords();
               WritingText();
            }
        }

        private void UpdateNameByRecords()
        {
            using (StreamReader streamReader = File.OpenText(RECORDS_FILE_NAME))
            {
                string lineRead = streamReader.ReadLine();
                string name = End.GetName();
                while (lineRead != null)
                {
                    if (lineRead.Equals(name))
                    {
                        CurrentName = lineRead;
                        lineRead = streamReader.ReadLine();
                        Score = lineRead;
                    }
                    lineRead = streamReader.ReadLine();
                }
            }
        }

        private void WritingText()
        {
            FileMode fileMode = FileMode.Create;

            if (File.Exists(RECORDS_FILE_NAME))
                fileMode = FileMode.Append;

            using(FileStream fileStream = new FileStream(RECORDS_FILE_NAME, fileMode, FileAccess.Write))
            {
                using(StreamWriter writer = new StreamWriter(fileStream))
                {
                    if (Score.Equals(""))
                    {
                        WritePlayerHistoryToFile(writer);
                    }
                    else
                    {
                        writer.WriteLine(CurrentName);
                        string numberOfWins = "", numberOfLosses = "", scoring = "";
                        int numberOfWins2 = 0, numberOfLosses2 = 0, scoring2 = 0;
                        int i = 0;

                        char ch = Score[i];
                        while (!(ch.Equals(' ')))
                        {
                            numberOfWins = numberOfWins + Score[i];

                            i++;
                            ch = Score[i];
                        }
                        int NumberOfWins1 = int.Parse(numberOfWins);
                        numberOfWins2 = NumberOfWins1 + End.GetNumberOfWins();

                        i++;
                        ch = Score[i];
                        while (!(ch.Equals(' ')))
                        {
                            numberOfLosses = numberOfLosses + Score[i];

                            i++;
                            ch = Score[i];
                        }
                        int NumberOfLosses1 = int.Parse(numberOfLosses);
                        numberOfLosses2 = NumberOfLosses1 + End.GetNumberOfLosses();
                        i++;
                        ch = Score[i];
                        while (!(ch.Equals(' ')))
                        {

                            scoring = scoring + Score[i];

                            i++;
                            ch = Score[i];
                        }
                        int scoring1 = int.Parse(scoring);
                        scoring2 = scoring1 + End.GetScoring();
                        writer.WriteLine(numberOfWins2 + " " + numberOfLosses2 + " " + scoring2 + " ");
                    }
                }
            }
        }

        private void WritePlayerHistoryToFile(StreamWriter writer)
        {
            writer.WriteLine(End.GetName());
            writer.WriteLine(End.GetNumberOfWins() + " " + End.GetNumberOfLosses() + " " + End.GetScoring() + " ");
        }
    }
}
