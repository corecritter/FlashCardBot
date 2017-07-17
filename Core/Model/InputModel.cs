using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
    public class InputModel<SlotType> where SlotType : ISlotType
    {
        public string MessageVersion { get; set; }
        public string InvocationSource { get; set; }
        public string UserID { get; set; }
        private Bot _Bot;
        public Bot Bot
        {
            get
            {
                if(_Bot == null)
                {
                    _Bot = new Bot();
                }
                return _Bot;
            }
            set
            {
                _Bot = value;
            }
        }

        public string OutputDialogMode { get; set; }

        private CurrentIntent<SlotType> _CurrentIntent;
        public CurrentIntent<SlotType> CurrentIntent {
            get
            {
                if(_CurrentIntent == null)
                {
                    _CurrentIntent = new CurrentIntent<SlotType>();
                }
                return _CurrentIntent;
            }
            set
            {
                _CurrentIntent = value;
            }
        }

        public SessionAttributes SessionAttributes { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (CurrentIntent != null)
            {
                sb.Append(CurrentIntent.ToString());
                if (this.CurrentIntent.Slots != null)
                {
                    sb.Append(Environment.NewLine);
                    sb.Append(this.CurrentIntent.Slots.ToString());
                }
            }
            if (this.Bot != null)
            {
                sb.Append(Environment.NewLine);
                sb.Append(this.Bot.ToString());
            }
            return sb.ToString();
        }
    }
}
