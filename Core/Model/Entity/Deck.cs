using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Model.Entity
{
    public class Deck
    {
        public string UserID { get; set; }
        public string DeckName { get; set; }
        public bool IsQuizRandom { get; set; }
        public bool IsQuizOver { get; private set; }
        public bool IsQuizStarted { get; private set; }

        private List<Card> _Cards;
        public List<Card> Cards
        {
            get
            {
                if (_Cards == null)
                {
                    _Cards = new List<Card>();
                }
                return _Cards;
            }
            set
            {
                _Cards = value;
            }
        }

        /// <summary>
        /// Resets all card counters to zero.
        /// </summary>
        public void ResetDeck()
        {
            IsQuizOver = false;
            IsQuizStarted = false;
            Cards.ForEach(card => {
                card.QuizCardStatusNumber = 0;
                card.QuizCardNumber = 0;
                card.IsDeleted = false;
            });
        }

        /// <summary>
        /// Gets the current Card based on the card counters and IsDeleted status
        /// </summary>
        /// <returns>The current card. Returns null if the deck doesn't have any cards or there are no available cards</returns>
        public Card GetCurrentCard()
        {
            if (!Cards.Any())
                return null;
            int maxCardNumber = GetMaxQuizCardNumber();
            if (maxCardNumber == 0)
            {
                AssignQuizValues();
            }
            var available = GetAVailableQuizCards().OrderBy(cardNumber => cardNumber.QuizCardNumber);
            if (!available.Any())
            {
                IsQuizOver = true;
                return null;
            }
            return available.First();
        }

        /// <summary>
        /// Increments the Passed in cards status
        /// </summary>
        /// <param name="currentCard">The card to update</param>
        public void UpdateCurrentCardStatus(Card currentCard)
        {
            if (currentCard != null)
                if (currentCard.QuizCardStatusNumber < 2)
                    currentCard.QuizCardStatusNumber += 1;
        }

        /// <summary>
        /// Updates the CurrentCardStatus of the CurrentCard and retrieves the next card
        /// </summary>
        /// <returns></returns>
        public Card GetNextCard()
        {
            var currentCard = GetCurrentCard();
            UpdateCurrentCardStatus(currentCard);
            if (currentCard != null && currentCard.QuizCardStatusNumber == 2)
                return currentCard;
            return GetCurrentCard();
        }

        public Card GetPreviousCard()
        {
            var currentCard = GetCurrentCard();
            if (currentCard != null)
            {
                currentCard.QuizCardStatusNumber = 0;
            }
            else if(Cards.Any())
            {
                var avail = GetNonDeletedCards().OrderByDescending(cardNumber => cardNumber.QuizCardNumber);
                if(avail.Any())
                {
                    var last = avail.First();
                    last.QuizCardStatusNumber = 0;
                }
            }
            var available = GetNonDeletedCards().Where(card => card.QuizCardStatusNumber == 2).OrderByDescending(cardNumber => cardNumber.QuizCardNumber);
            if (available.Any())
            {
                var previousCard = available.First();
                previousCard.QuizCardStatusNumber = 1;
                return previousCard;
            }
            return null;
        }

        public Card SkipCurrentCardAndGetNext()
        {
            var currentCard = GetCurrentCard();
            if (currentCard != null)
            {
                currentCard.QuizCardStatusNumber = 2;
            }
            var nextCard = GetCurrentCard();
            if (nextCard != null)
                nextCard.QuizCardStatusNumber = 1;
            return nextCard;
        }

        /// <summary>
        /// The cards not marked for deletion
        /// </summary>
        public List<Card> GetNonDeletedCards()
        {
            return Cards.Where(card => !card.IsDeleted).ToList();
        }

        public List<Card> GetDeletedCards()
        {
            return Cards.Where(card => card.IsDeleted).ToList();
        }

        /// <summary>
        /// Called to assign values when a new quiz begins
        /// </summary>
        private void AssignQuizValues()
        {
            IsQuizStarted = true;
            if (!IsQuizRandom)
            {
                int currentCardNumber = 0;
                Cards.ForEach(card =>
                {
                    card.QuizCardNumber = currentCardNumber + 1;
                    currentCardNumber++;
                });
            }
            else
            {
                List<int> allIndexes = (from i in Enumerable.Range(1, Cards.Count) select i).ToList();
                Random r = new Random();
                int cardCount = Cards.Count;
                Cards.ForEach(card =>
                {
                    int random = r.Next(0, allIndexes.Count);
                    card.QuizCardNumber = allIndexes[random];
                    allIndexes.RemoveAt(random);
                });
            }
        }

        private int GetMaxQuizCardNumber()
        {
            return Cards.Select(cardNumber => cardNumber.QuizCardNumber).Max();
        }

        private List<Card> GetAVailableQuizCards()
        {
            return GetNonDeletedCards().Where(card => card.QuizCardStatusNumber < 2 ).ToList();
        }
    }

    public class Card
    {
        public string Front { get; set; }
        public string Back { get; set; }
        public int QuizCardStatusNumber { get; set; }
        public int QuizCardNumber { get; set; }

        public enum QuizCardStatus
        {
            NeverShown = 0,
            front = 1,
            back = 2,
            Invalid = 3
        }

        public QuizCardStatus GetCardStatus()
        {
            return ((QuizCardStatus)QuizCardStatusNumber);
        }

        public string GetCardDataBasedOnStatus()
        {
            if (QuizCardStatusNumber == 1)
                return Front;
            if (QuizCardStatusNumber == 2)
                return Back;
            return String.Empty;
        }

        private bool _IsDeleted = false;
        public bool IsDeleted
        {
            get
            {
                return _IsDeleted;
            }
            set
            {
                _IsDeleted = value;
            }
        }
    }
}
