using Core.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Core.Test
{
    public class DeckTests
    {
        Deck currentDeck;
        public void CreateDeck(int cardCount)
        {
            currentDeck = new Deck();
            string frontForm = "Front_{0}";
            string backForm = "Back_{0}";
            for (int i = 1; i < cardCount + 1; i++)
            {
                currentDeck.Cards.Add(new Card() { Front = String.Format(frontForm, i.ToString()), Back = String.Format(backForm, i.ToString()) }); 
            }
        }

        [Fact]
        public void GetCurrent()
        {
            //Empty
            CreateDeck(0);
            var currentCard = currentDeck.GetCurrentCard();
            Assert.Null(currentCard);

            //In Order
            CreateDeck(3);
            currentCard = currentDeck.GetCurrentCard();
            Assert.NotNull(currentCard);
            Assert.Equal(0, currentDeck.Cards.IndexOf(currentCard));
            Assert.Equal(1, currentCard.QuizCardNumber);

            currentCard.QuizCardStatusNumber = 1;
            currentCard = currentDeck.GetCurrentCard();
            Assert.NotNull(currentCard);
            Assert.Equal(0, currentDeck.Cards.IndexOf(currentCard));
            Assert.Equal(1, currentCard.QuizCardNumber);

            currentCard.QuizCardStatusNumber = 2;
            currentCard = currentDeck.GetCurrentCard();
            Assert.NotNull(currentCard);
            Assert.Equal(1, currentDeck.Cards.IndexOf(currentCard));
            Assert.Equal(2, currentCard.QuizCardNumber);

            currentCard.QuizCardStatusNumber = 1;
            currentCard = currentDeck.GetCurrentCard();
            Assert.NotNull(currentCard);
            Assert.Equal(1, currentDeck.Cards.IndexOf(currentCard));
            Assert.Equal(2, currentCard.QuizCardNumber);

            currentCard.QuizCardStatusNumber = 2;
            currentCard = currentDeck.GetCurrentCard();
            Assert.NotNull(currentCard);
            Assert.Equal(2, currentDeck.Cards.IndexOf(currentCard));
            Assert.Equal(3, currentCard.QuizCardNumber);

            currentCard.QuizCardStatusNumber = 1;
            currentCard = currentDeck.GetCurrentCard();
            Assert.NotNull(currentCard);
            Assert.Equal(2, currentDeck.Cards.IndexOf(currentCard));
            Assert.Equal(3, currentCard.QuizCardNumber);

            currentCard.QuizCardStatusNumber = 2;
            currentCard = currentDeck.GetCurrentCard();
            Assert.Null(currentCard);


            //Random
            CreateDeck(3);
            currentDeck.IsQuizRandom = true;
            currentCard = currentDeck.GetCurrentCard();
            Assert.NotNull(currentCard);
            Assert.Equal(1, currentCard.QuizCardNumber);

            currentCard.QuizCardStatusNumber = 1;
            currentCard = currentDeck.GetCurrentCard();
            Assert.NotNull(currentCard);
            Assert.Equal(1, currentCard.QuizCardNumber);

            currentCard.QuizCardStatusNumber = 2;
            currentCard = currentDeck.GetCurrentCard();
            Assert.NotNull(currentCard);
            Assert.Equal(2, currentCard.QuizCardNumber);

            currentCard.QuizCardStatusNumber = 1;
            currentCard = currentDeck.GetCurrentCard();
            Assert.NotNull(currentCard);
            Assert.Equal(2, currentCard.QuizCardNumber);

            currentCard.QuizCardStatusNumber = 2;
            currentCard = currentDeck.GetCurrentCard();
            Assert.NotNull(currentCard);
            Assert.Equal(3, currentCard.QuizCardNumber);

            currentCard.QuizCardStatusNumber = 1;
            currentCard = currentDeck.GetCurrentCard();
            Assert.NotNull(currentCard);
            Assert.Equal(3, currentCard.QuizCardNumber);

            currentCard.QuizCardStatusNumber = 2;
            currentCard = currentDeck.GetCurrentCard();
            Assert.Null(currentCard);
        }

        [Fact]
        public void GetCurrent_WithDeletions()
        {
            CreateDeck(3);
            currentDeck.IsQuizRandom = false;
            currentDeck.Cards[0].IsDeleted = true;
            currentDeck.Cards[1].IsDeleted = true;
            currentDeck.Cards[2].IsDeleted = true;
            var currentCard = currentDeck.GetCurrentCard();
            Assert.Null(currentCard);

            CreateDeck(3);
            currentDeck.Cards[1].IsDeleted = true;

            currentCard = currentDeck.GetCurrentCard();
            Assert.NotNull(currentCard);
            Assert.Equal(0, currentDeck.Cards.IndexOf(currentCard));
            Assert.Equal(1, currentCard.QuizCardNumber);

            currentCard.QuizCardStatusNumber = 2;
            currentCard = currentDeck.GetCurrentCard();
            Assert.NotNull(currentCard);
            Assert.Equal(3, currentCard.QuizCardNumber);

            currentCard.QuizCardStatusNumber = 2;
            currentCard = currentDeck.GetCurrentCard();
            Assert.Null(currentCard);
        }

        [Fact]
        public void GetNext()
        {
            CreateDeck(3);
            currentDeck.IsQuizRandom = false;
            for (int i = 0; i < 2; i++)
            {
                if (i == 1)
                {
                    currentDeck.IsQuizRandom = true;
                    CreateDeck(3);
                }

                var currentCard = currentDeck.GetNextCard();
                Assert.NotNull(currentCard);
                Assert.Equal(1, currentCard.QuizCardNumber);

                currentCard = currentDeck.GetNextCard();
                Assert.NotNull(currentCard);
                Assert.Equal(1, currentCard.QuizCardNumber);

                currentCard = currentDeck.GetNextCard();
                Assert.NotNull(currentCard);
                Assert.Equal(2, currentCard.QuizCardNumber);

                currentCard = currentDeck.GetNextCard();
                Assert.NotNull(currentCard);
                Assert.Equal(2, currentCard.QuizCardNumber);

                currentCard = currentDeck.GetNextCard();
                Assert.NotNull(currentCard);
                Assert.Equal(3, currentCard.QuizCardNumber);

                currentCard = currentDeck.GetNextCard();
                Assert.NotNull(currentCard);
                Assert.Equal(3, currentCard.QuizCardNumber);

                currentCard = currentDeck.GetNextCard();
                Assert.Null(currentCard);
            }
        }

        [Fact]
        public void GetPrevious()
        {
            CreateDeck(3);
            var currentCard = currentDeck.GetCurrentCard();
            currentDeck.Cards[0].QuizCardStatusNumber = 2;
            currentDeck.Cards[1].QuizCardStatusNumber = 1;

            currentCard = currentDeck.GetPreviousCard();
            Assert.NotNull(currentCard);
            Assert.Equal(1, currentCard.QuizCardNumber);
            Assert.Equal(1, currentCard.QuizCardStatusNumber);
            Assert.Equal(0, currentDeck.Cards[1].QuizCardStatusNumber);

            CreateDeck(3);
            currentCard = currentDeck.GetCurrentCard();
            currentDeck.Cards[0].IsDeleted = true;
            currentDeck.Cards[0].IsDeleted= true;
            currentCard = currentDeck.GetPreviousCard();

            Assert.Null(currentCard);

            CreateDeck(3);
            currentCard = currentDeck.GetCurrentCard();
            currentDeck.Cards[0].QuizCardStatusNumber = 2;
            currentDeck.Cards[1].QuizCardStatusNumber = 2;
            currentDeck.Cards[2].QuizCardStatusNumber = 2;

            currentCard = currentDeck.GetPreviousCard();
            Assert.NotNull(currentCard);
            Assert.Equal(2, currentCard.QuizCardNumber);
            Assert.Equal(1, currentCard.QuizCardStatusNumber);
        }

        [Fact]
        public void SkipCurrentCard()
        {
            CreateDeck(3);
            var currentCard = currentDeck.GetCurrentCard();
            currentDeck.Cards[0].QuizCardStatusNumber = 2;

            currentCard = currentDeck.SkipCurrentCardAndGetNext();
            Assert.NotNull(currentCard);
            Assert.Equal(1, currentCard.QuizCardStatusNumber);
            Assert.Equal(3, currentCard.QuizCardNumber);
            Assert.Equal(2, currentDeck.Cards[1].QuizCardStatusNumber = 2);

            currentCard = currentDeck.SkipCurrentCardAndGetNext();
            Assert.Null(currentCard);

            CreateDeck(3);
            currentCard = currentDeck.GetCurrentCard();
            currentCard = currentDeck.SkipCurrentCardAndGetNext();
            Assert.NotNull(currentCard);
            Assert.Equal(1, currentCard.QuizCardStatusNumber);
            Assert.Equal(2, currentCard.QuizCardNumber);
        }
    }
}
