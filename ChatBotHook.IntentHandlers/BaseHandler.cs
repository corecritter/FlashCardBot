using System;
using System.Collections.Generic;
using System.Text;
using Core.Model;
using Newtonsoft.Json;
using ChatBotHook.DAL;

namespace ChatBotHook.IntentHandlers
{
    public abstract class BaseHandler<T> : IIntentHandler<T> where T : ISlotType
    {
        protected IDatabaseDAL Dal;
        public BaseHandler(IDatabaseDAL dal) //For unit testing
        {
            Dal = dal;
        }

        public BaseHandler()
        {
            Dal = new DynomoDatabaseDAL();
        }

        public InputModel<T> InputModel { get; set; }

        public string HandleIntent(dynamic inputModel)
        {
            InputModel = JsonConvert.DeserializeObject<InputModel<T>>(inputModel.ToString());
            return SerializeOutput(ProcessIntent())
                .Replace("\"fulfillmentState\":null,", String.Empty)
                .Replace("\"intentName\":null,", String.Empty)
                .Replace("\"slotToElicit\":null,", String.Empty)
                .Replace("\"slots\":null,", String.Empty)
                .Replace("\"responseCard\":null,", String.Empty);
                //.Replace("\"message\":{\"contentType\":null,\"content\":null},", String.Empty);
        }

        private string SerializeOutput(OutputModel<T> outputModel)
        {
            return JsonConvert.SerializeObject(outputModel);
        }

        protected abstract OutputModel<T> ProcessIntent();
    }
}
