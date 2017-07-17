using ChatBotHook.IntentHandlers.Test.Mock;
using Core;
using Core.Model;
using Core.Model.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ChatBotHook.IntentHandlers.Test
{
    public class QuizIntentHandlerTests
    {
        string deckNameForm = "Deck_{0}";
        string frontForm = "Front_{0}";
        string backForm = "Back_{0}";

        [Fact]
        public void Test()
        {
            InputModel<QuizSlotType> inputModel = new InputModel<QuizSlotType>();
            inputModel.UserID = "useid";
            inputModel.CurrentIntent.Name = "Quiz";
            inputModel.CurrentIntent.Slots = new QuizSlotType();

            MockDAL mockDal = new MockDAL();
            mockDal.DecksToReturn = CreateDecks(2, 3);
            mockDal.DeckExists = true;
            QuizIntentHandler quizHandler = new QuizIntentHandler(mockDal);

            string json = JsonConvert.SerializeObject(inputModel);
            string output = quizHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            OutputModel<QuizSlotType> outputModel = JsonConvert.DeserializeObject<OutputModel<QuizSlotType>>(output);
            Assert.NotNull(outputModel);

            inputModel.CurrentIntent.Slots.DeckName = "Deck_1";

            json = JsonConvert.SerializeObject(inputModel);
            output = quizHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            outputModel = JsonConvert.DeserializeObject<OutputModel<QuizSlotType>>(output);

            Assert.NotNull(outputModel);
            Assert.Equal(nameof(QuizSlotType.QuizOrder), outputModel.dialogAction.slotToElicit);

            inputModel.CurrentIntent.Slots.QuizOrder = Constants.QuizOrder.Sequential.ToString();
            json = JsonConvert.SerializeObject(inputModel);
            output = quizHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            outputModel = JsonConvert.DeserializeObject<OutputModel<QuizSlotType>>(output);

            Assert.NotNull(outputModel.dialogAction.responseCard);
            Assert.NotNull(outputModel.dialogAction.slots);
            Assert.Equal(nameof(QuizSlotType.QuizProgression), outputModel.dialogAction.slotToElicit);

            inputModel.CurrentIntent.Slots.QuizProgression = Constants.QuizProgression.Next.ToString();
            json = JsonConvert.SerializeObject(inputModel);
            output = quizHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            outputModel = JsonConvert.DeserializeObject<OutputModel<QuizSlotType>>(output);

        }

        public List<Deck> CreateDecks(int deckCount, int cardCount)
        {
            List<Deck> decks = new List<Deck>();
            for(int deckNumber = 1; deckNumber < deckCount + 1; deckNumber++)
            {
                decks.Add(new Deck() { DeckName = String.Format(deckNameForm, deckNumber) });
                for (int i = 1; i < cardCount + 1; i++)
                {
                    decks[decks.Count - 1].Cards.Add(new Card() { Front = String.Format(frontForm, i.ToString()), Back = String.Format(backForm, i.ToString()) });
                }
            }
            return decks;
        }
    }
}
