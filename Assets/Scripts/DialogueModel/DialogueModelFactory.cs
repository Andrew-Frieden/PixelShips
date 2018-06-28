using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueModelFactory {

    private DialogueModelConfig dialogueModelConfig;

    public DialogueModelFactory()
    {

    }

    public DialogueModel getDialogue(DialogueType type)
    {
        IEnumerable<DialogueModel> models = from m in this.dialogueModelConfig.GetDialogueModels()
                                            where m.type == type
                                            orderby Random.value
                                            select m;

        return models.FirstOrDefault();
    }
}
