using Core.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class DeckUtilitites
    {
        private const string deckFormat = "{0}, ";
        private const string lastDeckFormat = "and {0}";
        private const string deckCountMessage = "You currently have {0}";
        private const string deckSingularPluralMessage = "{0}. The {1} of the {2} {3}:";

        private static string CreateDeckNameString(List<Deck> allDecks, bool finalOutput)
        {
            if (allDecks == null)
                return String.Empty;
            if (finalOutput)
                return CreateFinalizedDeckNameString(allDecks);
            StringBuilder sb = new StringBuilder();
            allDecks.ForEach(deck => sb.Append(String.Format(deckFormat, deck.DeckName)));
            return sb.ToString().Trim().TrimEnd(new char[] { ',' });
        }

        private static string CreateFinalizedDeckNameString(List<Deck> allDecks)
        {
            if (allDecks.Count == 1)
                return allDecks[0].DeckName;
            if (allDecks.Count == 2)
                return String.Format("{0} and {1}", allDecks[0].DeckName, allDecks[1].DeckName);
            var deckNames = allDecks.Select(deck => deck.DeckName);
            var deckCount = allDecks.Count;
            StringBuilder sb = new StringBuilder();
            for(int i=0; i< deckNames.Count(); i++)
            {
                if (i == allDecks.Count - 1)
                    sb.Append(String.Format(lastDeckFormat, allDecks[i].DeckName));
                else
                    sb.Append(String.Format(deckFormat, allDecks[i].DeckName));
            }
            return sb.ToString();
        }

        public static string CreateDeckInformationMessage(List<Deck> allDecks)
        {
            if(allDecks == null || allDecks.Count == 0)
            {
                return "You currently do not have any decks. Try adding one!";
            }
            List<string> deckNameString = new List<string>() { CreateDeckNameString(allDecks, false) };
            List<Tuple<string, string, string>> singularPluralValues = new List<Tuple<string, string, string>>();
            singularPluralValues.Add(new Tuple<string, string, string>("no decks", "deck", "decks"));
            singularPluralValues.Add(new Tuple<string, string, string>("", "name", "names"));
            singularPluralValues.Add(new Tuple<string, string, string>("", "deck", "decks"));
            singularPluralValues.Add(new Tuple<string, string, string>("", "is", "are"));

            string formattedMessage = ResponseBuilder.BuildPluralOrSingularMessage(deckSingularPluralMessage, singularPluralValues, deckNameString);
            return String.Format("{0} {1} {2}.", String.Format(deckCountMessage, allDecks.Count.ToString()), formattedMessage, CreateDeckNameString(allDecks, true));
        }
    }
}
