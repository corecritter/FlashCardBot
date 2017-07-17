using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class ResponseBuilder
    {
        public static OutputModel<T> BuildResponse<T>(OutputModel<T> outputModel = null) where T : ISlotType
        {
            if (outputModel == null)
                outputModel = new OutputModel<T>();



            return outputModel;
        }

        /// <summary>
        /// Formats a string with singular or plural values, based on comma separated list counts
        /// </summary>
        /// <param name="messageFormat">The string to format and return</param>
        /// <param name="noneSingularPluralValues">The potential none, singular, and plural values to format</param>
        /// <param name="values">The list of comma separetd lists to count the items in</param>
        /// <returns></returns>
        public static string BuildPluralOrSingularMessage(string messageFormat, List<Tuple<string, string, string>> noneSingularPluralValues, List<string> values)
        {
            List<string> outputValues = new List<string>();
            int index = 0;
            if (values.Count == 1)
            {
                var splitValueCount = GetSplitCount(values[0]);
                foreach(var singularPluralValue in noneSingularPluralValues)
                {
                    if (splitValueCount == 0)
                        outputValues.Add(singularPluralValue.Item1);
                    if (splitValueCount == 1)
                        outputValues.Add(singularPluralValue.Item2);
                    else
                        outputValues.Add(singularPluralValue.Item3);
                }
            }
            else
            {
                foreach (var value in values)
                {
                    var splitValueCount = GetSplitCount(value);
                    if (splitValueCount == 0)
                        outputValues.Add(noneSingularPluralValues[index].Item1);
                    else if (splitValueCount == 1)
                        outputValues.Add(noneSingularPluralValues[index].Item2);
                    else
                        outputValues.Add(noneSingularPluralValues[index].Item3);
                    index++;
                }
            }
            return String.Format(messageFormat, outputValues.ToArray());
        }

        private static int GetSplitCount(string value)
        {
            if (String.IsNullOrEmpty(value))
                return 0;
            return value.Split(new char[] { ',' }).Count();
        }
    }
}
