using System;
using Xunit;
using ChatBotHook.DAL;

namespace ChatBotHook.DAL.Tests
{
    public class DynomoDBTests
    {
        //[Fact]
        public void Test_Load()
        {
            DynomoDatabaseDAL dataAccess = new DynomoDatabaseDAL();
            var deck = dataAccess.GetDeck("User1", "D11");
        }

        //[Fact]
        public void Test_Add()
        {
            DynomoDatabaseDAL dataAccess = new DynomoDatabaseDAL();
            dataAccess.AddNewDeck("User1", "D1");
        }

        //[Fact]
        public void Test_GetDecks()
        {
            DynomoDatabaseDAL dataAccess = new DynomoDatabaseDAL();
            var decks = dataAccess.GetAllDecks("f14ahg07kbm0w78f97pqgxfwwxxval6c");
        }

        //[Fact]
        public void Test_DeleteDeck()
        {
            DynomoDatabaseDAL dataAccess = new DynomoDatabaseDAL();
            dataAccess.DeleteDeck("f14ahg07kbm0w78f97pqgxfwwxxval6c", "Deck2");
        }
    }
}
