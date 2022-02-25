using System;
using System.Linq;
using System.Text;
using Unit4.CollectionsLib;
using System.Drawing;
using System.Windows.Forms;
using Shithead.Enums;
using System.IO;

namespace Shithead
{
    public class Card 
    { 
        public int NumValue { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Shape Shape { get; set; }
        public static Main_Game MainGame { get; set; }
        public bool IsDraggingFromListFinal { get; set; } = false;// האם אפשר לגרור את הקלף מהקלפים שעל השולחן
        public static Game Game { get; set; } //תכונה סטטית לצורך שימוש בפעולות המחלקה
        public Stack<Undo> LastTurn { get; set; } = new Stack<Undo>(); //מחסנית לשמירת כל המצבים שבהם היה הקלף
        public bool BackFinal { get; set; }
        public PictureBox PictureBox { get; set; } // תמונה של הקלף

        private bool isDragging = false; //האם הקלף ניתן לגרירה
        private int oldMouseX;// מיקום איקס קודם של הקלף
        private int oldMouseY;//מיקום ויי קודם של הקלף        
      
        public Card(int num, Shape sh) //פעולה הבונה קלף לפי מספר וצורה
        {
            NumValue = num;
            Shape = sh;
            PictureBox = new PictureBox();
            PictureBox.Size = new Size(75, 109);
            PictureBox.Image = Image.FromFile($"Images/BackCard.PNG");

            PictureBox.MouseUp += new MouseEventHandler(PB_MouseUp);
            PictureBox.MouseDown += new MouseEventHandler(PB_MouseDown);
            PictureBox.MouseMove += new MouseEventHandler(PB_MouseMove);
        }

        public void PushStackLastTurn(Undo c)//מכניסה את המצב האחרון שבו היה הקלף למחסנית המצבים 
        {
            LastTurn.Push(c);
        }

        public void SetCard(PictureBox pictureBox)//עדכון תכונות התמונה
        {
            pictureBox.Image = Image.FromFile($"Images/{NumValue}{Shape.ToString()}.PNG");
        }

        public void SetBackCard()//הפיכת קלף מהפנים כלפי מעלה לפנים כלפי מטה
        {
            PictureBox.Image = Image.FromFile("Images/BackCard.PNG");
        }

        private void PB_MouseDown(object sender, MouseEventArgs e)//הרמה של קלף לצורך הזזתו
        //----------------------------------------------------------------------
        {
            if (((PictureBox)sender).Visible)
            {
                if (e.Button == MouseButtons.Left && PictureBox.Location.Y>300 )//בשביל הקלפים האחרונים לשים משתנה בוליאני שבסוף המשחק משתנה כל פעם בהתאם למצב
                {
                    if (PictureBox.Location.X > 450 || ((IsDraggingFromListFinal)&&(!BackFinal)))
                    {

                        DragJustOneCard(PictureBox);// סימון עבור ארוע התזוזה שאנחנו בתהליך גרירה
                        isDragging = true;
                     
                        oldMouseX = e.X;
                        oldMouseY = e.Y;

                        ((PictureBox)sender).BringToFront();
                        SetCard(this.PictureBox);
                    }
                     else
                    {
                        MessageBox.Show("לא ניתן לגרור את הקלפים שעל השולחן מכיוון שנשארו לך עוד קלפים ביד");
                    }
                }
                else
                {
                    if (PictureBox.Location.Y == 300)
                    {
                        MessageBox.Show("לא ניתן לגרור קלף מערימת הקלפים");
                    }
                    else
                    {
                        MessageBox.Show("לא ניתן לגרור את הקלפים של המחשב");
                    }
                }               
            }
        }

        //----------------------------------------------------------------------
        private void PB_MouseMove(object sender, MouseEventArgs e)//תנועה של קלף
        //----------------------------------------------------------------------
        {
            if (isDragging)
            {
                //  Y מעדכן קואורדינטות של התמונה בהתאם למידת תזוזת העכבר לכיוון 
                int movefromOldToEy = (e.Y - oldMouseY);
                ((PictureBox)sender).Top = ((PictureBox)sender).Top + movefromOldToEy;

                //  X מעדכן קואורדינטות של התמונה בהתאם למידת תזוזת העכבר לכיוון
                int movefromOldToEx = (e.X - oldMouseX);
                ((PictureBox)sender).Left = ((PictureBox)sender).Left + movefromOldToEx;


            }
        }

        //----------------------------------------------------------------------
        private void PB_MouseUp(object sender, MouseEventArgs e)//הנחה של קלף
        //----------------------------------------------------------------------
        {
            if (!isDragging)
                return;
           
            if (Game.GetStartGameButton())
            {
                Replace(PictureBox); 
                               
            }

            if ((PictureBox.Location.X > 660 && PictureBox.Location.X < 760) && (PictureBox.Location.Y > 260 && PictureBox.Location.Y < 360) && !Game.GetStartGameButton())// בדיקה שהקלף הונח על העירמה שבמרכז השולחן
            {
                Card card;
                List<Card> listPlayer = Game.GetCardStackPlayer().GetList();
              
                if (Game.CheckWin())
                    return;

               
                if (listPlayer.IsEmpty()&&IsDraggingFromListFinal)//האם הקלף נזרק מהקלפים שעל השולחן
                {
                    List<Card> listPlayerFinal=Game.GetCardStackPlayer().GetListFinal();                   
                    card = Game.GetCardStackPlayer().FindCard(listPlayerFinal, PictureBox); ////הוצאת הקלף מרשימת הקלפים שעל השולחן
                    card.IsDraggingFromListFinal = false;
                    Game.GetCardStackPlayer().InsertCardToList(listPlayer, card);//העברת הקלף מהשולחן אל היד כדי להשתמש בו
                    Game.GetCardStackPlayer().ChekCardInList(listPlayerFinal, card);//מחיקת הקלף מרשימת הקלפים שעל השולחן
                  
                  
                  
                    Node<Card> currentPosition = listPlayerFinal.GetFirst();

                    while (currentPosition != null)
                    {
                        currentPosition.GetInfo().IsDraggingFromListFinal = false;
                        currentPosition = currentPosition.GetNext();
                    }
                                      
                }


                else
                {
                 
                    MainGame.EnabledUndo(true);
                    card = Game.GetCardStackPlayer().FindCard(listPlayer, PictureBox);//הוצאת הקלף מרשימת הקלפים שביד
                
                }              
               
                     if (Game.IsBigger(Game.GetStack(), card))//בדיקה האם הקלף שנזרק יותר גדול מהקלף שבראש העירמה
                     {
                        PictureBox.Location = new Point(700, 300);
                        PictureBox.BringToFront();
                        card = Game.GetCardStackPlayer().FindCardAndRemove(listPlayer, PictureBox);                        

                        if (!Game.GetStack().IsEmpty())
                        {
                            if (Game.GetStack().Top().NumValue == 3)
                            {
                                Game.GetStack().Top().PictureBox.Location = new Point(700, 300);
                            }                           
                        }

                        PushStackLastTurn(Undo.PlayerThrowCard);
                        Game.PushStack(card);
                        Game.PushStackUndo(card); 

                        Game.RemoveCard(Game.GetListMemory(), card); //(הוצאת קלף שהמשתמש זרק מרשימת הזיכרון ( אם היא נמצאת שם  

                        bool cardInQueue = true; 
                        while (Game.GetCardStackPlayer().LengthList(listPlayer) < 3 && cardInQueue)
                        {
                            //השלמה לשלושה קלפים ביד ע"י שליפת קלפים מהקופה
                            if (!Game.GetCardStackPlayer().GetQueue().IsEmpty())
                            {
                                Game.DrawCard(card);
                                   
                            }
                            else
                                cardInQueue = false;                               
                        }                                             

                        if (card.NumValue == 10)
                        {
                            Game.TheNumIs10();                             
                                                          
                        }

                        if ((card.NumValue != 8) && (card.NumValue != 10) && !Game.FourCardsHaveTheSameNumber(Game.GetStack()))
                        {
                            //טיפול בזריקה של יותר מקלף אחד
                            int count = 0;

                            if(!Game.GetCardStackPlayer().GetList().IsEmpty())
                            {
                                count = Game.GetCardStackPlayer().HowManyCardsInList(Game.GetCardStackPlayer().GetList(), card);
                            }

                            if (count != 0) 
                            {

                                Game.ThrowPlayerSameNumber(Game.GetNum(), card);
                                Game.SetNum(1);
                                Game.OrganizeList(Game.GetCardStackPlayer(), "player");

                            }
                            if (Game.CheckWin())
                            {
                                return;
                            }

                            ChangeIsDragingBackFinal();
                            Game.ChangeTextBox();
                            MainGame.ChangeVisible(Game.FindSameCard());
                            Game.OrganizeList(Game.GetCardStackPlayer(), "player");
                            if (!(Game.FourCardsHaveTheSameNumber(Game.GetStack())))
                            {
                                Game.ComputerThrowCard();//העברת התור למחשב
                            }                         
                        }                     

                        ChangeIsDragingBackFinal();
                        Game.ChangeTextBox();
                        MainGame.ChangeVisible(Game.FindSameCard());
                        Game.OrganizeList(Game.GetCardStackPlayer(), "player");

                     }
                               
                                                                       
                     else
                     {
                         //זריקת קלף שעל פי החוקים לא ניתן לשים מעל הקלפים שנמצאים בערימה
                         MessageBox.Show("מהלך לא חוקי");                         
                         PictureBox.Location = new Point(X, Y);
                                             
                         if (card.IsDraggingFromListFinal && !card.BackFinal)
                         {
                             Game.GetCardStackPlayer().GetListFinal().Insert(null, card);
                         }

                     }
                 }
            
            else
            {
                //למנוע רמאות של בחירת קלף אחר מרשימת הקלפים שנמצאים על השולחן 
                if (Game.GetCardStackPlayer().GetLastCards()&&Game.GetCardStackPlayer().GetList().IsEmpty())
                {                   
                  Card  card = Game.GetCardStackPlayer().FindCard(Game.GetCardStackPlayer().GetListFinal(), PictureBox);
                  card.IsDraggingFromListFinal = true;                 
                  MessageBox.Show("אתה חייב להשתמש בקלף הזה");
                }
                  PictureBox.Location = new Point(X, Y);
            }

            isDragging = false;
        }

        public void Replace(PictureBox picturebox)
        {
            //הפעולה מאפשרת לפני תחילת המשחק להחליף בין הקלפים שנמצאים עם הפנים כלפי מעלה לקלפים שנמצאים ביד של השחקן
            //אחרי החלפת קלפים לבדוק את הלחצנים של לזרוק 2 או 3 קלפים
         
            if ((PictureBox.Location.X > 30 && PictureBox.Location.X < 70) && (PictureBox.Location.Y > 490 && PictureBox.Location.Y < 530))
            {
                Game.FindSameCard();
                MainGame.ChangeVisible(Game.GetChangeVisible());
                Game.OrganizeList(Game.GetCardStackPlayer(), "player");

                return;

            }


            if ((PictureBox.Location.X > 130 && PictureBox.Location.X < 170) && (PictureBox.Location.Y > 490 && PictureBox.Location.Y < 530))
            {
                Game.FindSameCard();
                MainGame.ChangeVisible(Game.GetChangeVisible());
                Game.OrganizeList(Game.GetCardStackPlayer(), "player");
                return;
            }
            
            if ((PictureBox.Location.X > 230 && PictureBox.Location.X < 270) && (PictureBox.Location.Y > 490 && PictureBox.Location.Y < 530))
            {
                Game.FindSameCard();
                MainGame.ChangeVisible(Game.GetChangeVisible());
                Game.OrganizeList(Game.GetCardStackPlayer(), "player");
                return;
            }  
        }


        public void DragJustOneCard(PictureBox PB)
        { 
            //חסימת האפשרות לגרור יותר מקלף אחד מהרשימה של הקלפים האחרונים           
            List<Card> listPlayerFinal = Game.GetCardStackPlayer().GetListFinal();
            Node<Card> pos = listPlayerFinal.GetFirst();
            if (Game.GetCardStackPlayer().LengthList(listPlayerFinal) <= 3)
            {

                while (pos != null)
                {
                    if (!(PB.Equals(pos.GetInfo().PictureBox)))
                    {
                        pos.GetInfo().IsDraggingFromListFinal = false;
                    }
                    pos = pos.GetNext();
                }             
                Game.GetCardStackPlayer().SetLastCards(true);
             
            }
       
      }


        public void ChangeIsDragingBackFinal()
        {
            //בסוף התור שינוי כל הקלפים שנמצאים הפוכים על השולחן של השחקן ללא ניתנים לגרירה
            if (Game.GetCardStackPlayer().GetList().IsEmpty())
            {
                List<Card> listPlayerFinal = Game.GetCardStackPlayer().GetListFinal(); //הרשימה ריקה
                Node<Card> pos = listPlayerFinal.GetFirst();
                int count = Game.GetCardStackPlayer().LengthList(listPlayerFinal);


                while (pos != null)
                {
                    pos.GetInfo().IsDraggingFromListFinal = true;
                    if (count == 3)
                    {
                        pos.GetInfo().BackFinal = false;
                    }
                    else
                    {
                        Game.GetListMemory().Insert(null, pos.GetInfo());
                    }
                    pos = pos.GetNext();
                }

            }

            else
            {
                List<Card> listPlayerFinal = Game.GetCardStackPlayer().GetListFinal();
                Node<Card> pos = listPlayerFinal.GetFirst();

                while (pos != null)
                {
                    Game.GetCardStackPlayer().FindCardAndRemove(Game.GetListMemory(),
                        pos.GetInfo().PictureBox);
                    pos = pos.GetNext();
                }

            }      


        }

    }

   
}
