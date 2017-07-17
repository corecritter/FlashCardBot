using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using ChatBotHook;
using ChatBotHook.Parse;
using System.IO;
using Core.Model;

namespace ChatBotHook.Tests
{
    public class FunctionTest
    {
        //string input = "{\"currentIntent\": { \"slots\": { \"PickUpDate\": \"2030-11-08\", \"PickUpCity\": \"Chicago\", \"ReturnDate\": \"2030-11-08\", \"CarType\": \"economy\", \"DriverAge\": 21 },\"name\": \"ManageDecks\",\"confirmationStatus\": \"None\"},\"bot\": {\"alias\": \"$LATEST\",\"version\": \"$LATEST\",\"name\": \"BookTrip\"},\"userId\": \"John\",\"invocationSource\": \"DialogCodeHook\",\"outputDialogMode\": \"Text\",\"messageVersion\": \"1.0\",\"sessionAttributes\": { }}";
        string input = "{\"MessageVersion\":null,\"InvocationSource\":null,\"UserID\":null,\"Bot\":{\"Alias\":null,\"Version\":null,\"Name\":null},\"OutputDialogMode\":null,\"CurrentIntent\":{\"Name\":\"ManageDecks\",\"ConfirmationStatus\":null,\"Slots\":{\"ManageType\":\"Add\",\"DeckName\":null,\"Front\":null,\"Back\":null}},\"SessionAttributes\":null}";
        [Fact]
        public void TestDeserialize()
        {
            var returnModel = CreateTestModel<ManageDeckSlotType>();
            Assert.NotNull(returnModel);
            Assert.IsType(typeof(InputModel<ManageDeckSlotType>), returnModel);
            Assert.NotNull(returnModel.CurrentIntent);
            Assert.NotNull(returnModel.CurrentIntent.Slots);
            Assert.NotNull(returnModel.Bot);
        }

        [Fact]
        public void TestSerialize()
        {
            string filePath = @"C:\testFolder\test.txt";
            InputModel<ManageDeckSlotType> inputModel = CreateTestModel<ManageDeckSlotType>();
            MemoryStream outputStream = new MemoryStream();
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                new InputDeserializer().Serialize<InputModel<ManageDeckSlotType>>(inputModel, fs);
            }
            Assert.True(File.Exists(filePath));
            DeleteFile(filePath);
        }

        private InputModel<T> CreateTestModel<T>() where T : ISlotType
        {
            InputDeserializer d = new InputDeserializer();
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(ms))
                {
                    sw.Write(input);
                    sw.Flush();
                    var l = ms.Length;
                    ms.Position = 0;
                    return d.Deserialize<InputModel<T>>(ms);
                }
            }
        }

        [Fact]
        public void TestSerialize_String()
        {
            string filePath = @"C:\testFolder\test.txt";
            MemoryStream outputStream = new MemoryStream();
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                new InputDeserializer().Serialize<string>(input, fs);
            }
            Assert.True(File.Exists(filePath));
            DeleteFile(filePath);
        }

        private void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        //[Fact]
        //public void Test()
        //{
        //    Function f = new Function();
        //    InputDeserializer d = new InputDeserializer();
        //    using (var ms = new MemoryStream())
        //    {
        //        using (var sw = new StreamWriter(ms))
        //        {
        //            sw.Write(input);
        //            sw.Flush();
        //            var l = ms.Length;
        //            ms.Position = 0;
        //            var inputModel =  d.Deserialize<dynamic>(ms);
        //            string output = f.FunctionHandler(inputModel, null);
        //        }
        //    }
        //}
    }
}
