using System;
using System.Linq;
using System.Text;
using Unit4.CollectionsLib;
using System.Drawing;
using System.Windows.Forms;
using Shithead.Enums;

namespace Shithead
{
    public class Board
    {
        private Game game;//החזקת כל הקלפים 
        static Main_Game mainGame; //תכונה סטטית לצורך שימוש בפעולות המחלקה


        public Board(Game game)
        {
            //פעולה בונה המקבלת את המחלקה גיים ומעדכנת אותה במחלקה
            this.game = game;
        }

        public Game GetGame()
        {
            //הפעולה מחזירה את המחלקה   
            return this.game;
        }


        public static void SetMainGame(Main_Game mainGame1)
        {
            //פעולה סטטית לצורך שימוש בפעולות הפורם
            mainGame = mainGame1;
        }

        public void StartLocation(Game game)
        {
            //קביעת המיקום ההתחלתי של הקלפים במשחק- מצב התחלתי של הלוח
            game.ChangeVisibleThorwMoreThanOneCardButtons = (1);
            game.NumberOfSameCard = (1);

            int x = 50, y1 = 150, y2 = 500;

            List<Card> ListComputerFinal = game.GetCardStackComputer().TableCards;
            List<Card> ListPlayerFinal = game.GetCardStackPlayer().TableCards;

            List<Card> ListComputer = game.GetCardStackComputer().HandCards;
            List<Card> ListPlayer = game.GetCardStackPlayer().HandCards;



            for (int i = 0; i < 6; i++)
            {
                Card CardComputer = game.GetCardStackComputer().Deck.Remove();
                Card CardPlayer = game.GetCardStackPlayer().Deck.Remove();

                if (i == 3)
                {
                    x = 50;
                    y1 = y1 - 13;
                    y2 = y2 + 10;
                }
                if (i >= 3)
                {
                    CardComputer.SetCard(CardComputer.PictureBox);
                    CardComputer.PictureBox.BringToFront();


                    CardPlayer.SetCard(CardPlayer.PictureBox);
                    CardPlayer.PictureBox.BringToFront();
                }
                else
                {
                    CardComputer.BackFinal = true;
                    CardPlayer.BackFinal = true;
                }
                CardComputer.PictureBox.Location = new Point(x, y1);
                CardComputer.X = x;
                CardComputer.Y = y1;
                game.GetCardStackComputer().InsertCardToList(ListComputerFinal, CardComputer);

                CardPlayer.PictureBox.Location = new Point(x, y2);
                CardPlayer.X = x;
                CardPlayer.Y = y2;
                game.GetCardStackPlayer().InsertCardToList(ListPlayerFinal, CardPlayer);

                x = x + 100;
            }



            x = 500;

            for (int i = 0; i < 3; i++)
            {
                Card cardComputer = game.GetCardStackComputer().Deck.Remove();

                Card cardPlayer = game.GetCardStackPlayer().Deck.Remove();

                cardComputer.PictureBox.Location = new Point(x, 50);
                if (game.ShowComputerCards == ShowComputerCards.Yes)
                {
                    cardComputer.SetCard(cardComputer.PictureBox);
                }

                cardComputer.X = x;
                cardComputer.Y = 50;
                game.GetCardStackComputer().InsertCardToList(ListComputer, cardComputer);


                cardPlayer.PictureBox.Location = new Point(x, 600);
                cardPlayer.SetCard(cardPlayer.PictureBox);
                cardPlayer.X = x;
                cardPlayer.Y = 600;
                game.GetCardStackPlayer().InsertCardToList(ListPlayer, cardPlayer);

                game.FindSameCard();
                mainGame.ChangeVisible(game.ChangeVisibleThorwMoreThanOneCardButtons);

                x = x + 100;

            }

            game.ChangeTextBox();
            game.OrganizeList(game.GetCardStackPlayer(), "player");
            game.OrganizeList(game.GetCardStackComputer(), "computer");


        }
    }
}

       

    

