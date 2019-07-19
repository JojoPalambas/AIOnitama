using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDefault : AI
{
    public override void Init(Team team)
    {
        Debug.Log(team);
        return;
    }

    public override TurnResponse PlayTurn()
    {
        return null;
    }
}
