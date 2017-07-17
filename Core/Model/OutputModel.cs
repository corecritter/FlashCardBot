using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
    public class OutputModel<SlotType> where SlotType : ISlotType
    {
        private DialogAction<SlotType> _dialogAction;
        public DialogAction<SlotType> dialogAction
        {
            get
            {
                if(_dialogAction == null)
                {
                    _dialogAction = new DialogAction<SlotType>();
                }
                return _dialogAction;
            }
        }

        private SessionAttributes _sessionAttributes;
        public SessionAttributes sessionAttributes
        {
            get
            {
                if(_sessionAttributes == null)
                {
                    _sessionAttributes = new SessionAttributes();
                }
                return _sessionAttributes;
            }
        }
    }
}
