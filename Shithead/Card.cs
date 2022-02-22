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
        private static readonly string IMAGES_FOLDER_PATH = Path.Combine(Environment.CurrentDirectory, "Images");
        private int x; //ערך האיקס של מיקום הקלף
        private int y;//ערך הווי של מיקום הקלף
        private int num;//ערך הקלף
        public Shape shape;
        static Main_Game mainGame; //תכונה סטטית לצורך שימוש בפעולות המחלקה
        private bool isDraggingFromListFinal = false;// האם אפשר לגרור את הקלף מהקלפים שעל השולחן
        private PictureBox pictureBox;// תמונה של הקלף
        static Game game; //תכונה סטטית לצורך שימוש בפעולות המחלקה
        private bool isDragging = false; //האם הקלף ניתן לגרירה
        private int oldMouseX;// מיקום איקס קודם של הקלף
        private int oldMouseY;//מיקום ויי קודם של הקלף        
        
        private Stack<Undo> lastTurn=new Stack<Undo>(); //מחסנית לשמירת כל המצבים שבהם היה הקלף
        private bool backFinal; // האם הקלף עם הפנים כלפי מטה


        public void SetBackFinal(bool boolian) // מעדכנת האם הקלף הפוך
        {
            this.backFinal = boolian;
        }
        public bool GetBackFinal()//מחזירה האם הקלף הפוך
        {
            return this.backFinal;
        }

        public void SetX(int num)//מעדכנת ערך האיקס של מיקום הקלף
        {
            this.x = num;
        }

        public void SetY(int num)//מעדכנת ערך הווי של מיקום הקלף
        {
            this.y = num;
        }

        public int GetX()//מחזירה ערך האיקס שח מיקום הקלף
        {
            return this.x;
        }

        public int GetY()//מחזירה ערך הווי של מיקום הקלף
        {
            return this.y;
        }

        public static void SetGame(Game game1)//פעולה סטטית המאפשרת להשתמש בפעולות המחלקה
        {
            game = game1;
        }

        public static void SetMainGame(Main_Game mainGame1)//פעולה סטטית המאפשרת להשתמש בפעולות הפורם
        {
            mainGame = mainGame1;
        }

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
                    if (pictureBox.Location.X > 450 || ((isDraggingFromListFinal)&&(!backFinal)))
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
            

           
            if (game.GetStartGameButton())
            {
                Replace(pictureBox); 
                               
            }

            if ((pictureBox.Location.X > 660 && pictureBox.Location.X < 760) && (pictureBox.Location.Y > 260 && pictureBox.Location.Y < 360) && !game.GetStartGameButton())// בדיקה שהקלף הונח על העירמה שבמרכז השולחן
            {
               
                Card c;
                List<Card> listPlayer = game.GetCardStackPlayer().GetList();
              
                if (game.CheckWin())
                    return;

               
                if (listPlayer.IsEmpty()&&isDraggingFromListFinal)//האם הקלף נזרק מהקלפים שעל השולחן
                {
                    List<Card> listPlayerFinal=game.GetCardStackPlayer().GetListFinal();                   
                    c = game.GetCardStackPlayer().FindCard(listPlayerFinal, pictureBox); ////הוצאת הקלף מרשימת הקלפים שעל השולחן
                    c.SetIsDraggingFromListFinal(false);
                    game.GetCardStackPlayer().InsertCardToList(listPlayer, c);//העברת הקלף מהשולחן אל היד כדי להשתמש בו
                    game.GetCardStackPlayer().ChekCardInList(listPlayerFinal, c);//מחיקת הקלף מרשימת הקלפים שעל השולחן
                  
                  
                  
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
                 
                    mainGame.EnabledUndo(true);
                    c = game.GetCardStackPlayer().FindCard(listPlayer, pictureBox);//הוצאת הקלף מרשימת הקלפים שביד
                
                }              
               
                     if (game.IsBigger(game.GetStack(), c))//בדיקה האם הקלף שנזרק יותר גדול מהקלף שבראש העירמה
                     {

                         pictureBox.Location = new Point(700, 300);
                         pictureBox.BringToFront();
                         Card card;

                         
                        card = game.GetCardStackPlayer().FindCardAndRemove(listPlayer, pictureBox);                        

                        if (!game.GetStack().IsEmpty())
                        {
                            if (game.GetStack().Top().GetNum() == 3)
                            {
                                game.GetStack().Top().GetPictureBox().Location = new Point(700, 300);
                            }                           
                        }

                        PushStackLastTurn(Undo.PlayerThrowCard);
                        game.PushStack(card);
                        game.PushStackUndo(card); 



                         game.RemoveCard(game.GetListMemory(), card); //(הוצאת קלף שהמשתמש זרק מרשימת הזיכרון ( אם היא נמצאת שם  

                         bool cardInQueue = true; 
                             while (game.GetCardStackPlayer().LengthList(listPlayer) < 3 && cardInQueue)
                             {
                                 //השלמה לשלושה קלפים ביד ע"י שליפת קלפים מהקופה
                                 if (!game.GetCardStackPlayer().GetQueue().IsEmpty())
                                 {
                                     game.DrawCard(card);
                                   
                                 }
                                 else
                                     cardInQueue = false;                               
                             }                                             

                         if (card.GetNum() == 10)
                         {
                             game.TheNumIs10();                             
                                                          
                         }



                         if ((card.GetNum() != 8) && (card.GetNum() != 10) && !game.FourCardsHaveTheSameNumber(game.GetStack()))
                         {
                             //טיפול בזריקה של יותר מקלף אחד
                             int count = 0;

                             if(!game.GetCardStackPlayer().GetList().IsEmpty())
                             {
                                 count = game.GetCardStackPlayer().HowManyCardsInList(game.GetCardStackPlayer().GetList(), card);
                             }

                             if (count != 0) 
                             {

                                 game.ThrowPlayerSameNumber(game.GetNum(), card);
                                 game.SetNum(1);
                                 game.OrganizeList(game.GetCardStackPlayer(), "player");

                             }
                             if (game.CheckWin())
                             {
                                 return;
                             }

                             ChangeIsDragingBackFinal();
                             game.ChangeTextBox();
                             mainGame.ChangeVisible(game.FindSameCard());
                             game.OrganizeList(game.GetCardStackPlayer(), "player");
                             if (!(game.FourCardsHaveTheSameNumber(game.GetStack())))
                             {
                                 game.ComputerThrowCard();//העברת התור למחשב
                             }                         
                         }                     

                         ChangeIsDragingBackFinal();
                         game.ChangeTextBox();
                         mainGame.ChangeVisible(game.FindSameCard());
                         game.OrganizeList(game.GetCardStackPlayer(), "player");

                     }
                               
                                                                       
                     else
                     {
                         //זריקת קלף שעל פי החוקים לא ניתן לשים מעל הקלפים שנמצאים בערימה
                         MessageBox.Show("מהלך לא חוקי");                         
                         pictureBox.Location = new Point(x, y);
                                             
                         if (c.isDraggingFromListFinal && !c.backFinal)
                         {
                             game.GetCardStackPlayer().GetListFinal().Insert(null, c);
                         }

                     }
                 }
            
            else
            {
                //למנוע רמאות של בחירת קלף אחר מרשימת הקלפים שנמצאים על השולחן 
                if (game.GetCardStackPlayer().GetLastCards()&&game.GetCardStackPlayer().GetList().IsEmpty())
                {                   
                  Card  card = game.GetCardStackPlayer().FindCard(game.GetCardStackPlayer().GetListFinal(), pictureBox);
                  card.SetIsDraggingFromListFinal(true);                 
                  MessageBox.Show("אתה חייב להשתמש בקלף הזה");
                }
                  pictureBox.Location = new Point(x, y);
                  
                
            }

            isDragging = false;



        }

        public void Replace(PictureBox picturebox)
        {
            //הפעולה מאפשרת לפני תחילת המשחק להחליף בין הקלפים שנמצאים עם הפנים כלפי מעלה לקלפים שנמצאים ביד של השחקן
            //אחרי החלפת קלפים לבדוק את הלחצנים של לזרוק 2 או 3 קלפים
         
            if ((pictureBox.Location.X > 30 && pictureBox.Location.X < 70) && (pictureBox.Location.Y > 490 && pictureBox.Location.Y < 530))
            {

                Card cardReplaced = game.GetCardStackPlayer().FindCard(game.GetCardStackPlayer().GetList(), pictureBox);
                Card caaaaaaaa = game.GetCardStackPlayer().GetListFinal().GetFirst().GetInfo();
                Card caaa = game.GetCardStackPlayer().FindCardAcordingToLocation(game.GetCardStackPlayer().GetListFinal(),game.GetCardStackPlayer().GetList(), cardReplaced, 50, 510);
                game.FindSameCard();
                mainGame.ChangeVisible(game.GetChangeVisible());
                game.OrganizeList(game.GetCardStackPlayer(), "player");

                return;

            }


            if ((pictureBox.Location.X > 130 && pictureBox.Location.X < 170) && (pictureBox.Location.Y > 490 && pictureBox.Location.Y < 530))
            {

                Card cardReplaced = game.GetCardStackPlayer().FindCard(game.GetCardStackPlayer().GetList(), pictureBox);
                Card caaaaaaaa = game.GetCardStackPlayer().GetListFinal().GetFirst().GetInfo();
                Card caaa = game.GetCardStackPlayer().FindCardAcordingToLocation(game.GetCardStackPlayer().GetListFinal(), game.GetCardStackPlayer().GetList(), cardReplaced, 150, 510);
                game.FindSameCard();
                mainGame.ChangeVisible(game.GetChangeVisible());
                game.OrganizeList(game.GetCardStackPlayer(), "player");


                return;

            }

            
            if ((pictureBox.Location.X > 230 && pictureBox.Location.X < 270) && (pictureBox.Location.Y > 490 && pictureBox.Location.Y < 530))
            {

                Card cardReplaced = game.GetCardStackPlayer().FindCard(game.GetCardStackPlayer().GetList(), pictureBox);
                Card caaaaaaaa = game.GetCardStackPlayer().GetListFinal().GetFirst().GetInfo();
                Card caaa = game.GetCardStackPlayer().FindCardAcordingToLocation(game.GetCardStackPlayer().GetListFinal(), game.GetCardStackPlayer().GetList(), cardReplaced, 250, 510);
                game.FindSameCard();
                mainGame.ChangeVisible(game.GetChangeVisible());
                game.OrganizeList(game.GetCardStackPlayer(), "player");

                return;

            }  


        }


        public void DragJustOneCard(PictureBox PB)
        { 
            //חסימת האפשרות לגרור יותר מקלף אחד מהרשימה של הקלפים האחרונים           
            List<Card> listPlayerFinal = game.GetCardStackPlayer().GetListFinal();
            Node<Card> pos = listPlayerFinal.GetFirst();
            if (game.GetCardStackPlayer().LengthList(listPlayerFinal) <= 3)
            {

                while (pos != null)
                {
                    if (!(PB.Equals(pos.GetInfo().GetPictureBox())))
                    {
                        pos.GetInfo().SetIsDraggingFromListFinal(false);
                    }
                    pos = pos.GetNext();
                }             
                game.GetCardStackPlayer().SetLastCards(true);
             
            }
       
      }


        public void ChangeIsDragingBackFinal()
        {
            //בסוף התור שינוי כל הקלפים שנמצאים הפוכים על השולחן של השחקן ללא ניתנים לגרירה
            if (game.GetCardStackPlayer().GetList().IsEmpty())
            {
                List<Card> listPlayerFinal = game.GetCardStackPlayer().GetListFinal(); //הרשימה ריקה
                Node<Card> pos = listPlayerFinal.GetFirst();
                int count = game.GetCardStackPlayer().LengthList(listPlayerFinal);


                while (pos != null)
                {
                    pos.GetInfo().SetIsDraggingFromListFinal(true);
                    if (count == 3)
                    {
                        pos.GetInfo().SetBackFinal(false);
                    }
                    else
                    {
                        game.GetListMemory().Insert(null, pos.GetInfo());
                    }
                    pos = pos.GetNext();
                }

            }

            else
            {
                List<Card> listPlayerFinal = game.GetCardStackPlayer().GetListFinal();
                Node<Card> pos = listPlayerFinal.GetFirst();

                while (pos != null)
                {
                    game.GetCardStackPlayer().FindCardAndRemove(game.GetListMemory() , pos.GetInfo().GetPictureBox());
                    pos = pos.GetNext();
                }

            }      


        }

    }

   
}
