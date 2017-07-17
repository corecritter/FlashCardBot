using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotHook.IntentHandlers
{
    public abstract class IntentCreator : IIntentCreator
    {
        public IIntentHandler GetIntentHandler(string intentName)
        {
            if (String.IsNullOrEmpty(intentName))
                return null;
            return CreateIntentHandler(intentName.ToLower());
        }

        protected abstract IIntentHandler CreateIntentHandler(string intentName);
    }
}
