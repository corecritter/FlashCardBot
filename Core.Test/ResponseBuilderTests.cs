using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Core.Test
{
    public class ResponseBuilderTests
    {
        [Fact]
        public void PluralOrSingular()
        {
            //Test single format value, single singular format output value
            string formatMessage = "You have {0}.";
            List<Tuple<string, string, string>> singularPluralValues = new List<Tuple<string, string, string>>();
            singularPluralValues.Add(new Tuple<string, string, string>("no decks", "a deck", "many decks"));
            List<string> values = new List<string>() { "Item1" };

            string output = ResponseBuilder.BuildPluralOrSingularMessage(formatMessage, singularPluralValues, values);
            Assert.Equal("You have a deck.", output);

            //Test single format value, single plural format output value
            values = new List<string>() { "Item1, Item2" };
            output = ResponseBuilder.BuildPluralOrSingularMessage(formatMessage, singularPluralValues, values);
            Assert.Equal("You have many decks.", output);

            //Test multiple format values, multiple plural output values, multiple count values
            formatMessage = "You have {0}. The {1} of the {2} {3}";
            singularPluralValues = new List<Tuple<string, string, string>>();
            singularPluralValues.Add(new Tuple<string, string, string>("none", "a deck", "many decks"));
            singularPluralValues.Add(new Tuple<string, string, string>("no", "name", "names"));
            singularPluralValues.Add(new Tuple<string, string, string>("zero", "deck", "decks"));
            singularPluralValues.Add(new Tuple<string, string, string>("nope", "is", "are"));
            values = new List<string>() { "Item1, Item2", "Item1, Item2", "Item1, Item2", "Item1, Item2" };

            output = ResponseBuilder.BuildPluralOrSingularMessage(formatMessage, singularPluralValues, values);
            Assert.Equal("You have many decks. The names of the decks are", output);

            //Test multiple format values, multiple plural output values, single count values
            values = new List<string>() { "Item1, Item2" };
            output = ResponseBuilder.BuildPluralOrSingularMessage(formatMessage, singularPluralValues, values);
            Assert.Equal("You have many decks. The names of the decks are", output);

            //Test multiple format values, multiple singular and plural output values, multiple count values
            values = new List<string>() { "", "Item1, Item2", "Item1, Item2", "Item1, Item2" };
            output = ResponseBuilder.BuildPluralOrSingularMessage(formatMessage, singularPluralValues, values);
            Assert.Equal("You have none. The names of the decks are", output);
        }
    }
}
