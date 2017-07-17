using ChatBotHook.DAL;
using Core;
using Core.Model;
using Core.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatBotHook.IntentHandlers
{
    public class QuizIntentHandler : BaseHandler<QuizSlotType>
    {

        public QuizIntentHandler() { }

        public QuizIntentHandler(IDatabaseDAL dal) : base(dal) { }

        protected override OutputModel<QuizSlotType> ProcessIntent()
        {
            string fulfillmentState = null;
            string dialogActionType = Constants.DIALOG_ACTION_TYPE_ELICIT;
            string intentName = InputModel.CurrentIntent.Name;
            var slots = InputModel.CurrentIntent.Slots;
            string slotToElicit = InputModel.CurrentIntent.Slots.GetSlotToElicit();
            string responseMessage = String.Empty;
            string quizOrder = slots.QuizOrder == null ? String.Empty : slots.QuizOrder.ToLower();
            string quizProgression = slots.QuizProgression == null ? String.Empty : slots.QuizProgression.ToLower();
            ResponseCard responseCard = null;
            if (slotToElicit == nameof(QuizSlotType.DeckName))
            {
                responseMessage = "What is the name of the flash card deck you'd like to be quizzed on?";
                var allDecks = Dal.GetAllDecks(InputModel.UserID);
                if (allDecks != null && allDecks.Any())
                    responseMessage = String.Format("{0} {1}", responseMessage, DeckUtilitites.CreateDeckInformationMessage(allDecks));
            }
            else if (slotToElicit == nameof(QuizSlotType.QuizOrder))
            {
                var allDecks = Dal.GetAllDecks(InputModel.UserID);
                Deck quizDeck = allDecks?.FirstOrDefault(deck => deck.DeckName == slots.DeckName);
                if (allDecks != null && quizDeck!=null)
                {
                    responseMessage = "Would you liked to be quizzed in an in order or random fashion?";
                    responseCard = BuildOrderResponseCard(1);
                    quizDeck.Cards = quizDeck.GetNonDeletedCards();
                    quizDeck.ResetDeck();
                    Dal.UpdateDeck(quizDeck);
                }
                else
                {
                    responseMessage = "You don't have a deck with that name!";
                }
            }
            else if (slotToElicit == nameof(QuizSlotType.QuizProgression))
            {
                Card currentCard = null;
                bool deleteCurrent = false;
                bool quizOver = false;
                var deck = Dal.GetDeck(InputModel.UserID, slots.DeckName);
                if(!deck.Cards.Any())
                {
                    responseMessage = "The deck you selected does not contain any cards!";
                }
                //If quiz hasn't started, set random property and call GetNextCard, always. 
                //Otherwise, use the value of the QuizProgressionSlot
                else if(!deck.IsQuizStarted)
                {
                    deck.IsQuizRandom = quizOrder.Equals(Constants.QuizOrder.Random.ToString().ToLower());
                    currentCard = deck.GetNextCard();
                }
                else
                {
                    if (quizProgression == Constants.QuizProgression.Next.ToString().ToLower())
                    {
                        currentCard = deck.GetNextCard();
                    }
                    else if (quizProgression == Constants.QuizProgression.Previous.ToString().ToLower())
                    {
                        currentCard = deck.GetPreviousCard();
                    }
                    else if (quizProgression == Constants.QuizProgression.Skip.ToString().ToLower())
                    {
                        currentCard = deck.SkipCurrentCardAndGetNext();
                    }
                    else if(quizProgression == Constants.QuizProgression.Delete.ToString().ToLower())
                    {
                        currentCard = deck.GetCurrentCard();
                        deleteCurrent = true;
                    }
                    else if (quizProgression == Constants.QuizProgression.Stop.ToString().ToLower())
                    {
                        quizOver = true;
                    }
                    
                }
                if (currentCard == null)
                {
                    quizOver = true;
                }
                else if (deleteCurrent)
                {
                    currentCard.IsDeleted = true;
                    currentCard = deck.GetNextCard();
                }

                if (currentCard != null)
                {
                    slots.QuizProgression = null;
                    slotToElicit = slots.GetSlotToElicit();
                    var quizCardStatus = currentCard.GetCardStatus();
                    responseMessage = String.Format("Here is the {0} of the card.", quizCardStatus.ToString().ToLower());
                    responseCard = BuildProgressionResponseCard(2, currentCard.GetCardDataBasedOnStatus());
                    Dal.UpdateDeck(deck);
                }
                else
                {
                    quizOver = true;
                }
                if (quizOver)
                {
                    if (responseMessage == String.Empty)
                        responseMessage = "The quiz has ended!";
                    slotToElicit = null;
                    dialogActionType = Constants.DIALOG_ACTION_TYPE_CLOSE;
                    fulfillmentState = Constants.FULLFILLMENT_STATE_FULFILLED;
                    intentName = null;
                    slots = null;
                }
            }

            var outputModel = new OutputModel<QuizSlotType>();
            outputModel.dialogAction.fulfillmentState = fulfillmentState;
            outputModel.dialogAction.type = dialogActionType;
            outputModel.dialogAction.slots = slots;
            outputModel.dialogAction.message.content = responseMessage;
            outputModel.dialogAction.message.contentType = Constants.RESPONSE_CONTENT_TYPE;
            outputModel.dialogAction.slotToElicit = slotToElicit;
            outputModel.dialogAction.intentName = intentName;
            outputModel.dialogAction.responseCard = responseCard;
            return outputModel;
        }

        private ResponseCard BuildProgressionResponseCard(int version, string cardData)
        {
            var responseCard = BuildResponseCard(version);
            responseCard.genericAttachments = new List<GenericAttatchment>()
            {
                new GenericAttatchment()
                {
                    title = cardData,
                    buttons = new List<Button>()
                    {
                        new Button()
                        {
                            text = "Next",
                            value = "Next"
                        },
                        new Button()
                        {
                            text = "Skip",
                            value = "Skip"
                        },
                        new Button()
                        {
                            text = "Previous",
                            value = "Previous"
                        },
                        new Button()
                        {
                            text = "Delete",
                            value = "Delete"
                        },
                        new Button()
                        {
                            text = "Stop",
                            value = "Stop"
                        }
                    }
                }
            };
            return responseCard;
        }

        private ResponseCard BuildOrderResponseCard(int version)
        {
            var responseCard = BuildResponseCard(version);
            responseCard.genericAttachments = new List<GenericAttatchment>()
            {
                new GenericAttatchment()
                {
                title = "Select quiz type",
                buttons = new List<Button>()
                    {
                        new Button()
                        {
                            text = "In order",
                            value = "Sequential"
                        },
                        new Button()
                        {
                            text = "Random order",
                            value = "Random"
                        }
                    }
                }
            };
            return responseCard;
        }

        private ResponseCard BuildResponseCard(int version)
        {
            ResponseCard responseCard = new ResponseCard()
            {
                contentType = "application/vnd.amazonaws.card.generic",
                version = version
            };
            return responseCard;
        }
    }
}

