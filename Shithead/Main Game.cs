using Shithead.Enums;
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
    public partial class Main_Game : Form
    {

        private Graphics g;//גרפיקה
        Card_Stack CS1;//כל הקלפים שיש ברשות השחקן
        Card_Stack CS2;//כל הקלפים שיש ברשות המחשב
        Game game;//כל הקלפים במשחק
        Board board;//כל הקלפים ומיקומם

        public Main_Game()
        {
            //פעולה בונה הבונה את הלוח והקלפים
            InitializeComponent();

            g = this.CreateGraphics();
            CS1 = new Card_Stack();
            CS2 = new Card_Stack();
            game = new Game(CS1, CS2, this);
            board = new Board(game);
            Card.Game = game;
            Card.MainGame = this;
            Game.SetMainGame(this);
            Board.SetMainGame(this);                             
            game.SetLevel(Level.Normal);
            game.SetShow(ShowComputerCards.No); //כשאני מסיים את המשחק לשנות את זה ל no
            board.StartLocation(game);



                 
        }

        private void Main_Game_Paint(object sender, PaintEventArgs e)
        {
            //ציור ממסגרת המסמנת איפה להניח את הקלפים
            Pen p = new Pen(Color.Black, 1);
            g.DrawRectangle(p, 700, 300, 75, 109);
            Font f = new Font("Arial", 15);
            Font f1 = new Font("Arial", 10);
            SolidBrush drawBrush = new SolidBrush(Color.Black);           

            if (button2.Visible)
            {
                g.DrawString("ערימת", f, drawBrush, 715, 330);
                g.DrawString("הקלפים", f, drawBrush, 700, 350);
                g.DrawString("קופה מחשב", f1, drawBrush, 360, 30);
                g.DrawString("קופה שחקן", f1, drawBrush, 360, 709);

                g.DrawString("הקלפים שביד השחקן", f, drawBrush, 600, 709);
                g.DrawString("הקלפים שביד המחשב", f, drawBrush, 600, 20);
                g.DrawString("הקלפים של השחקן על השולחן", f, drawBrush, 90, 629);
                g.DrawString("הקלפים של המחשב על השולחן", f, drawBrush, 90, 100);
                g.DrawString("החלף את הקלפים שבידך בקלפים שנמצאים על השולחן על פי שיקול דעתך ובסיום לחץ על התחל משחק", f, drawBrush, 80, 430);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!game.GetStack().IsEmpty())
            {
                game.TakeCards("player");               
                game.ComputerThrowCard();
            }
           
        }
       
        public void ChangeVisible(int num)
        {
            if (num == 1)
            {
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
            }

            if (num == 2)
            {
                button3.Enabled = true;
            }
            if (num == 3)
            {
                button3.Enabled = true;
                button4.Enabled = true;
            }
            if (num == 4)
            {
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
            }
        }

        public void EnabledUndo(bool boolian)
        {
            if (boolian == true)
            {
                button6.Enabled = true;
            }
            else
            {
                button6.Enabled = false;
            }
        }
        public Button GetUndoButton()
        {
            return button6;
        }
        public Button GetTakeCardsButton()
        {
            return button1;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            game.SetStartGameButton(false);
            button2.Visible = false;
            this.Refresh();

            if ((game.GetLevel() == Level.Normal) || (game.GetLevel() == Level.Hard))
            {
                game.ComputerReplaceCards();
                game.OrganizeList(game.GetCardStackComputer(), "computer");
            }                      
        }

        public TextBox GetTextBox1()
        {
            return textBox1;
        }

        public TextBox GetTextBox2()
        {
            return textBox2;
        }

        public int isclick(int num)
        {          
                return num;          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            game.SetNum(2);
        }


        private void button4_Click(object sender, EventArgs e)
        {
            game.SetNum(3);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            game.SetNum(4);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            game.UndoFunc();        
            if (game.GetStackUndo().IsEmpty())
            {
                button6.Enabled = false;
            }
            ChangeVisible(game.FindSameCard());
            game.OrganizeList(game.GetCardStackComputer(), "computer");
            game.OrganizeList(game.GetCardStackPlayer(), "player");
        }

      public void End(int pointsComputer,int pointsPlayer)
      {
          End end = new End();
          end.SetComputer(pointsComputer);
          end.SetPlayer(pointsPlayer);
        

          if (pointsComputer > pointsPlayer)
          {
              end.BackgroundImage = Shithead.Properties.Resources.youLose;
          }
          game.GetStack().Top().SetCard(game.GetStack().Top().PictureBox);
          game.GetStack().Top().PictureBox.BringToFront();          
          Refresh();
          System.Threading.Thread.Sleep(300);
          game.GetStack().Top().PictureBox.BringToFront();
          end.Show();        
          this.Close();
      }     

      private void קלToolStripMenuItem_Click(object sender, EventArgs e)
      {
          game.SetLevel(Level.Easy);
          קלToolStripMenuItem.Checked = true;
          בינוניToolStripMenuItem.Checked = false;
          קשהToolStripMenuItem.Checked = false;
      }

      private void בינוניToolStripMenuItem_Click(object sender, EventArgs e)
      {
          game.SetLevel(Level.Normal);
          בינוניToolStripMenuItem.Checked = true;
          קשהToolStripMenuItem.Checked = false;
          קלToolStripMenuItem.Checked = false;
      }

      private void קשהToolStripMenuItem_Click(object sender, EventArgs e)
      {
          game.SetLevel(Level.Hard);
          קשהToolStripMenuItem.Checked = true;
          קלToolStripMenuItem.Checked = false;
          בינוניToolStripMenuItem.Checked = false;
      }

      private void משחקחדשToolStripMenuItem_Click(object sender, EventArgs e)
      {
          instructions a = new instructions();
          a.Show();
      }
               

      private void חשוףToolStripMenuItem_Click(object sender, EventArgs e)
      {
          game.SetShow(ShowComputerCards.Yes);
          חשוףToolStripMenuItem.Checked = true;
          הסתרToolStripMenuItem.Checked = false;
          game.OrganizeList(game.GetCardStackComputer(), "computer");
          
      }

      private void הסתרToolStripMenuItem_Click(object sender, EventArgs e)
      {
          game.SetShow(ShowComputerCards.No);
          הסתרToolStripMenuItem.Checked = true;
          חשוףToolStripMenuItem.Checked = false;
          game.OrganizeList(game.GetCardStackComputer(), "computer");
      }

      private void יציאהלמסךהראשיToolStripMenuItem_Click(object sender, EventArgs e)
      {
          Application.Exit();
      }

      private void טבלתשיאיםToolStripMenuItem_Click(object sender, EventArgs e)
      {
          TableOfRecords a = new TableOfRecords();
          a.Show();
      }

      private void יציאהToolStripMenuItem_Click(object sender, EventArgs e)
      {
          Main_Game a = new Main_Game();
          a.Show();
          this.Close();
      }

   
    }
}
