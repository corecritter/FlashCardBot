using ChatBotHook.IntentHandlers.Test.Mock;
using Core;
using Core.Model;
using Core.Model.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace ChatBotHook.IntentHandlers.Test
{
    public class ManageDeckHandlerTests
    {
        [Fact]
        public void Test_ManageDeckHandler()
        {
            InputModel<ManageDeckSlotType> inputModel = new InputModel<ManageDeckSlotType>();
            inputModel.UserID = "useid";
            inputModel.CurrentIntent.Name = "ManageDecks";
            inputModel.CurrentIntent.Slots = new ManageDeckSlotType();
            inputModel.CurrentIntent.Slots.ManageType = Constants.ManageTypes.Add.ToString();
            string json = JsonConvert.SerializeObject(inputModel);

            MockDAL mockDal = new MockDAL();
            ManageDeckHandler manageDeckHandler = new ManageDeckHandler(mockDal);

            string output = manageDeckHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            OutputModel<ManageDeckSlotType> outputModel= JsonConvert.DeserializeObject<OutputModel<ManageDeckSlotType>>(output);

            Assert.NotNull(outputModel);
            Assert.Equal(nameof(ManageDeckSlotType.DeckName), outputModel.dialogAction.slotToElicit);
            Assert.NotNull(outputModel.dialogAction.slots);
            Assert.Equal(Constants.ManageTypes.Add.ToString(), outputModel.dialogAction.slots.ManageType);

            inputModel.CurrentIntent.Slots.DeckName = "DeckName";
            json = JsonConvert.SerializeObject(inputModel);
            output = manageDeckHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            outputModel = JsonConvert.DeserializeObject<OutputModel<ManageDeckSlotType>>(output);

            Assert.False(mockDal.AddDeckCalled);
            Assert.False(mockDal.GetDeckCalled);
            Assert.False(mockDal.UpdateDeckCalled);

            inputModel.CurrentIntent.Slots.Confirm = "yes";
            json = JsonConvert.SerializeObject(inputModel);
            output = manageDeckHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            outputModel = JsonConvert.DeserializeObject<OutputModel<ManageDeckSlotType>>(output);

            Assert.True(mockDal.AddDeckCalled);
            Assert.False(mockDal.GetDeckCalled);
            Assert.False(mockDal.UpdateDeckCalled);
            Assert.Equal("useid", mockDal.LastDataPassed.UserId);
            Assert.Equal("DeckName", mockDal.LastDataPassed.DeckName);

            mockDal.Reset();
            mockDal.DeckExists = true;
            inputModel.CurrentIntent.Slots = new ManageDeckSlotType();
            inputModel.CurrentIntent.Slots.ManageType = Constants.ManageTypes.Modify.ToString();

            json = JsonConvert.SerializeObject(inputModel);
            output = manageDeckHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            outputModel = JsonConvert.DeserializeObject<OutputModel<ManageDeckSlotType>>(output);

            Assert.Equal(nameof(ManageDeckSlotType.DeckName), outputModel.dialogAction.slotToElicit);

            inputModel.CurrentIntent.Slots.DeckName = "D1";
            json = JsonConvert.SerializeObject(inputModel);
            output = manageDeckHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            outputModel = JsonConvert.DeserializeObject<OutputModel<ManageDeckSlotType>>(output);

            Assert.False(mockDal.AddDeckCalled);
            Assert.Equal(nameof(ManageDeckSlotType.Front), outputModel.dialogAction.slotToElicit);

            inputModel.CurrentIntent.Slots.Front = "Front";
            json = JsonConvert.SerializeObject(inputModel);
            output = manageDeckHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            outputModel = JsonConvert.DeserializeObject<OutputModel<ManageDeckSlotType>>(output);

            Assert.Equal(nameof(ManageDeckSlotType.Back), outputModel.dialogAction.slotToElicit);

            inputModel.CurrentIntent.Slots.Back = "Back";
            json = JsonConvert.SerializeObject(inputModel);
            output = manageDeckHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            outputModel = JsonConvert.DeserializeObject<OutputModel<ManageDeckSlotType>>(output);

            Assert.Equal(nameof(ManageDeckSlotType.Confirm), outputModel.dialogAction.slotToElicit);
            Assert.True(mockDal.AddCardToDeckCaleled);
            Assert.NotNull(mockDal.LastDataPassed);

            inputModel.CurrentIntent.Slots.Confirm = "yes";
            json = JsonConvert.SerializeObject(inputModel);
            output = manageDeckHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            outputModel = JsonConvert.DeserializeObject<OutputModel<ManageDeckSlotType>>(output);

            Assert.Equal(nameof(ManageDeckSlotType.Front), outputModel.dialogAction.slotToElicit);
            Assert.False(String.IsNullOrEmpty(outputModel.dialogAction.message.content));

            inputModel.CurrentIntent.Slots.Confirm = "no";
            json = JsonConvert.SerializeObject(inputModel);
            output = manageDeckHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            outputModel = JsonConvert.DeserializeObject<OutputModel<ManageDeckSlotType>>(output);

            Assert.Equal(Constants.FULLFILLMENT_STATE_FULFILLED, outputModel.dialogAction.fulfillmentState);
            Assert.Null(outputModel.dialogAction.slots);
            Assert.Null(outputModel.dialogAction.intentName);
            Assert.Null(outputModel.dialogAction.slotToElicit);

            mockDal.Reset();
            mockDal.DeckExists = false;

            inputModel.CurrentIntent.Slots.Confirm  = null;
            inputModel.CurrentIntent.Slots.Front = null;
            inputModel.CurrentIntent.Slots.Back = null;
            json = JsonConvert.SerializeObject(inputModel);
            output = manageDeckHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            outputModel = JsonConvert.DeserializeObject<OutputModel<ManageDeckSlotType>>(output);

            Assert.Equal(Constants.FULLFILLMENT_STATE_FULFILLED, outputModel.dialogAction.fulfillmentState);

            inputModel.CurrentIntent.Slots.ManageType = Constants.ManageTypes.Modify.ToString();
            json = JsonConvert.SerializeObject(inputModel);
            output = manageDeckHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            outputModel = JsonConvert.DeserializeObject<OutputModel<ManageDeckSlotType>>(output);

            Assert.Equal(Constants.FULLFILLMENT_STATE_FULFILLED, outputModel.dialogAction.fulfillmentState);

            inputModel.CurrentIntent.Slots.ManageType = Constants.ManageTypes.Delete.ToString();
            json = JsonConvert.SerializeObject(inputModel);
            output = manageDeckHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            outputModel = JsonConvert.DeserializeObject<OutputModel<ManageDeckSlotType>>(output);

            Assert.Equal(Constants.FULLFILLMENT_STATE_FULFILLED, outputModel.dialogAction.fulfillmentState);

            mockDal.DeckExists = true;
            json = JsonConvert.SerializeObject(inputModel);
            output = manageDeckHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            outputModel = JsonConvert.DeserializeObject<OutputModel<ManageDeckSlotType>>(output);

            Assert.Equal(nameof(ManageDeckSlotType.Confirm), outputModel.dialogAction.slotToElicit);

            inputModel.CurrentIntent.Slots.Confirm = "no";
            json = JsonConvert.SerializeObject(inputModel);
            output = manageDeckHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            outputModel = JsonConvert.DeserializeObject<OutputModel<ManageDeckSlotType>>(output);

            Assert.False(mockDal.DeleteDeckCalled);
            Assert.Equal(Constants.FULLFILLMENT_STATE_FULFILLED, outputModel.dialogAction.fulfillmentState);

            inputModel.CurrentIntent.Slots.Confirm = "yes";
            json = JsonConvert.SerializeObject(inputModel);
            output = manageDeckHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            outputModel = JsonConvert.DeserializeObject<OutputModel<ManageDeckSlotType>>(output);

            Assert.True(mockDal.DeleteDeckCalled);
            Assert.Equal(Constants.FULLFILLMENT_STATE_FULFILLED, outputModel.dialogAction.fulfillmentState);

            mockDal.Reset();
            mockDal.DecksToReturn = new List<Deck>()
            {
                new Deck()
                { DeckName = "Deck1" },
                new Deck()
                { DeckName = "Deck2"}
            };
            inputModel.CurrentIntent.Slots.Confirm = null;
            inputModel.CurrentIntent.Slots.DeckName = null;
            json = JsonConvert.SerializeObject(inputModel);
            output = manageDeckHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            outputModel = JsonConvert.DeserializeObject<OutputModel<ManageDeckSlotType>>(output);

            Assert.True(outputModel.dialogAction.message.content.Contains("2"));

            mockDal.DecksToReturn = null;
            json = JsonConvert.SerializeObject(inputModel);
            output = manageDeckHandler.HandleIntent(JsonConvert.DeserializeObject<dynamic>(json));
            outputModel = JsonConvert.DeserializeObject<OutputModel<ManageDeckSlotType>>(output);

            Assert.False(outputModel.dialogAction.message.content.Contains("You currently have"));
        }
    }
}
