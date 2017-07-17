using System;
using System.Collections.Generic;
using System.Text;
using Core.ComponentModel;
using System.Collections;
using System.Linq;

namespace Core.Model
{
    public class QuizSlotType : ISlotType
    {
        public string DeckName { get; set; }
        public string QuizOrder { get; set; }
        public string QuizProgression { get; set; }

        public string GetSlotToElicit()
        {
            if (String.IsNullOrEmpty(DeckName) || String.IsNullOrWhiteSpace(DeckName))
                return nameof(DeckName);
            if (String.IsNullOrEmpty(QuizOrder) || String.IsNullOrWhiteSpace(QuizOrder))
                return nameof(QuizOrder);
            //if (String.IsNullOrEmpty(QuizProgression) || String.IsNullOrWhiteSpace(QuizProgression))
                return nameof(QuizProgression);
        }

        public IEnumerable<ValidationError> Validate()
        {
            return new List<ValidationError>();
        }
    }
}
