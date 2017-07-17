using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using ChatBotHook.DAL;

namespace ChatBotHook.IntentHandlers.Test.Mock
{
    public class MockMakers
    {
        public static Moq.Mock<IDatabaseDAL> CreateMoqDAL()
        {
            Moq.Mock<IDatabaseDAL> dal = new Mock<IDatabaseDAL>();
            dal.Setup(svc => svc.AddNewDeck(It.IsAny<string>(), It.IsAny<string>(), null));
            return dal;
        }
    }
}
