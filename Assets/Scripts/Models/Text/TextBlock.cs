using System;

namespace Models.Text
{
    public class TextBlock
    {
        private string _text;
        public string EntityId { get; }

        public TextBlock(string text, string entityId)
        {
            _text = text;
            EntityId = entityId;
        }
        
        //For Debugging
        public override string ToString()
        {
            return _text;
        }
    }
}