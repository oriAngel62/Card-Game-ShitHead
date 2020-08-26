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
    
        private int computer = 0;//ניקוד מחשב
        private int player=0;//ניקוד שחקן

        private int scoring = 0;// ניקוד כללי: ניקוד שחקן - ניקוד מחשב
        private int numberOfWins = 0; //מספר ניצחונות
        private int numberOfLosses = 0;//מספר הפסדים


        private string name;//שם המשתמש

        private Graphics g;//גרפיקה

        public End()
        {            
         
            InitializeComponent();


            g = this.CreateGraphics();           
            MyText.SetEnd(this);
                    
        }

        

        public int GetPointsComputer()
        {
            //מחזירה את ניקוד המחשב
            return this.computer;
        }
        public int GetPointsPlayer()
        {
            //מחזירה את ניקוד השחקן
            return this.player;
        }

        public void SetComputer(int num)
        {
            //מעדכנת את ניקוד המחשב
            this.computer = num;
        }

        public void SetPlayer(int num)
        {
            //מעדכנת את ניקוד השחקן
            this.player = num;
        }

        public int GetScoring()
        {
            //מחזירה את הניקוד הכללי
            return this.scoring;
        }

        public void SetScoring()
        {
            //מעדכנת את הניקוד הכללי
            this.scoring = this.player-this.computer;
        }


        public void SetNumberOfWins()
        {
            //מעדכנת את מספר הניצחונות
            if (this.player > this.computer)
            {
                this.numberOfWins = this.numberOfWins + 1;
            }
        }
        public void SetNumberOfLosses()
        {
            //מעדכנת את מספר ההפסדים
            if (this.player < this.computer)
            {
                this.numberOfLosses = this.numberOfLosses + 1;
            }
        }

        public int GetNumberOfWins()
        {
            //מחזירה את מספר הנצחונות
          return  this.numberOfWins;
            
        }
        public int GetNumberOfLosses()
        {
            //מעדכנת את מספר ההפסדים
            return this.numberOfLosses;
        }
       

        private void End_Load(object sender, EventArgs e)
        {
            //עדכון הנתונים על המשתמש בעת העלאת הפורם
            SetScoring();
            SetNumberOfWins();
            SetNumberOfLosses();

           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //לחיצה על לחצןמשחק חדש
            Main_Game a = new Main_Game();
            a.Show();
            this.Close();
        }
   
        public string GetName()
        {
            //מחזירה את שם התמשתמש
            return this.name;
        }
        public void SetName(string name1)
        {
            //עדכון שם המשתמש
            this.name = name1;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //כתיבת שם המתמש על תיבת הטקסט
            SetName(textBox1.Text);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            //לחיצה על לחצן שמור ניקוד
            MyText text = new MyText();
            text.Main1();
            Table_of_records records = new Table_of_records();
            records.Show();
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
