﻿using System;
using System.Linq;
using System.Text;
using Unit4.CollectionsLib;
using System.Drawing;
using System.Windows.Forms;
using Shithead.Enums;

namespace Shithead
{
    public class Game 
    {

        private Card_Stack cardStackPlayer;//כל הקלפים שיש ברשות השחקן
        private Card_Stack cardStackComputer;//כל הקלפים שיש ברשות המשתמש
        private Stack<Card> stack = new Stack<Card>();//ערימת הקלפים שנמצאת במרכז השולחן 
        static Main_Game mainGame;//תכונה סטטית לצורך שימוש בפעולות הפורם 
        static End end;//תכונה סטטית לצורך שימוש בפעולות הפורם 

        public Level Level { get; set; }

       
        public ShowComputerCards ShowComputerCards { get; set; }



        public int ChangeVisibleThorwMoreThanOneCardButtons { get; set; }
        public int NumberOfSameCard { get; set; }
        public  bool StartGameButton { get; set; } = true;//בדיקת לחיצת כפתור התחל משחק
        private bool checkWin = false;//בדיקת ניצחון
        

        public int PointsComputer { get; set; } = 0;
        public int PointsPlayer { get; set; } = 0;

        private List<Card> memory = new List<Card>();//רשימת קלפים המכילה את הקלפים שהמחשב יודע שיש לשחקן ביד

        private Stack<Card> undo = new Stack<Card>();//מחסנית המכילה את סדר שימושם של הקלפים במשחק לצורך פעולת החזרה


        public Card_Stack GetCardStackPlayer()
        {
            //מחזירה את קלפי השחקן
            return this.cardStackPlayer;
        }

        public Card_Stack GetCardStackComputer()
        {
            //מחזירה את קלפי המחשב
            return this.cardStackComputer;
        }


        public static void SetMainGame(Main_Game mainGame1)
        {
            //פעולה סטטית לשימוש פעולות הפורם
            mainGame = mainGame1;
        }
        public static void SetEnd(End end1)
        {
            //פעולה סטטית לשימוש פעולות הפורם
            end = end1;
        }

        public Stack<Card> GetStack()
        {
            //מחיזרה את ערימת הקלפים
            return this.stack;
        }

        public void PushStack(Card c)
        {
            //מכניסה קלף לערימה
            this.stack.Push(c);
        }
        public void PushStackUndo(Card c)
        {
            // מכניסה קלף למחסנית המכילה את סדר שימושם של הקלפים במשחק לצורך פעולת החזרה 
            this.undo.Push(c);
        }


        public Stack<Card> GetStackUndo()
        {
            //מחזירה את מחסנית החזרה
            return this.undo;
        }

        public List<Card> GetListMemory()
        {
            //מחזירה את רשימת הזיכרון
            return this.memory;
        }

        public void SetListMemory(List<Card> Memory)
        {
            //מעדכנת את רשימת הזיכרון
            this.memory = Memory;
        }

        public Card[] shuffle()
        {
            //הפעולה מערבבת את 52 קלפי המשחק
            Card[] a1 = new Card[52];
            Card c;
            Random rnd = new Random();
            int i, x, j, b = 52, k = 0;
            for (i = 2; i < 15; i++)
            {
                for (Shape shape = Shape.D; shape <= Shape.C; shape++)
                {
                    a1[k] = new Card(i, shape);
                    k++;
                }

            }
            Card[] cp = new Card[52];
            for (j = 0; j < 52; j++)
            {

                x = rnd.Next(0, b);
                cp[j] = a1[x];
                c = a1[b - 1];
                a1[b - 1] = a1[x];
                a1[x] = c;
                b = b - 1;
            }
            return cp;

        }

        public Game(Card_Stack CardStackPlayer, Card_Stack CardStackComputer, Main_Game f)
        {
            //פעולה בונה המחלקת את קלפי המשחק בין השחקן למחשב 
            this.cardStackPlayer = CardStackPlayer;
            this.cardStackComputer = CardStackComputer;

            Card[] a1 = new Card[52];
            Card[] a = shuffle();

            for (int i = 0; i < 26; i++)
            {

                a[i].PictureBox.Location = new Point(350, 600);
                CardStackPlayer.Deck.Insert(a[i]);
                f.Controls.Add(a[i].PictureBox);

                a[i + 26].PictureBox.Location = new Point(350, 50);

                CardStackComputer.Deck.Insert(a[i + 26]);
                f.Controls.Add(a[i + 26].PictureBox);


            }

            PictureBox logo = new PictureBox();            
            f.Controls.Add(logo);
            logo.Image = Image.FromFile("Images/logo.PNG");          
            logo.Size = new Size(178, 73);
            logo.Location = new Point(5,5);
            logo.BringToFront();

            PictureBox specialCards = new PictureBox();
            f.Controls.Add(specialCards);
            specialCards.Image = Image.FromFile("Images/specialCards.PNG");
            specialCards.Size = new Size(400, 216);
            specialCards.Location = new Point(900, 250);
            specialCards.BringToFront();
          
        }


        public bool IsBigger(Stack<Card> stack, Card card)
        {
            //הפעולה מקבלת את ערימת הקלפים וקלף ובודקת האם אפשר להניח את הקלף בעירמה 
            if (stack.IsEmpty())
                return true;

            if ((card.NumValue == 10) || (card.NumValue == 2) || (card.NumValue == 3))
            {
                return true;
            }

            if (stack.Top().NumValue == 3)
                return TheNumIs3(card);


            if (card.NumValue >= stack.Top().NumValue && stack.Top().NumValue != 7)
            {
                return true;
            }

            if (stack.Top().NumValue == 7 && card.NumValue <= stack.Top().NumValue)
            {
                return true;
            }

            return false;

        }

        public void DrawCard(Card card)
        {
            //הפעולה שולפת קלף מהקופה בהתאם לקלף שהיא מקבלת 
            if (card.Y == 600)
            {
                List<Card> listPlayer;
                if (GetCardStackPlayer().HandCards.IsEmpty())
                {
                    listPlayer = GetCardStackPlayer().TableCards;
                }
                else
                {

                    listPlayer = GetCardStackPlayer().HandCards;
                }


                if (!this.cardStackPlayer.Deck.IsEmpty())
                {
                    Card cardPlayer = this.cardStackPlayer.Deck.Remove();

                    int x = card.X;
                    int y = card.Y;

                    cardPlayer.PictureBox.Location = new Point(x, y);
                    cardPlayer.SetCard(cardPlayer.PictureBox);
                    cardPlayer.X = x;
                    cardPlayer.Y = y;

                    this.cardStackPlayer.InsertCardToList(listPlayer, cardPlayer);                   
                    cardPlayer.PushStackLastTurn(Undo.DrawCard);
                    PushStackUndo(cardPlayer);


                    mainGame.ChangeVisible(FindSameCard());

                }

            }
            else
            {
                List<Card> listComputer = this.cardStackComputer.HandCards;
                if (!this.cardStackComputer.Deck.IsEmpty())
                {
                    Card cardComputer = this.cardStackComputer.Deck.Remove();

                    int x = card.X;
                    int y = card.Y;

                    cardComputer.PictureBox.Location = new Point(x, y);                
                    cardComputer.X = x;
                    cardComputer.Y = y;

                    this.cardStackPlayer.InsertCardToList(listComputer, cardComputer);
                    cardComputer.PushStackLastTurn(Undo.DrawCard);               
                    PushStackUndo(cardComputer);
                }
            }


        }

        public void Drag(Card card)
        {
            //הפעולה מקבלת קלף וגוררת אותו אל ערימת הקלפים
            bool booldrag = true;
            PictureBox picturebox = card.PictureBox;
            int hefreshx = 700 - card.X;
            int hefreshy = 300 - card.Y;
            hefreshx = hefreshx / 20;
            hefreshy = hefreshy / 20;

            picturebox.Location = new Point(card.X, card.Y);

            while (((picturebox.Location.X != 700 || picturebox.Location.Y != 300) && booldrag))
            {
                System.Threading.Thread.Sleep(18);
                picturebox.Location = new Point(picturebox.Location.X + hefreshx, picturebox.Location.Y + hefreshy);
                picturebox.BringToFront();
                if ((picturebox.Location.X > 650 && picturebox.Location.X < 770) && (picturebox.Location.Y > 250 && picturebox.Location.Y < 370))
                {
                    picturebox.Location = new Point(700, 300);
                    picturebox.BringToFront();
                    booldrag = false;
                }
            }

            if (card.NumValue == 3)
            {
                card.PictureBox.Location = new Point(713, 300);
                if (!stack.IsEmpty())
                {
                    if (stack.Top().NumValue == 3)
                    {
                        stack.Top().PictureBox.Location = new Point(710, 300);
                        card.PictureBox.Location = new Point(720, 300);
                        if (!stack.IsEmpty())
                        {
                            Card c = stack.Pop();
                            if (!stack.IsEmpty())
                            {
                                if (stack.Top().NumValue == 3)
                                {
                                    stack.Top().PictureBox.Location = new Point(710, 300);
                                    c.PictureBox.Location = new Point(720, 300);
                                    card.PictureBox.Location = new Point(730, 300);

                                }
                                PushStack(c);
                            }
                        }
                    }

                }
            }
            card.SetCard(picturebox);
            PushStack(card);
        }

        public void ComputerThrowCard()
        {
            //פעולה ראשית לתור המחשב
            //הפעולה זורקת קלף של המחשב אם מתאפשר אחרת המחשב לוקח את כל הקלפים שנמצאים בערימה
            Node<Card> pos;
            List<Card> listComputer = cardStackComputer.HandCards;
          
            if (CheckWin())
            {
                return;
            }

            if (cardStackComputer.LengthList(cardStackComputer.HandCards) != 0)
            {
                pos = listComputer.GetFirst();
            }
            else
            {
                listComputer = cardStackComputer.TableCards;

                pos = listComputer.GetFirst();

                if (cardStackComputer.LengthList(cardStackComputer.TableCards) == 3)
                {
                    while (pos != null)
                    {
                        pos.GetInfo().BackFinal = false;
                        pos = pos.GetNext();
                    }
                }

                if (cardStackComputer.LengthList(cardStackComputer.TableCards) <= 3 && cardStackComputer.HandCards.IsEmpty())
                {
                    Card c = cardStackComputer.ChekCardInList(cardStackComputer.TableCards, cardStackComputer.TableCards.GetFirst().GetInfo());                 
                    cardStackComputer.HandCards.Insert(null, c);
                    listComputer = cardStackComputer.HandCards;
                }

                pos = listComputer.GetFirst();
            }

            bool boolian = false, checkInMemory = false, extraTurn = false, bestCard = false, minMaxCard=false;

            Card card = null;

            if (Level == Level.Hard)
            {
                this.memory=cardStackComputer.CopyList(GetCardStackPlayer().HandCards);              
            }


            if ((Level == Level.Normal) || (Level == Level.Hard))
            {

                if (!GetListMemory().IsEmpty() && GetCardStackComputer().LengthList(listComputer) >= 2)
                {
                    if (GetCardStackComputer().HandCards.IsEmpty())
                    {
                        card = GetCardStackComputer().MinMaxCard(listComputer, memory, stack);

                        if (card != null)
                        {
                            minMaxCard = true;
                            if (card.Equals(GetCardStackComputer().MaxCard(listComputer)) && GetCardStackComputer().ThreeInList(listComputer) == null)
                            {
                                bestCard = true;
                            }
                        }
                        
                        checkInMemory = true;
                    }
                }
            }


            while (!boolian)
            {
                if (!checkInMemory || card == null)
                {
                    card = MinCardIsBigger(listComputer, stack);
                    if (card == null)
                    {
                        card = ThrowSpecialCard(listComputer);

                        if (card == null)
                        {
                            boolian = true;
                            string str = "computer";
                            TakeCards(str);
                        }
                    }
                }


                if (card != null)
                {
                    checkInMemory = false;
                  
                    PictureBox picturebox = card.PictureBox;
                
                    if (!card.BackFinal)
                    {
                        card.PushStackLastTurn(Undo.ComputerThrowCard);
                      
                        PushStackUndo(card);
                        Drag(card);
                    
                        GetCardStackComputer().RemoveCard(listComputer, card);
                        if ((card.NumValue == 10))
                        {
                            card.SetCard(card.PictureBox);
                            card.PictureBox.BringToFront();
                            mainGame.Refresh();
                            TheNumIs10();
                        }

                        boolian = true;


                        if ((card.NumValue == 8) 
                            || (card.NumValue == 10) 
                            || FourCardsHaveTheSameNumber(this.stack))
                        {
                            extraTurn = true;
                        }

                        else
                        {
                          
                            if ((Level == Level.Normal) || (Level == Level.Hard))
                            {
                                if (!cardStackComputer.HandCards.IsEmpty() && !IsSpecial(card) && (!minMaxCard || bestCard))
                                {

                                    Card card1 = cardStackComputer.ChekSameCard(listComputer, card);

                                    while (card1 != null)
                                    {
                                        System.Threading.Thread.Sleep(50);
                                        mainGame.Refresh();
                                     
                                        card1.PushStackLastTurn(Undo.ComputerThrowCard);
                                        
                                        PushStackUndo(card1);
                                        Drag(card1);
                                        card1 = cardStackComputer.ChekSameCard(listComputer, card);

                                    }

                                }
                            }
                            if (FourCardsHaveTheSameNumber(this.stack))
                            {
                                extraTurn = true;
                            }
                        }
                        bool cardInQueue = true;
                        while (GetCardStackComputer().LengthList(GetCardStackComputer().HandCards) < 3 && cardInQueue)
                        {
                            if (!cardStackComputer.Deck.IsEmpty())
                            {
                                DrawCard(card);
                            }
                            else
                                cardInQueue = false;
                        }

                    }
                    pos = pos.GetNext();


                    if (pos == null && !boolian)
                    {
                   
                        boolian = true;
                        string str = "computer";
                        TakeCards(str);
                    }

                }
            }
            if (CheckWin())
            {
                return;
            }

            if (extraTurn == true)
            {
                card.SetCard(card.PictureBox);
                card.PictureBox.BringToFront();
                mainGame.Refresh();
                System.Threading.Thread.Sleep(200);
                mainGame.Refresh();
                ComputerThrowCard();
            }

            if (!undo.IsEmpty())
            {
                mainGame.EnabledUndo(true);
            }

            if (GetCardStackPlayer().LengthList(GetCardStackPlayer().TableCards)<6 || GetCardStackComputer().LengthList(GetCardStackComputer().TableCards)<6)
            {                
                mainGame.EnabledUndo(false);
                mainGame.GetUndoButton().Visible = false;
            }
            if (stack.IsEmpty())
            {
                mainGame.GetTakeCardsButton().Enabled = false;
            }
            else
            {
                mainGame.GetTakeCardsButton().Enabled= true;
            }
            ChangeTextBox();
            OrganizeList(GetCardStackComputer(), "computer");
        }


        public void TakeCards(string name)
        {
            //הפעולה לוקחת את כל הקלפים שנמצאים בערימת הקלפים אל היד של אחד המשתמשים בהתאם לשם שהיא מקבלת
            bool boolian = true;

            if (name == "player")
            {

                while (!stack.IsEmpty())
                {

                    Card card = stack.Pop();

                  

                    cardStackPlayer.InsertCardToList(cardStackPlayer.HandCards, card);

                    InsertCardToList(memory, card);        
                    PushStackUndo(card);

                    card.PushStackLastTurn(Undo.PlayerTakeCard);                
                    mainGame.ChangeVisible(FindSameCard());


                    if (!GetCardStackPlayer().HandCards.IsEmpty())
                    {
                        List<Card> listPlayerFinal = GetCardStackPlayer().TableCards;
                        Node<Card> pos = listPlayerFinal.GetFirst();


                        while (pos != null)
                        {
                            pos.GetInfo().IsDraggingFromListFinal = false;
                            pos = pos.GetNext();
                        }
                    }
                }
                OrganizeList(cardStackPlayer, "player");

            }    

            if (name == "computer")
            {
                int x = 800, y = 50;
                while (!stack.IsEmpty())
                {
                    if (cardStackComputer.LengthList(cardStackComputer.HandCards) >= 8 && boolian)
                    {
                        x = 500;
                        y = 180;
                        boolian = false;
                    }

                    Card card = stack.Pop();
                    card.X = x;
                    card.Y = y;
                    cardStackComputer.InsertCardToList(cardStackComputer.HandCards, card);              
                    PushStackUndo(card);                  
                    card.PushStackLastTurn(Undo.ComputerTakeCard);
           


                    PictureBox PB = card.PictureBox;
                    PB.Location = new Point(x, y);
                    x = x + 100;

                }
            }

        }


        public void InsertCardToList(List<Card> list, Card c)
        {
            //הפעולה מקבלת רשימה וקלף ומכניסה את הקלף לרשימה
            Node<Card> pos = list.GetFirst();
            while (pos != null)
            {
                pos = pos.GetNext();
            }
            list.Insert(pos, c);

        }

        public void RemoveCard(List<Card> list, Card card)
        {
            //הפעולה מקבלת רשימה וקלף ומוחקת את הקלף מהרשימה
            Node<Card> pos = list.GetFirst();
            while (pos != null)
            {
                if (pos.GetInfo().PictureBox.Equals(card.PictureBox))
                {
                    list.Remove(pos);
                }
                pos = pos.GetNext();
            }

        }

        public void OrganizeList(Card_Stack cardStack, string str)
        {
            //הפעולה מסדרת את מיקומי הקלפים על הלוח מהקטן לגדול  

            int count = 1;
            int length = cardStack.LengthList(cardStack.HandCards);
            List<Card> list;

            if (str == "player")
            {              
                list = InsertionSort(cardStack);
                cardStack.HandCards = list;
                Node<Card> pos = list.GetFirst();

                int x = 500;
                int y = 600;
                while (pos != null)
                {

                    if (x >= 1200)
                    {
                        x = 500;
                        y = 450;

                    }

                    Card card = pos.GetInfo();
                    card.PictureBox.Location = new Point(x, y);
                    card.X = x;
                    card.Y = y;
                    card.SetCard(card.PictureBox);

                    if (pos.GetNext() != null)
                    {
                        if (card.NumValue == pos.GetNext().GetInfo().NumValue)
                        {
                            x = x + 15;
                            pos.GetNext().GetInfo().PictureBox.BringToFront();

                        }
                        else
                            x = x + 100;
                    }

                    pos = pos.GetNext();
                    count++;
                }

            }
            if (str == "computer")
            {                
                if (ShowComputerCards == ShowComputerCards.Yes)
                {
                    list = InsertionSort(cardStack);
                }

                else
                {
                    list = cardStackComputer.HandCards;
                }
                cardStack.HandCards = list;
                Node<Card> pos = list.GetFirst();

                int x = 500;
                int y = 50;

                while (pos != null)
                {

                    if (x >= 1200)
                    {
                        x = 500;
                        y = 180;

                    }

                    Card card = pos.GetInfo();
                    if (ShowComputerCards == ShowComputerCards.No)
                    {
                        card.SetBackCard();
                    }
                    if (ShowComputerCards == ShowComputerCards.Yes)
                    {
                        card.SetCard(card.PictureBox);
                    }
                    card.PictureBox.Location = new Point(x, y);
                    card.X = x;
                    card.Y = y;

                    if (pos.GetNext() != null)
                    {
                        if (ShowComputerCards == ShowComputerCards.Yes)
                        {
                            if (card.NumValue == pos.GetNext().GetInfo().NumValue)
                            {
                                x = x + 15;
                                pos.GetNext().GetInfo().PictureBox.BringToFront();
                            }
                            else
                                x = x + 100;
                        }
                        else
                        {
                            x = x + 70;
                        }
                    }                 
                    pos = pos.GetNext();
                    count++;
                }


            }
        }


        public void InsertinToSortedList(List<Card> lst, Card card)
        {
            //הפעולה מקבלת רשימה וקלף ומכניסה את הקלף לרשימה בצורה ממוינת
            bool inList = false;
            Node<Card> prev = null;
            Node<Card> pos = lst.GetFirst();
            while (pos != null && !inList)
            {
                if (card.NumValue < pos.GetInfo().NumValue)
                    inList = true;
                else
                {
                    prev = pos;
                    pos = pos.GetNext();
                }
            }
            lst.Insert(prev, card);
        }

        
        public List<Card> InsertionSort(Card_Stack cardStack)
        {
            //הפעולה מחזירה רשימה ממוינת
            List<Card> tmpList = new List<Card>();
            Node<Card> pos = cardStack.HandCards.GetFirst();
            while (pos != null)
            {
                InsertinToSortedList(tmpList, pos.GetInfo());
                pos = pos.GetNext();
            }
            return tmpList;
        }

        public void TheNumIs10()
        {
           //הפעולה מתבצעת כאשר הקלף 10 נזרק לערימת הקלפים. כאשר נזרק 10 ערימת הקלפים מתרוקנת
            System.Threading.Thread.Sleep(200);
            while (!stack.IsEmpty())
            {
                Card card = stack.Pop();
           
                    card.PushStackLastTurn(Undo.ThrowTen);
             

                PushStackUndo(card);

                card.PictureBox.Visible = false;

                card.PictureBox.Location = new Point(1000, 300);
            }
        }


        public bool TheNumIs3(Card cardAfter3)
        {
           //(הפעולה בודקת אם הקלף שנזרק מתאים להיות מעל הקלף 3 שנמצא כעת בראש ערימת הקלפים (בדיקה האם הקלף שנזרק מתאים לקלף שנמצא לפני 3
            Card card = stack.Pop();

            if (stack.IsEmpty())
            {
                stack.Push(card);
                return true;
            }

            if (stack.Top().NumValue == 3) 
            {
                bool boolian = TheNumIs3(cardAfter3);
                if (boolian)
                {
                    stack.Top().PictureBox.Location = new Point(700, 300);
                }
                stack.Push(card);
                return boolian;
            }

            if (stack.Top().NumValue != 7)
            {
                if (cardAfter3.NumValue >= stack.Top().NumValue)
                {
                    stack.Push(card);
                    return true;
                }                                   
            }
            else
            {
                if (cardAfter3.NumValue <= stack.Top().NumValue)
                {
                    stack.Push(card);
                    return true;
                }
            }
            stack.Push(card);
            return false;

        }


        public void ThrowPlayerSameNumber(int count, Card card)
        {
            //הפעולה מקבלת קלף וזורקת קלפים עם אותו המספר שנמצאים ביד השחקן בהתאם למספר שהפעולה קיבלה  
            List<Card> listPlayer = cardStackPlayer.HandCards;

            Card SameNum;
            for (int i = 0; i < count - 1; i++)
            {
              

                SameNum = cardStackPlayer.ChekSameCard(listPlayer, card);

                if (SameNum != null)
                {
                  

                    SameNum.PushStackLastTurn(Undo.PlayerThrowCard);

                    PushStackUndo(SameNum);
                    Drag(SameNum);
                }
            }


            bool cardInQueue = true;
            while (GetCardStackPlayer().LengthList(listPlayer) < 3 && cardInQueue)
            {
                if (!GetCardStackPlayer().Deck.IsEmpty())
                {
                    DrawCard(card);
                }
                else
                    cardInQueue = false;
            }

        }

        public bool FourCardsHaveTheSameNumber(Stack<Card> stack)
        {
            //בדיקה האם ארבעת הקלפים העליונים בערימת הקלפים הם בעלי אותו מספר אם כן הערימה תתרוקן וקלפיה יושלחו לזבל
            Stack<Card> copy = CopyStack(stack);
            Stack<Card> temp = new Stack<Card>();

            int count = 0;
            while (!copy.IsEmpty() && count != 4)
            {
                temp.Push(copy.Pop());
                count++;
            }
            if (count != 4)
                return false;

            copy.Push(temp.Pop());
            while (!temp.IsEmpty())
            {
                if (copy.Top().NumValue != temp.Top().NumValue)
                    return false;
                copy.Push(temp.Pop());
            }

            stack.Top().SetCard(stack.Top().PictureBox);
            stack.Top().PictureBox.BringToFront();
            mainGame.Refresh();           
            TheNumIs10();
            return true;
        }

        public Stack<Card> CopyStack(Stack<Card> stack)
        {
            //הפעולה מקבלת מחסנית ומחזירה ההעתק שלה
            Stack<Card> newS = new Stack<Card>();
            Stack<Card> temp = new Stack<Card>();
            while (!stack.IsEmpty())
                temp.Push(stack.Pop());
            while (!temp.IsEmpty())
            {
                newS.Push(temp.Top());
                stack.Push(temp.Pop());
            }
            return newS;
        }

        public void UndoFunc()
        {
            //פעולה ראשית שאחראית על כפתור החזרה
            //הפעולה מחזירה את מהלך המשחק למצב שבו המשתמש לא ביצע את התור האחרון שלו
            Card card = undo.Pop();


            if (!stack.IsEmpty())
            {
              if (stack.Top().NumValue == 8) // שחקן זרק שמונה
                {                  
                    if (stack.Top().LastTurn.Top() == Undo.PlayerThrowCard)
                    {
                        card = UndoPlayerDrawCard(card);
                        UndoPlayerThrowCard(card);
                        return;
                    }
                }
            }  
            
            if (card.LastTurn.Top() == Undo.ComputerTakeCard) // (מחשב (6
            {
               card=UndoTakeCards(card);
               if (card == null)
                   return;
               card = UndoPlayerDrawCard(card);
            }

            if (card.LastTurn.Top() == Undo.ThrowTen)
            {
                card = UndoThrowTen(card);
                if (card == null)
                    return;
                Card ca = card;
                card = UndoComputerThrowCard(card);
                if (card.PictureBox.Equals(ca.PictureBox))
                {
                    card = UndoPlayerDrawCard(card);
                }
            }
            card = UndoComputerDrawCard(card);
            card = UndoComputerThrowCard(card);        

           if (!stack.IsEmpty())
            {
                bool boolian = true;
                while (boolian && stack.Top().NumValue == 8)
                {                  
                    if (stack.Top().LastTurn.Top() == Undo.ComputerThrowCard)
                    {
                        card = UndoComputerDrawCard(card);
                        card = UndoComputerThrowCard(card);
                    }
                    if (stack.IsEmpty())
                        boolian = false;
                }
            }  


            if (card.LastTurn.Top() == Undo.PlayerTakeCard) //שחקן
            {
                UndoTakeCards(card);              
                    return;
            }


            card = UndoPlayerDrawCard(card);
            UndoPlayerThrowCard(card);

            if (undo.IsEmpty())
            {
                mainGame.EnabledUndo(false);
            }
            else
            {
                if (undo.Top().LastTurn.Top() == Undo.ThrowTen)
                {
                    UndoFunc();
                }
            }

        }

        public Card UndoTakeCards(Card card)
        {
            //ביצוע פעולת החזור במקרה שנלחץ קח קלפים 
            while (card.LastTurn.Top() == Undo.ComputerTakeCard)
            {
             PushStack(card);
             GetCardStackComputer().FindCardAndRemove(GetCardStackComputer().HandCards, card.PictureBox);
             
             card.PictureBox.Location = new Point(700, 300);
             card.LastTurn.Pop();
             
             if (!undo.IsEmpty())
             {
                 card = undo.Pop();
             }
             else
             {
                 return card;
             }                             

            }

            if (card.LastTurn.Top() == Undo.PlayerTakeCard)
            {
                while (card.LastTurn.Top() == Undo.PlayerTakeCard)
                {
                    PushStack(card);

                    GetCardStackPlayer().FindCardAndRemove(GetCardStackPlayer().HandCards, card.PictureBox);
                    GetCardStackPlayer().FindCardAndRemove(memory, card.PictureBox);
                    card.PictureBox.Location = new Point(700, 300);
                    card.LastTurn.Pop();


                    if (!undo.IsEmpty())
                    {
                        card = undo.Pop();
                    }
                    else
                    {

                        return null;
                    }
             
                }
                PushStackUndo(card);
            }
           
            OrganizeList(GetCardStackPlayer(), "player");
            OrganizeList(GetCardStackPlayer(), "Computer");
            return card;
        }


        public Card UndoThrowTen(Card card)
        {
            //ביצוע פעולת החזור במקרה שנזרק הקלף 10
            while (card.LastTurn.Top() == Undo.ThrowTen)
            {
                PushStack(card);
                card.PictureBox.Location = new Point(700, 300);
                card.PictureBox.Visible = true;
                card.LastTurn.Pop();

                if (!undo.IsEmpty())
                {
                    card = undo.Pop();
                }
                else
                {
                    return null;
                }              

            }
            return card;
        }

        public Card UndoComputerDrawCard(Card card)
        {
            //ביצוע פעולת החזור במקרה שהמחשב שלף קלף
            while (card.LastTurn.Top() == Undo.DrawCard) //קלף שהמחשב שלף
            {
                cardStackComputer.AddTOTopQueue(GetCardStackComputer().Deck, card);              
                card.PictureBox.Location = new Point(350, 50);
                card.SetBackCard();               
                Card c = GetCardStackComputer().FindCard(GetCardStackComputer().HandCards, card.PictureBox);
                if (c != null)
                {
                    GetCardStackComputer().FindCardAndRemove(GetCardStackComputer().HandCards, card.PictureBox);
                }
                else
                {
                    GetCardStackPlayer().FindCardAndRemove(GetCardStackPlayer().HandCards, card.PictureBox);
                }

                card.LastTurn.Pop();
                card = undo.Pop();
            }
            ChangeTextBox();
            return card;
        }

        public Card UndoPlayerDrawCard(Card card)
        {
            //ביצוע פעולת החזור במקרה שהשחקן שלף קלף
            while (card.LastTurn.Top() == Undo.DrawCard) //קלף שהשחקן שלף
            {
                cardStackPlayer.AddTOTopQueue(GetCardStackPlayer().Deck, card);
                card.PictureBox.Location = new Point(350, 600);
                card.SetBackCard();              
                Card c = GetCardStackComputer().FindCard(GetCardStackComputer().HandCards, card.PictureBox);
                if (c != null)
                {
                    GetCardStackComputer().FindCardAndRemove(GetCardStackComputer().HandCards, card.PictureBox);
                }
                else
                {
                    GetCardStackPlayer().FindCardAndRemove(GetCardStackPlayer().HandCards, card.PictureBox);
                }

                if (Level == Level.Hard)
                {
                    GetCardStackPlayer().FindCardAndRemove(memory, card.PictureBox);
                }

                card.LastTurn.Pop();
                card = undo.Pop();
            }
            ChangeTextBox();
            return card;

        }

        public Card UndoComputerThrowCard(Card card)
        {
            //ביצוע פעולת החזור במקרה שהמחשב זרק קלף לערימת הקלפים
            while (card.LastTurn.Top() == Undo.ComputerThrowCard)//קלף שהמחשב זרק
            {
                if (!stack.IsEmpty())
                {
                    stack.Pop();
                }
                
                GetCardStackComputer().InsertCardToList(GetCardStackComputer().HandCards, card);
               
                card.LastTurn.Pop();
                card = undo.Pop();
            }
            return card;
        }

        public void UndoPlayerThrowCard(Card card)
        {
            //ביצוע פעולת החזור במקרה שהשחקן זרק קלף לערימת הקלפים
            while (card.LastTurn.Top() == Undo.PlayerThrowCard)//קלף שהמחשב זרק
            {
                if (!stack.IsEmpty())
                {
                    stack.Pop();
                    InsertCardToList(memory, card);
                }

                GetCardStackPlayer().InsertCardToList(GetCardStackPlayer().HandCards, card);
                card.LastTurn.Pop();

                GetCardStackPlayer().InsertCardToList(memory, card);


                if (!undo.IsEmpty())
                {
                    card = undo.Pop();
                }
                else
                {

                    return;
                }            
            }
            undo.Push(card);
            
        }

        public int FindSameCard()
        {
            //הפעולה בודקת את המספר הכי שכיח בקרב הקלפים שביד השחקן ומחזירה את מספר הפעמים שמספר זה מופיע ביד השחקן    
            List<Card> listPlayer = this.cardStackPlayer.HandCards;
            Node<Card> pos = listPlayer.GetFirst();
            int max = 1;
            int count = 1;
            while (pos != null)
            {
                Card card = pos.GetInfo();
                if (card.NumValue != 8)
                {
                    count = GetCardStackPlayer().HowManyCardsInList(listPlayer, card);
                }
                if (count > max)
                {
                    max = count;
                }
                pos = pos.GetNext();
            }
            ChangeVisibleThorwMoreThanOneCardButtons = (max);
            return this.ChangeVisibleThorwMoreThanOneCardButtons;
        }


        public bool IsSpecial(Card card)
        {
            //הפעולה מקבלת קלף ובודקת האם הוא קלף מיוחד
            if ((card.NumValue == 10) || (card.NumValue == 2) 
                || (card.NumValue == 3) || (card.NumValue == 7))
            {
                return true;
            }
            return false;
        }

        public void Throw10(List<Card> list, Card card)
        {
            //(הפעולה מקבלת רשימה וקלף ומבצעת את חוקי המשחק כאשר נזרק 10 לערימת הקלפים (הערימה מתרוקנת והשחקן שזרק את הקלף מקבל תור נוסף  
            Drag(card);
            GetCardStackComputer().RemoveCard(list, card);
            card.SetCard(card.PictureBox);
            card.PictureBox.BringToFront();
            mainGame.Refresh();           
            TheNumIs10();
            ComputerThrowCard();

        }

        public Card MinCardIsBigger(List<Card> listComputer, Stack<Card> stack)
        {
            //הפעולה מקבלת רשימת קלפי מחשב וערימת הקלפים ומחזירה את הקלף הנמוך ביותר שאפשר לזרוק לערימה
            List<Card> copy = GetCardStackComputer().CopyList(listComputer);

            Node<Card> pos = copy.GetFirst();

            Card minCard;

            while (pos != null)
            {
                minCard = GetCardStackComputer().MinCard(copy);

                if (minCard != null)
                {
                    if (IsBigger(stack, minCard) && !IsSpecial(minCard) && !minCard.BackFinal)
                    {
                        return minCard;
                    }
                    RemoveCard(copy, minCard);
                }
                else
                {
                    return null;
                }
            }


            return null;
        }


        public Card ThrowSpecialCard(List<Card> listComputer)
        {
            //null הפעולה מקבלת רשימת קלפים של המחשב ומחזירה את אחד הקלפים המיוחדים בהתאם ל"חוזק" שלהם אם לא נמצא קלף מיוחד הפעולה תחזיר    
            Card seven = cardStackComputer.SevenInList(listComputer);
            if (seven != null && IsBigger(stack, seven))
            {
                return seven;
            }

            Card two = cardStackComputer.TwoInList(listComputer);
            if (two != null)
            {
                return two;
            }

            Card three = cardStackComputer.ThreeInList(listComputer);
            if (three != null)
            {
                return three;
            }

            Card ten = cardStackComputer.TenInList(listComputer);
            if (ten != null)
            {
                return ten;
            }

            return null;
        }

        public bool CheckWin()
        {
            //בדיקה האם השחקן או המחשב ניצחו
            if (this.checkWin == false)
            {
                if (cardStackComputer.TableCards.IsEmpty() && cardStackComputer.HandCards.IsEmpty())
                {
                    PointsComputer = 2;
                   

                    if (cardStackPlayer.Deck.IsEmpty())
                    {
                        PointsPlayer = (1);
                    }

                    mainGame.End(PointsComputer, PointsPlayer);
                    this.checkWin = true;                    
                    return true;                  
                }
                if (cardStackPlayer.TableCards.IsEmpty() && cardStackPlayer.HandCards.IsEmpty())
                {
                    PointsPlayer = (2);
                    if (cardStackComputer.Deck.IsEmpty())
                    {
                        PointsComputer=1;
                    }

                    mainGame.End(PointsComputer, PointsPlayer);
                    this.checkWin = true;
                    return true;
                }
            }

            return false;
        }

        public int LengthStack(Stack<Card> stack)
        {
            //הפעולה מקבלת מחסנית ומחזירה את אורכה
            Stack<Card> copyStack = CopyStack(stack);
            int count = 0;
            while (!copyStack.IsEmpty())
            {
                copyStack.Pop();
                count++;
            }
            return count;
        }

        public void ComputerReplaceCards()
        {
            //הפעולה מאפשרת למחשב לפני תחילת המשחק להחליף בין הקלפים שנמצאים עם הפנים כלפי מעלה לקלפים שנמצאים בידו 
            List<Card> ListComputerFinal = GetCardStackComputer().TableCards;
            List<Card> ListComputer = GetCardStackComputer().HandCards;
            Node<Card> pos = ListComputer.GetFirst();
            Node<Card> posFinal = ListComputerFinal.GetFirst();
            int count = 0;

            while (count != 3)
            {
                while (pos != null)
                {
                    if (IsBetter(posFinal.GetInfo(),pos.GetInfo()))
                    {
                        GetCardStackComputer().FindCardAcordingToLocation(ListComputerFinal, ListComputer, pos.GetInfo(), posFinal.GetInfo().X, posFinal.GetInfo().Y);
                        pos = ListComputer.GetFirst();
                        posFinal = ListComputerFinal.GetFirst();
                        
                    }
                    else
                    {
                        pos = pos.GetNext();
                    }
                }
                count++;
                posFinal = posFinal.GetNext();
                pos = ListComputer.GetFirst();

            }

            posFinal = ListComputerFinal.GetFirst();
            while (posFinal != null)
            {
              Card c = posFinal.GetInfo();
              posFinal= posFinal.GetNext();
            }

            return;
        }

        public bool IsBetter(Card card, Card cardReplaced)
        {
            //הפעולה מקבלת קלף וקלף מוחלף פוטנציאלי ובודקת האם למחשב כדי להחליף בין הקלפים
            if (cardReplaced.NumValue == 10 && card.NumValue != 10)
                return true;
            if (cardReplaced.NumValue == 3 && card.NumValue != 10 && card.NumValue != 3)
                return true;
            if (cardReplaced.NumValue == 2 && card.NumValue != 10 
                && card.NumValue != 3 && card.NumValue != 2)
                return true;         
            if (cardReplaced.NumValue > card.NumValue && card.NumValue != 10 &&
                card.NumValue != 2 && card.NumValue != 3)
                return true;
            return false;
        }


        public void ChangeTextBox()
        {
            //הפעולה מעדכנת את מספר הקלפים שנשארו בקופה של השחקןוהמחשב
            int num1 = GetCardStackComputer().LengthQueue(GetCardStackComputer().Deck);
            int num2 = GetCardStackPlayer().LengthQueue(GetCardStackPlayer().Deck);

            if (num1 == 0)
                mainGame.GetTextBox1().Visible = false;
            else
                mainGame.GetTextBox1().Visible = true;
            if (num2 == 0) 
                mainGame.GetTextBox2().Visible = false;
            
            else
                mainGame.GetTextBox2().Visible = true;
            
            string countQueueComputer = num1.ToString();
            string countQueuePlayer = num2.ToString();

            mainGame.GetTextBox1().Text = countQueueComputer;
            mainGame.GetTextBox2().Text = countQueuePlayer;
        }

        
 
    }

}

  


    

          
    


