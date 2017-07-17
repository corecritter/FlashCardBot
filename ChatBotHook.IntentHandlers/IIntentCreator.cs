using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotHook.IntentHandlers
{
    public interface IIntentCreator
    {
        IIntentHandler GetIntentHandler(string intentName);
    }
}
