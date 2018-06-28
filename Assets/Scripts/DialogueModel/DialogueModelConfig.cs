using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueModelConfig {

    private List<DialogueModel> dialogueModels;

	public DialogueModelConfig()
    {
        this.dialogueModels.Add(new DialogueModel("You made it back to base.You earned [_BOOTY] Booty.",
            "Upgrade Ship [Costs _UPGRADECOST]",
            "Set sail for space plunder!",1, 
            new Dictionary<string, string> {
                { "type", "UPGRADE" }
            }, new Dictionary<string, string> {
                { "type", "ADVENTURE" }
            }, DialogueType.RETURNHOME));

        //Add more options here. (come up with a script to add them)
    }

    public IEnumerable<DialogueModel> GetDialogueModels()
    {
        IEnumerable<DialogueModel> model = this.dialogueModels;

        return model;
    }
}
