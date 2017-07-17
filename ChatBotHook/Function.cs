using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using NLog;
using ChatBotHook.IntentHandlers;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
//[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
[assembly: LambdaSerializer(typeof(ChatBotHook.Parse.InputDeserializer))]

namespace ChatBotHook
{
    public class Function
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private IIntentCreator _intentCreator = new FlashCardBotIntentCreator();

        public dynamic FunctionHandler(dynamic inputModel, ILambdaContext context)
        {
            _logger.Info("Called Function Handler");
            //_logger.Info(inputModel.ToString());

            string intentName = String.Empty;
            if (inputModel.currentIntent != null)
                intentName = inputModel.currentIntent.name.ToString();
            IIntentHandler intentHandler = _intentCreator.GetIntentHandler(intentName);
            return JsonConvert.DeserializeObject<dynamic>(intentHandler.HandleIntent(inputModel));
        }
    }
}
