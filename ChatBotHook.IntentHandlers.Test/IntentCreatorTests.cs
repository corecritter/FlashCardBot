using Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ChatBotHook.IntentHandlers.Test
{

    public class IntentCreatorTests
    {
        [Fact]
        public void Test_FlashCardBotIntentCreator()
        {
            FlashCardBotIntentCreator intentCreator = new FlashCardBotIntentCreator();

            IIntentHandler intentHandler =  intentCreator.GetIntentHandler(Constants.INTENT_NAME_MANAGE_DECKS);
            Assert.Equal(typeof(ManageDeckHandler), intentHandler.GetType());

            intentHandler = intentCreator.GetIntentHandler("SomeInvalidIntent");
            Assert.Null(intentHandler);
        }
    }
}
