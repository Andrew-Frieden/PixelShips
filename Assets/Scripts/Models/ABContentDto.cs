using System;

namespace Models
{
    public class ABContentDto
    {
        public string MainText;
        public string OptionAText;
        public string OptionBText;

        public SimpleActionDto OptionAActionSimple;
        public SimpleActionDto OptionBActionSimple;
        
        // public ComplexActionDto OptionAActionComplex;
        // public ComplexActionDto OptionBActionComplex;
        
        public void AddSimpleAction(SimpleActionDto action)
        {
            if (OptionAActionSimple == null)
            {
                OptionAActionSimple = action;
            }
            else if (OptionBActionSimple == null)
            {
                OptionBActionSimple = action;
            }
            throw new Exception("ABContentDto.AddSimpleAction() => both actions already set.");
        }
    }
}
