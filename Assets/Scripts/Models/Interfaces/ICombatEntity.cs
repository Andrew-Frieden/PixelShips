using System.Collections.Generic;

namespace Models
{
    public interface ICombatEntity : ITextEntity
    {
        int Hull { get; set; }
    }
}