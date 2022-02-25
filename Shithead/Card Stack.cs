using System;
using System.Linq;
using System.Text;
using Unit4.CollectionsLib;
using System.Drawing;
using System.Windows.Forms;

namespace Shithead
{
    public class Card_Stack
    {

        private Queue<Card> queue = new Queue<Card>(); // הקופה שמכילה את הקלפים של כל שחקן לאחר חלוקתם
        private List<Card> list = new List<Card>();// הקלפים שביד השחקן
        private List<Card> listFinal = new List<Card>();// הקלפים שנמצאים על השולחן
        

        private bool lastCards = false;// טיפול ב"רמאות" של שחקן- אם הוא מחזיר קלף עם הפנים כלפי מטה שהוא כבר לקח 

        static Game game; //תכונה סטטית לצורך שימוש בפעולות המחלקה 

        public static void SetGame(Game game1)
        {
            //פעולה סטטית לשימוש בפעולות המחלקה
            game = game1;
        }

        public Queue<Card> GetQueue()
        {
            //מחזירה את הקופה
            return this.queue;
        }

        public List<Card> GetList()
        {
            //מחזירה את הקלפים שביד
            return this.list;
        }

        public void SetList(List<Card> list)
        {
            //הפעולה מעדכנת רשימה
            this.list = list;
        }


        public List<Card> GetListFinal() 
        {
            //מחזירה את הקלפים
            return this.listFinal;
        }

        public void SetLastCards(bool boolian) 
        {
            //עדכון האם השחקן השתשמש בקלף עם הפנים כלפי מטה 
            this.lastCards = boolian;
        }
        public bool GetLastCards()
        {
            //מחזירה האם השחקן השתמש בקלף בקלף עם הפנים כלפי מטה
            return this.lastCards;
        }


        public Card FindCardAndRemove(List<Card> list, PictureBox picturebox)
        {
            //הפעולה מקבלת תמונה ורשימה ובודקת האם התמונה נמצאת ברשימה אם כן הפעולה מחזירה את הקלף בעל אותה תמונה ומוחקת אותו מהרשימה  
            Node<Card> pos = list.GetFirst();
            while (pos != null)
            {
                if (pos.GetInfo().GetPictureBox().Equals(picturebox))
                {
                    Card c = pos.GetInfo();
                    if (list != null)
                        list.Remove(pos);

                    return c;
                }
                pos = pos.GetNext();
            }
            return null;

        }

        public Card FindCard(List<Card> list, PictureBox picturebox)
        {
            //הפעולה מקבלת תמונה ורשימה ובודקת האם התמונה נמצאת ברשימה אם כן הפעולה מחזירה את הקלף בעל אותה תמונה 
            Node<Card> pos = list.GetFirst();
            while (pos != null)
            {
                Card c0 = pos.GetInfo();
                if (pos.GetInfo().GetPictureBox().Equals(picturebox))
                {
                    Card c = pos.GetInfo();
                    return c;
                }
                pos = pos.GetNext();
            }
            return null;

        }



        public void InsertCardToList(List<Card> list, Card c) 
        {
           //הפעולה מקבלת רשימת קלפים וקלף ומכניסה את הקלף לרשימה
            Node<Card> pos = list.GetFirst();
            while (pos != null)
            {
                pos = pos.GetNext();
            }
            list.Insert(pos, c);

        }

        public void RemoveCard(List<Card> list, Card card)
        {
            //(הפעולה מקבלת רשימת קלף וקלף ומוציאה את הקלף מהרשימה (אם הוא נמצא שם 
            Node<Card> pos = list.GetFirst();
            while (pos != null)
            {
                if (pos.GetInfo().GetPictureBox().Equals(card.GetPictureBox()))
                {
                    list.Remove(pos);

                }
                pos = pos.GetNext();
            }

        }

        public void AddTOTopQueue(Queue<Card> queue, Card card)
        {
            //הפעולה מקבלת תור קלפים וקלף ומכניסה את הקלף לראש התור
            queue.Insert(card);
            while (!queue.Head().Equals(card))
            {
                queue.Insert(queue.Remove());
            }
        }

        public int LengthQueue(Queue<Card> queue)
        {
            //הפעולה מקבלת תור ומחזירה את אורכו
            if (!queue.IsEmpty())
            {
                int count = 1;
                Card card = queue.Head();
                queue.Insert(queue.Remove());
                while (!queue.Head().Equals(card))
                {
                    queue.Insert(queue.Remove());
                    count++;
                }
                return count;
            }
            return 0;
        }
           
        public int LengthList(List<Card> list)
        {
            //הפעולה מקבלת רשימה ומחזירה את אורכה
           int length = 0;
            Node<Card> pos = list.GetFirst();
            while (pos != null)
            {
                length++;
                pos = pos.GetNext();
            }
            return length;
        }

        public bool CardInHand(List<Card> list, PictureBox picturebox)
        {
            //הפעולה מקבלת רשימה ותמונה ובודקת האם התמונה נמצאת באחד מהקלפים ברשימה
            Node<Card> pos = list.GetFirst();
            while (pos != null)
            {
                if (pos.GetInfo().GetPictureBox().Equals(picturebox))
                    return true;
                pos = pos.GetNext();
            }
            return false;

        }

        public Card ChekCardInList(List<Card> list, Card card)
        {
            //הפעולה מקבלת רשימה וקלף ובודקת האם התמונה של אותו קלף נמצאת ברשימה אם כן הפעולה מוחקת את אותו הקלף מהרשימה 
            Node<Card> pos = list.GetFirst();
            while (pos != null)
            {
                if (card.GetPictureBox().Equals(pos.GetInfo().GetPictureBox()))
                {
                    Card SameCard = pos.GetInfo();
                    list.Remove(pos);
                    return SameCard;
                }
                pos = pos.GetNext();
            }

            return null;
        }


        public Card ChekSameCard(List<Card> list, Card card)
        {
            //הפעולה מקבלת רשימה וקלף ובודקת האם הקלף נמצא ברשימה אם כן הפעולה מוחקת את הקלף 
            Node<Card> pos = list.GetFirst();
            while (pos != null)
            {
                if (card.GetNum() == pos.GetInfo().GetNum())
                {
                    Card SameCard = pos.GetInfo();
                    list.Remove(pos);
                    return SameCard;
                }
                pos = pos.GetNext();
            }

            return null;
        }


        public int HowManyCardsInList(List<Card> list, Card card)
        {
           //הפעולה מקבלת רשימה וקלף ומחזירה כמה פעמים קלף זה נמצא ברשימה
            int count = 0;
            Node<Card> pos = list.GetFirst();
            while (pos != null)
            {
                if (card.GetNum() == pos.GetInfo().GetNum())
                {
                    count++;
                }

                pos = pos.GetNext();

            }
            return count;

        }


        public bool IsCardInList(List<Card> list, Card card)
        {
            //הפעולה מקבלת רשימת קלפים וקלף ובודקת האם הקלף נמצא ברשימה אם כן הפעולה תמחוק את הקלף
            Card c;
            c = ChekCardInList(list, card);
            if (c == null)
                return false;
            return true;


        }

        public Card MinCard(List<Card> list)
        {
            //(הפעולה מקבלת רשימת קלפים ומחזירה את הקלף בעל המספר הכי נמוך (לא כולל קלפים מיוחדים  
            Node<Card> pos = list.GetFirst();
            Card minCard = null;
            int minNum = 15;

            while (pos != null)
            {
                if (minNum > pos.GetInfo().GetNum() 
                    && !IsSpecial(pos.GetInfo()) 
                    && !pos.GetInfo().BackFinal)
                {
                    minCard = pos.GetInfo();
                    minNum = pos.GetInfo().GetNum();
                }

                pos = pos.GetNext();
            }
            if (minCard != null)
            {
                return minCard;
            }
            return null;
        }

        public Card MaxCard(List<Card> list)
        {
            //(הפעולה מקבלת רשימת קלפים ומחזירה את הקלף בעל המספר הכי גבוה (לא כולל קלפים מיוחדים 
            Node<Card> pos = list.GetFirst();
            Card maxCard = null;
            int maxNum = 0;


            while (pos != null)
            {
                if (maxNum < pos.GetInfo().GetNum() 
                    && !IsSpecial(pos.GetInfo()) 
                    && !pos.GetInfo().BackFinal)
                {
                    maxCard = pos.GetInfo();
                    maxNum = pos.GetInfo().GetNum();
                }

                pos = pos.GetNext();

            }
            if (maxCard != null)
            {
                return maxCard;
            }
            return null;
        }


        public List<Card> CopyList(List<Card> list)
        {
            //הפעולה מקבלת רשימת קלפים ומחזירה רשימה חדשה שאיבריה זהים לאיבריה של הרשימה המקורית 
            List<Card> newList = new List<Card>();
            Node<Card> pos1 = list.GetFirst();

            while (pos1 != null)
            {
                InsertCardToList(newList, pos1.GetInfo());
                pos1 = pos1.GetNext();

            }
            return newList;
        }


        public Card MinMaxCard(List<Card> listComputer, List<Card> memory, Stack<Card> stack)
        {
            //פעולה ראשית של הבינה מלאכותית 
 
            //הפעולה מקבלת רשימת קלפי מחשב רשימת זיכרון והמחסנית שבמרכז השולחן ומחזירה את הקלף המינימלי שגדול יותר מהקלף הגבוה ביותר ברשימת הזיכרון 
            //אם הפעולה לא תמצא קלף כזה היא תנסה למצוא קלף מיוחד שעומד בתנאים של פעולות העזר
            List<Card> copy = CopyList(listComputer);

            Node<Card> pos = copy.GetFirst();

            Card maxCardListMemory = MaxCard(memory);

            Card minCard = MinCard(copy);

            Card special = Special(listComputer, memory, stack);
            if (special != null)
            {
                if (special.GetNum() == 7)
                {
                    return special;
                }
            }
            

            if (minCard != null && maxCardListMemory != null)
            {
                if (stack.IsEmpty())
                {
                    if (minCard.GetNum() > maxCardListMemory.GetNum() 
                        && !minCard.BackFinal)
                    {
                        return minCard;
                    }
                   
                    while (minCard != null)
                    {
                        minCard = MinCard(copy);
                        if (minCard != null)
                        {
                            if (minCard.GetNum() > maxCardListMemory.GetNum()
                                && !minCard.BackFinal)
                            {
                                return minCard;
                            }
                            RemoveCard(copy, minCard);
                        }

                    }

                }

                else
                {
                    if (minCard.GetNum() > maxCardListMemory.GetNum() 
                        && minCard.GetNum() >= stack.Top().GetNum() 
                        && !minCard.BackFinal)
                    {
                        return minCard;
                    }
                    while (minCard != null)
                    {
                        minCard = MinCard(copy);
                        if (minCard != null)
                        {
                            if (minCard.GetNum() > maxCardListMemory.GetNum() 
                                && minCard.GetNum() >= stack.Top().GetNum() 
                                && !minCard.BackFinal)
                            {
                                return minCard;
                            }
                            RemoveCard(copy, minCard);
                        }
                    }
                }

                special = Special(listComputer, memory, stack);
                if (special != null)
                {
                    return special;
                }
            }
                return null;

            

           
        }

        public bool IsSpecial(Card card)
        {
            //הפעולה מקבל קלף ובודקת האם הקלף הוא קלף מיוחד כלומר אחד מהקלפים 2,10,3,7
            if ((card.GetNum() == 10) || (card.GetNum() == 2) || (card.GetNum() == 3) || (card.GetNum() == 7))
            {
                return true;
            }
            return false;
        }

        public Card Special(List<Card> listComputer, List<Card> memory, Stack<Card> stack)
        {
            // הפעולה מקבלת את רשימת קלפי המחשב, רשימת הזיכרון ומחסנית ערימת הקלפים ומחזירה קלף מיוחד

            Card seven = Throw7(listComputer, memory, stack);
            if (seven != null)
            {
                return seven;
            }

            Card two = Throw2(listComputer, memory, stack);
            if (two != null)
            {
                return two;
            }

            Card three = Throw3(listComputer, memory, stack);
            if (three != null)
            {
                return three;
            }


            return null;
        }

        public Card Throw7(List<Card> listComputer, List<Card> memory, Stack<Card> stack)
        {
            //הפעולה מקבלת רשימת קלפי מחשב, רשימת הזיכרון וערימת הקלפים וזורקת את הקלף 7 מהרשימה במידה והוא מקיים את התנאים

            Card maxCardListMemory = MaxCard(memory);
            Card seven = SevenInList(listComputer);
            int count1 = HowManyCardsUnder7(memory);
            int count2 = HowMany7InList(listComputer);

            if (!stack.IsEmpty() && maxCardListMemory != null && seven != null)
            {
                if ((maxCardListMemory.GetNum() < 7 || count2 > count1) && stack.Top().GetNum() <= 7)
                {
                    if (stack.Top().GetNum() == 3)
                    {
                        bool boolian = game.TheNumIs3(seven);
                        stack.Pop();
                        if (boolian)
                            return seven;
                        else
                            return null;
                    }

                    return seven;
                }
            }
            else
            {
                if (maxCardListMemory != null)
                {
                    if (maxCardListMemory.GetNum() < 7)
                    {
                        return seven;
                    }
                }
            }
          

            return null;
        }
        public Card SevenInList(List<Card> listComputer)
        {
            // null הפעולה מקבלת רשימת קלפים ובודקת אם קלף שערכו 7 נמצא ברשימה, אם כן הפעולה מחזירה את הקלף אחרת הפעולה מחזירה 
            Node<Card> pos = listComputer.GetFirst();
            while (pos != null)
            {
                if (pos.GetInfo().GetNum() == 7 && !pos.GetInfo().BackFinal)
                {
                    return pos.GetInfo();
                }
                pos = pos.GetNext();
            }
            return null;
        }
        public Card Throw3(List<Card> listComputer, List<Card> memory, Stack<Card> stack)
        {
            //הפעולה מקבלת רשימת קלפי מחשב, רשימת הזיכרון וערימת הקלפים וזורקת את הקלף 3 מהרשימה במידה והוא מקיים את התנאי

            Card maxCardListMemory = MaxCard(memory);
            Card three = ThreeInList(listComputer);
            if (!stack.IsEmpty())
            {
                if (maxCardListMemory.GetNum() < stack.Top().GetNum() && three != null)
                {
                    return three;
                }
            }
            return null;
        }
        public Card ThreeInList(List<Card> listComputer)
        {
            // null הפעולה מקבלת רשימת קלפים ובודקת אם קלף שערכו 3 נמצא ברשימה, אם כן הפעולה מחזירה את הקלף אחרת הפעולה מחזירה 
            Node<Card> pos = listComputer.GetFirst();
            while (pos != null)
            {
                if (pos.GetInfo().GetNum() == 3 && !pos.GetInfo().BackFinal)
                {
                    return pos.GetInfo();
                }
                pos = pos.GetNext();
            }
            return null;
        }


        public Card TenInList(List<Card> listComputer)
        {
            // null הפעולה מקבלת רשימת קלפים ובודקת אם קלף שערכו 10 נמצא ברשימה, אם כן הפעולה מחזירה את הקלף אחרת הפעולה מחזירה 
            Node<Card> pos = listComputer.GetFirst();
            while (pos != null)
            {
                if (pos.GetInfo().GetNum() == 10 && !pos.GetInfo().BackFinal)
                {
                    return pos.GetInfo();
                }
                pos = pos.GetNext();
            }
            return null;
        }

        public Card Throw2(List<Card> listComputer, List<Card> memory, Stack<Card> stack)
        {
            //הפעולה מקבלת רשימת קלפי מחשב, רשימת הזיכרון וערימת הקלפים וזורקת את הקלף 2 מהרשימה במידה והוא מקיים את התנאי

            Card maxCardListMemory = MaxCard(memory);
            Card two = TwoInList(listComputer);
            if (!stack.IsEmpty())
            {
                if (maxCardListMemory.GetNum() < stack.Top().GetNum() && two != null)
                {
                    return two;
                }
            }
            return null;
        }
        public Card TwoInList(List<Card> listComputer)
        {
            // null הפעולה מקבלת רשימת קלפים ובודקת אם קלף שערכו 2 נמצא ברשימה, אם כן הפעולה מחזירה את הקלף אחרת הפעולה מחזירה 
            Node<Card> pos = listComputer.GetFirst();
            while (pos != null)
            {
                if (pos.GetInfo().GetNum() == 2 && !pos.GetInfo().BackFinal)
                {
                    return pos.GetInfo();
                }
                pos = pos.GetNext();
            }
            return null;
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

        public Stack<Card> CopyStack(Stack<Card> stack)
        {
            //הפעולה מקבלת מחסנית ומחזירה העתק שלה
            Stack<Card> tmp1 = new Stack<Card>();
            Stack<Card> tmp2 = new Stack<Card>();
            while (!stack.IsEmpty())
            {
                tmp2.Push(stack.Pop());
            }
            while (!tmp2.IsEmpty())
            {
                tmp1.Push(tmp2.Top());
                stack.Push(tmp2.Pop());
            }
            return stack;
        }
        public Card FindCardAcordingToLocation(List<Card> listFinal, List<Card> list, Card cardReplaced, int x, int y)
        {
            // הפעולה מקבלת את רשימת הקלפים שנמצאת על השולחן, רשימת הקלפים שנמצאת ביד, הקלף שאותו רוצים להחליף והמיקום של הקלף המוחלף. הפעולה מחליפה בין הקלף המחליף והמוחלף 
            Node<Card> pos = listFinal.GetFirst();

            if (pos.GetInfo().X == x && pos.GetInfo().Y == y)
            {
                Card card = pos.GetInfo();
                card.X = cardReplaced.X;
                card.Y =cardReplaced.Y;
                cardReplaced.X = x;
                cardReplaced.Y = y;
                card.GetPictureBox().Location = new Point(card.X, card.Y);
                cardReplaced.GetPictureBox().Location = new Point(x, y);
                cardReplaced.GetPictureBox().BringToFront();
                cardReplaced.SetCard(cardReplaced.GetPictureBox());
                InsertCardToList(list, card);
                RemoveCard(list, cardReplaced);
                listFinal.Insert(pos, cardReplaced);
                listFinal.Remove(pos);
                return card;
            }           

            Node<Card> posNext = pos.GetNext();
            while (posNext != null)
            {
                Card ca = posNext.GetInfo();
                if (posNext.GetInfo().X == x && posNext.GetInfo().Y == y)
                {
                    Card card = posNext.GetInfo();
                    card.X = cardReplaced.X;
                    card.Y = cardReplaced.Y;
                    cardReplaced.X=x;
                    cardReplaced.Y =y;                    

                   card.GetPictureBox().Location = new Point(card.X, card.Y);
                   cardReplaced.GetPictureBox().Location = new Point(x, y);
                   cardReplaced.GetPictureBox().BringToFront();
                   cardReplaced.SetCard(cardReplaced.GetPictureBox());
                   InsertCardToList(list, card);                  
                   RemoveCard(list, cardReplaced);
                   listFinal.Insert(pos, cardReplaced);
                   listFinal.Remove(posNext);
                   return card;
                }
                pos = pos.GetNext();
                posNext = posNext.GetNext();
            }

            return null;
        }

        public int HowManyCardsUnder7(List<Card> listMemory)
        {
            //הפעולה מקבלת את רשימת הזיכרון ומחזירה כמה קלפים ברשימה קטנים או שווים ל7             
            int count=0;
            Node<Card> pos = listMemory.GetFirst();
            while (pos != null)
            {
                if (pos.GetInfo().GetNum() <= 7)
                {
                    count++;
                }
                pos = pos.GetNext();
            }
            return count;
            
        }

        public int HowMany7InList(List<Card> listComputer)
        {
            //הפעולה מקבלת רשימה ומחזירה כמה את מספר הקלפים ששווים ל7 ברשימה
            int count = 0;
            Node<Card> pos = listComputer.GetFirst();
            while (pos != null)
            {
                if (pos.GetInfo().GetNum() == 7)
                {
                    count++;
                }
                pos = pos.GetNext();
            }
            return count;

        }

        

       
    }
}
  


