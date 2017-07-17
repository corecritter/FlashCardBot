namespace Core.Model
{
    public class DialogAction<SlotType>
    {
        public string type { get; set; }
        public string intentName { get; set; }
        public string slotToElicit { get; set; }
        public string fulfillmentState { get; set; }

        private Message _message;
        public Message message
        {
            get
            {
                if(_message == null)
                {
                    _message = new Message();
                }
                return _message;
            }
            set
            {
                _message = value;
            }
        }
        public SlotType slots { get; set; }
        public ResponseCard responseCard { get; set; }
    }
}