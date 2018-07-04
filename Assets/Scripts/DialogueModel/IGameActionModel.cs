using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameActionModel {

    void resolve();
    bool validate();
	
}

public class shipDamage : IGameActionModel
{

    public void resolve()
    {

    }

    public bool validate()
    {
        return true;
    }
}