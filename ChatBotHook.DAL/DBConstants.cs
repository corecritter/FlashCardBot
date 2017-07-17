using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotHook.DAL
{
    internal class DBConstants
    {
        //Table Names
        public const string TABLE_NAME_USER = "User";
        public const string TABLE_NAME_DECK = "Deck";
        public const string TABLE_NAME_CARD = "Card";

        //ColumnNames
        public const string COLUMN_NAME_USER_USERID = "UserID";
        public const string COLUMN_NAME_DECK_DECKID = "DeckID";
        public const string COLUMN_NAME_DECK_NAME = "DeckName";
        public const string COLUMN_NAME_CARD_CARDID = "CardID";
        public const string COLUMN_NAME_CARD_FRONT = "Front";
        public const string COLUMN_NAME_CARD_BACK = "Back";
    }
}
