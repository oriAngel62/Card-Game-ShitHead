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
        private int num;//ערך הקלף
        public int X { get; set; }
        public int Y { get; set; }
        public Shape shape;
        public static Main_Game MainGame { get; set; }
        private bool isDraggingFromListFinal = false;// האם אפשר לגרור את הקלף מהקלפים שעל השולחן
        private PictureBox pictureBox;// תמונה של הקלף
        public static Game Game { get; set; } //תכונה סטטית לצורך שימוש בפעולות המחלקה
        private bool isDragging = false; //האם הקלף ניתן לגרירה
        private int oldMouseX;// מיקום איקס קודם של הקלף
        private int oldMouseY;//מיקום ויי קודם של הקלף        
        
        private Stack<Undo> lastTurn=new Stack<Undo>(); //מחסנית לשמירת כל המצבים שבהם היה הקלף
        public bool BackFinal { get; set; }

        public Stack<Undo> GetStackLastTurn()//מחזירה את המצב האחרון שבו היה הקלף
        {
            return this.lastTurn;
        }

        public void PushStackLastTurn(Undo c)//מכניסה את המצב האחרון שבו היה הקלף למחסנית המצבים 
        {
            this.lastTurn.Push(c);
        }

      
        public Card(int num, Shape sh) //פעולה הבונה קלף לפי מספר וצורה
        {
            this.pictureBox = new PictureBox();

            this.num = num;
            this.shape = sh;

            pictureBox.Size = new Size(75, 109);


            pictureBox.Image = Image.FromFile($"Images/BackCard.PNG");

            this.pictureBox.MouseUp += new MouseEventHandler(PB_MouseUp);
            this.pictureBox.MouseDown += new MouseEventHandler(PB_MouseDown);
            this.pictureBox.MouseMove += new MouseEventHandler(PB_MouseMove);
        }

        public void SetCard(PictureBox PB)//עדכון תכונות התמונה
        {

            PB.Image = Image.FromFile($"Images/{num}{shape.ToString()}.PNG");
        }

        public void SetBackCard()//הפיכת קלף מהפנים כלפי מעלה לפנים כלפי מטה
        {
            pictureBox.Image = Image.FromFile("Images/BackCard.PNG");
        }

        public void SetIsDraggingFromListFinal(bool boolian)//עדכון אפשרות גרירה מהקלפים שנמצאים על השולחן
        {
            this.isDraggingFromListFinal = boolian;
        }

        public bool GetIsDraggingFromListFinal()//מחזירה האם אפשר לגרור את הקלפים שנמצאים על השולחן
        {
            return this.isDraggingFromListFinal;
        }


        public int GetNum()//מחזירה את המספר של הקלף
        {
            return this.num;
        }

        public PictureBox GetPictureBox()//מחזירה את התמונה של הקלף
        {
            return this.pictureBox;
        }
      

        private void PB_MouseDown(object sender, MouseEventArgs e)//הרמה של קלף לצורך הזזתו
        //----------------------------------------------------------------------
        {
        
            if (((PictureBox)sender).Visible)
            {
                if (e.Button == MouseButtons.Left && pictureBox.Location.Y>300 )//בשביל הקלפים האחרונים לשים משתנה בוליאני שבסוף המשחק משתנה כל פעם בהתאם למצב
                {
                    if (pictureBox.Location.X > 450 || ((isDraggingFromListFinal)&&(!BackFinal)))
                    {

                        DragJustOneCard(pictureBox);// סימון עבור ארוע התזוזה שאנחנו בתהליך גרירה
                        isDragging = true;
                     
                        oldMouseX = e.X;
                        oldMouseY = e.Y;

                        ((PictureBox)sender).BringToFront();
                        SetCard(this.pictureBox);
                    }
                     else
                    {
                        MessageBox.Show("לא ניתן לגרור את הקלפים שעל השולחן מכיוון שנשארו לך עוד קלפים ביד");
                    }
                }
                else
                {
                    if (pictureBox.Location.Y == 300)
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
                Replace(pictureBox); 
                               
            }

            if ((pictureBox.Location.X > 660 && pictureBox.Location.X < 760) && (pictureBox.Location.Y > 260 && pictureBox.Location.Y < 360) && !Game.GetStartGameButton())// בדיקה שהקלף הונח על העירמה שבמרכז השולחן
            {
               
                Card c;
                List<Card> listPlayer = Game.GetCardStackPlayer().GetList();
              
                if (Game.CheckWin())
                    return;

               
                if (listPlayer.IsEmpty()&&isDraggingFromListFinal)//האם הקלף נזרק מהקלפים שעל השולחן
                {
                    List<Card> listPlayerFinal=Game.GetCardStackPlayer().GetListFinal();                   
                    c = Game.GetCardStackPlayer().FindCard(listPlayerFinal, pictureBox); ////הוצאת הקלף מרשימת הקלפים שעל השולחן
                    c.SetIsDraggingFromListFinal(false);
                    Game.GetCardStackPlayer().InsertCardToList(listPlayer, c);//העברת הקלף מהשולחן אל היד כדי להשתמש בו
                    Game.GetCardStackPlayer().ChekCardInList(listPlayerFinal, c);//מחיקת הקלף מרשימת הקלפים שעל השולחן
                  
                  
                  
                    Node<Card> pos = listPlayerFinal.GetFirst();

                    while (pos != null)
                    {
                        //  לא לאפשר לגרור יותר מקלף אחד שעל השולחן
                        pos.GetInfo().SetIsDraggingFromListFinal(false);
                        pos = pos.GetNext();
                    }
                                      
                }


                else
                {
                 
                    MainGame.EnabledUndo(true);
                    c = Game.GetCardStackPlayer().FindCard(listPlayer, pictureBox);//הוצאת הקלף מרשימת הקלפים שביד
                
                }              
               
                     if (Game.IsBigger(Game.GetStack(), c))//בדיקה האם הקלף שנזרק יותר גדול מהקלף שבראש העירמה
                     {

                         pictureBox.Location = new Point(700, 300);
                         pictureBox.BringToFront();
                         Card card;

                         
                        card = Game.GetCardStackPlayer().FindCardAndRemove(listPlayer, pictureBox);                        

                        if (!Game.GetStack().IsEmpty())
                        {
                            if (Game.GetStack().Top().GetNum() == 3)
                            {
                                Game.GetStack().Top().GetPictureBox().Location = new Point(700, 300);
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

                         if (card.GetNum() == 10)
                         {
                             Game.TheNumIs10();                             
                                                          
                         }



                         if ((card.GetNum() != 8) && (card.GetNum() != 10) && !Game.FourCardsHaveTheSameNumber(Game.GetStack()))
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
                         pictureBox.Location = new Point(X, Y);
                                             
                         if (c.isDraggingFromListFinal && !c.BackFinal)
                         {
                             Game.GetCardStackPlayer().GetListFinal().Insert(null, c);
                         }

                     }
                 }
            
            else
            {
                //למנוע רמאות של בחירת קלף אחר מרשימת הקלפים שנמצאים על השולחן 
                if (Game.GetCardStackPlayer().GetLastCards()&&Game.GetCardStackPlayer().GetList().IsEmpty())
                {                   
                  Card  card = Game.GetCardStackPlayer().FindCard(Game.GetCardStackPlayer().GetListFinal(), pictureBox);
                  card.SetIsDraggingFromListFinal(true);                 
                  MessageBox.Show("אתה חייב להשתמש בקלף הזה");
                }
                  pictureBox.Location = new Point(X, Y);
                  
                
            }

            isDragging = false;



        }

        public void Replace(PictureBox picturebox)
        {
            //הפעולה מאפשרת לפני תחילת המשחק להחליף בין הקלפים שנמצאים עם הפנים כלפי מעלה לקלפים שנמצאים ביד של השחקן
            //אחרי החלפת קלפים לבדוק את הלחצנים של לזרוק 2 או 3 קלפים
         
            if ((pictureBox.Location.X > 30 && pictureBox.Location.X < 70) && (pictureBox.Location.Y > 490 && pictureBox.Location.Y < 530))
            {

                Card cardReplaced = Game.GetCardStackPlayer().FindCard(Game.GetCardStackPlayer().GetList(), pictureBox);
                Card caaaaaaaa = Game.GetCardStackPlayer().GetListFinal().GetFirst().GetInfo();
                Card caaa = Game.GetCardStackPlayer().FindCardAcordingToLocation(Game.GetCardStackPlayer().GetListFinal(),Game.GetCardStackPlayer().GetList(), cardReplaced, 50, 510);
                Game.FindSameCard();
                MainGame.ChangeVisible(Game.GetChangeVisible());
                Game.OrganizeList(Game.GetCardStackPlayer(), "player");

                return;

            }


            if ((pictureBox.Location.X > 130 && pictureBox.Location.X < 170) && (pictureBox.Location.Y > 490 && pictureBox.Location.Y < 530))
            {

                Card cardReplaced = Game.GetCardStackPlayer().FindCard(Game.GetCardStackPlayer().GetList(), pictureBox);
                Card caaaaaaaa = Game.GetCardStackPlayer().GetListFinal().GetFirst().GetInfo();
                Card caaa = Game.GetCardStackPlayer().FindCardAcordingToLocation(Game.GetCardStackPlayer().GetListFinal(), Game.GetCardStackPlayer().GetList(), cardReplaced, 150, 510);
                Game.FindSameCard();
                MainGame.ChangeVisible(Game.GetChangeVisible());
                Game.OrganizeList(Game.GetCardStackPlayer(), "player");


                return;

            }

            
            if ((pictureBox.Location.X > 230 && pictureBox.Location.X < 270) && (pictureBox.Location.Y > 490 && pictureBox.Location.Y < 530))
            {

                Card cardReplaced = Game.GetCardStackPlayer().FindCard(Game.GetCardStackPlayer().GetList(), pictureBox);
                Card caaaaaaaa = Game.GetCardStackPlayer().GetListFinal().GetFirst().GetInfo();
                Card caaa = Game.GetCardStackPlayer().FindCardAcordingToLocation(Game.GetCardStackPlayer().GetListFinal(), Game.GetCardStackPlayer().GetList(), cardReplaced, 250, 510);
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
                    if (!(PB.Equals(pos.GetInfo().GetPictureBox())))
                    {
                        pos.GetInfo().SetIsDraggingFromListFinal(false);
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
                    pos.GetInfo().SetIsDraggingFromListFinal(true);
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
                    Game.GetCardStackPlayer().FindCardAndRemove(Game.GetListMemory() , pos.GetInfo().GetPictureBox());
                    pos = pos.GetNext();
                }

            }      


        }

    }

   
}
