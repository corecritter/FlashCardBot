using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class Constants
    {
        //Messages
        public const string STRING_FORMAT_PROPERTY_VALUE = "{0} : {1}, ";

        public const string MESSAGE_RESPONSE_UPDATE_SUCCESS = "Done! Your flash card deck has been {0}";
        public const string MESSAGE_RESPONSE_UPDATE_FAILURE = "Unsuccessful! {0}";

        //Error Messages
        public const string ERROR_MESSAGE_INVALID_VALUE = "{0} is invalid.";
        public const string ERROR_MESSAGE_RETRY_RESPONSE = "{0}";

        //Response Names
        public const string RESPONSE_CONTENT_TYPE = "PlainText";

        //IntentNames
        public const string INTENT_NAME_MANAGE_DECKS = "ManageDecks";
        public const string INTENT_NAME_QUIZ = "Quiz";

        //Dialog Action Types
        public const string DIALOG_ACTION_TYPE_ELICIT = "ElicitSlot";
        public const string DIALOG_ACTION_TYPE_CLOSE = "Close";

        //Fulfillment States
        public const string FULLFILLMENT_STATE_FULFILLED = "Fulfilled";
        public const string FULFILLMENT_STATE_FAILED = "Failed";

        //Sommething else
        public enum ManageTypes
        {
            Add,
            Modify,
            Delete
        }
        public enum QuizProgression
        {
            Skip,
            Previous,
            Stop,
            Next,
            Delete
        }
        public enum QuizOrder
        {
            Random,
            Sequential
        }
    }
}
