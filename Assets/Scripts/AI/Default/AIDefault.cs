using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDefault : AI
{
    private Team team;

    public override void Init(Team team)
    {
        this.team = team;
        return;
    }

    public override TurnResponse PlayTurn()
    {
        return new TurnResponse("Elephant", new Vector2Int(0, 0), new Vector2Int(1, 1));
    }
}
