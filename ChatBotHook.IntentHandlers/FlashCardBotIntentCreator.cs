using Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotHook.IntentHandlers
{
    public class FlashCardBotIntentCreator : IntentCreator
    {
        protected override IIntentHandler CreateIntentHandler(string intentName)
        {
            IIntentHandler intentHandler = null;
            if (intentName == "managedecks")
                intentHandler = new ManageDeckHandler();
            if (intentName == "quiz")
                intentHandler = new QuizIntentHandler();
            return intentHandler;
        }
    }
}
