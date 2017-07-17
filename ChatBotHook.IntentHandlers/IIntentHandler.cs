using Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotHook.IntentHandlers
{
    public interface IIntentHandler<SlotType> : IIntentHandler where SlotType : ISlotType 
    {
        //InputModel<SlotType> InputModel { get; set; }
    }

    public interface IIntentHandler
    {
        string HandleIntent(dynamic inputModel);
    }
}
