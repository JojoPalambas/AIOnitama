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

    // Makes the list of all the possible turns, then picks up a random one
    public override TurnResponse PlayTurn()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
            }
        }

        return new TurnResponse("Elephant", new Vector2Int(0, 0), new Vector2Int(1, 1));
    }
}
