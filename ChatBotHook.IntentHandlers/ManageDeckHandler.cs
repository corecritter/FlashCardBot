using ChatBotHook.DAL;
using Core;
using Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatBotHook.IntentHandlers
{
    public class ManageDeckHandler : BaseHandler<ManageDeckSlotType>
    {
        public ManageDeckHandler() { }

        public ManageDeckHandler(IDatabaseDAL dal) : base(dal) { }

        protected override OutputModel<ManageDeckSlotType> ProcessIntent()
        {
            string dialogActionType = Constants.DIALOG_ACTION_TYPE_ELICIT;
            string fulfillmentState = null;
            string intentName = InputModel.CurrentIntent.Name;
            var slots = InputModel.CurrentIntent.Slots;
            var errors = InputModel.CurrentIntent.Slots.Validate();
            string slotToElicit = InputModel.CurrentIntent.Slots.GetSlotToElicit();
            string responseMessage = String.Empty;
            string manageType = InputModel.CurrentIntent.Slots.ManageType.ToLower();

            if (slotToElicit == nameof(ManageDeckSlotType.ManageType))
            {
                var error = errors.FirstOrDefault(rr => rr.PropertyName == nameof(ManageDeckSlotType.ManageType));

                if (error != null)
                    responseMessage = "Would you like to add a new, modify an existing, or delete a flash card deck?";
                else
                {
                    responseMessage = String.Format(Constants.ERROR_MESSAGE_INVALID_VALUE, "Manage Type is invalid");
                    InputModel.CurrentIntent.Slots.ManageType = null;
                }
            }
            else if(slotToElicit == nameof(ManageDeckSlotType.DeckName))
            {
                responseMessage = String.Format("What is the name of the flash card deck you'd like to {0}?", manageType);
                var allDecks = Dal.GetAllDecks(InputModel.UserID);
                if (allDecks != null && allDecks.Any())
                    responseMessage = String.Format("{0} {1}", responseMessage, DeckUtilitites.CreateDeckInformationMessage(allDecks));
            }
            else if (slotToElicit == nameof(ManageDeckSlotType.Front))
            {
                if (Dal.DeckExits(InputModel.UserID, slots.DeckName))
                {
                    responseMessage = "Enter the front of the card";
                }
                else
                {
                    responseMessage = String.Format("You don't have a flash card deck named {0}!", slots.DeckName);
                    slotToElicit = null;
                    dialogActionType = Constants.DIALOG_ACTION_TYPE_CLOSE;
                    fulfillmentState = Constants.FULLFILLMENT_STATE_FULFILLED;
                    intentName = null;
                    slots = null;
                }
            }
            else if (slotToElicit == nameof(ManageDeckSlotType.Back))
            {
                responseMessage = "Enter the back of the card";
            }
            else if (slotToElicit == nameof(ManageDeckSlotType.Confirm))
            {
                bool fulFill = false;
                if (manageType == Constants.ManageTypes.Add.ToString().ToLower())
                {
                    if (!Dal.DeckExits(InputModel.UserID, slots.DeckName))
                        responseMessage = String.Format("Are you sure you want to add {0} as a new flash card deck?", InputModel.CurrentIntent.Slots.DeckName);
                    else
                    {
                        fulFill = true;
                        responseMessage = String.Format("You already have a deck named {0}!", slots.DeckName);
                    }
                }
                else if (manageType == Constants.ManageTypes.Delete.ToString().ToLower())
                {
                    if (Dal.DeckExits(InputModel.UserID, slots.DeckName))
                        responseMessage = String.Format("Are you sure you want to delete the flash card deck named {0}", InputModel.CurrentIntent.Slots.DeckName);
                    else
                    {
                        fulFill = true;
                        responseMessage = String.Format("You don't have a deck named {0}!", slots.DeckName);
                    }
                }
                else if(manageType == Constants.ManageTypes.Modify.ToString().ToLower())
                {
                    Dal.AddCardToDeck(InputModel.UserID, slots.DeckName, slots.Front, slots.Back);
                    responseMessage = "Would you like to add another?";
                }
                if (fulFill)
                {
                    slotToElicit = null;
                    dialogActionType = Constants.DIALOG_ACTION_TYPE_CLOSE;
                    fulfillmentState = Constants.FULLFILLMENT_STATE_FULFILLED;
                    intentName = null;
                    slots = null;
                }
            }
            else if(slotToElicit == string.Empty)
            {
                if (InputModel.CurrentIntent.Slots.Confirm.ToLower() == "yes")
                {
                    string successType = String.Empty;
                    if (manageType == Constants.ManageTypes.Modify.ToString().ToLower())
                    {
                        slots.Front = null;
                        slots.Back = null;
                        slots.Confirm = null;
                        slotToElicit = slots.GetSlotToElicit();
                        responseMessage = "Enter the front of the card";
                    }
                    else
                    {
                        if (manageType == Constants.ManageTypes.Add.ToString().ToLower())
                        {
                            manageType = "added";
                            Dal.AddNewDeck(InputModel.UserID, slots.DeckName);
                            responseMessage = String.Format(Constants.MESSAGE_RESPONSE_UPDATE_SUCCESS, manageType);
                        }
                        else if (manageType == Constants.ManageTypes.Delete.ToString().ToLower())
                        {
                            manageType = "deleted";
                            responseMessage = String.Format(Constants.MESSAGE_RESPONSE_UPDATE_SUCCESS, manageType);
                            Dal.DeleteDeck(InputModel.UserID, slots.DeckName);
                        }
                        slotToElicit = null;
                        dialogActionType = Constants.DIALOG_ACTION_TYPE_CLOSE;
                        fulfillmentState = Constants.FULLFILLMENT_STATE_FULFILLED;
                        intentName = null;
                        slots = null;
                    }
                }
                else
                {
                    responseMessage = "OK";
                    slotToElicit = null;
                    dialogActionType = Constants.DIALOG_ACTION_TYPE_CLOSE;
                    fulfillmentState = Constants.FULLFILLMENT_STATE_FULFILLED;
                    intentName = null;
                    slots = null;
                }
            }

            var outputModel = new OutputModel<ManageDeckSlotType>();
            outputModel.dialogAction.type = dialogActionType;
            outputModel.dialogAction.fulfillmentState = fulfillmentState;
            outputModel.dialogAction.slots = slots;
            outputModel.dialogAction.message.content = responseMessage;
            outputModel.dialogAction.message.contentType = Constants.RESPONSE_CONTENT_TYPE;
            outputModel.dialogAction.slotToElicit = slotToElicit;
            outputModel.dialogAction.intentName = intentName;
            return outputModel;
        }
    }
}