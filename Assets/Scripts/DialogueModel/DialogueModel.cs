using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueModel {

    public string bodyText { get; }
    public string optionAText { get; }
    public string optionBText { get; }
    public int id { get; }
    public Dictionary<string,string> optionAEffects { get; }
    public Dictionary<string, string> optionBEffects { get; }
    public DialogueType type { get; }

    public DialogueModel(string bodyText, string optionAText, string optionBText, int id, Dictionary<string, string> optionAEffects, Dictionary<string, string> optionBEffects, DialogueType type)
    {
        this.bodyText = bodyText;
        this.optionAText = optionAText;
        this.optionBText = optionBText;
        this.id = id;
        this.optionAEffects = optionAEffects;
        this.optionBEffects = optionBEffects;
        this.type = type;
    }

}

public enum DialogueType
{
    DEATH, RETURNHOME, COMBAT, DISCOVERY, ENTERSYSTEM, EXITSYSTEM
}
