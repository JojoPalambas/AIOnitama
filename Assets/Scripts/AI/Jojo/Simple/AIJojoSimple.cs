using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIJojoSimple : AI
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
        List<TurnResponse> possibleTurns = GetAllTurns();
        Debug.Log(possibleTurns.Count);

        if (possibleTurns.Count == 0)
            return null;

        int rand = Random.Range(0, possibleTurns.Count);

        return possibleTurns[rand];
    }

    private List<TurnResponse> GetAllTurns()
    {
        List<TurnResponse> possibleTurns = new List<TurnResponse>();

        PieceState[][] table = InfoGiver.table;

        // Iterates over all the table twice to find all the possible turns (yes this is disgusting, but done in 1 second)
        TurnResponse tr = null;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                for (int k = 0; k < 5; k++)
                {
                    for (int l = 0; l < 5; l++)
                    {
                        if (team == Team.A)
                        {
                            tr = new TurnResponse(InfoGiver.cardA1.cardName, new Vector2Int(i, j), new Vector2Int(k, l));
                            if (InfoGiver.IsTurnValid(table, InfoGiver.cardA1, InfoGiver.cardA2, Team.A, tr))
                                possibleTurns.Add(tr);

                            tr = new TurnResponse(InfoGiver.cardA2.cardName, new Vector2Int(i, j), new Vector2Int(k, l));
                            if (InfoGiver.IsTurnValid(table, InfoGiver.cardA1, InfoGiver.cardA2, Team.A, tr))
                                possibleTurns.Add(tr);
                        }
                        if (team == Team.B)
                        {
                            tr = new TurnResponse(InfoGiver.cardB1.cardName, new Vector2Int(i, j), new Vector2Int(k, l));
                            if (InfoGiver.IsTurnValid(table, InfoGiver.cardB1, InfoGiver.cardB2, Team.B, tr))
                                possibleTurns.Add(tr);

                            tr = new TurnResponse(InfoGiver.cardB2.cardName, new Vector2Int(i, j), new Vector2Int(k, l));
                            if (InfoGiver.IsTurnValid(table, InfoGiver.cardB1, InfoGiver.cardB2, Team.B, tr))
                                possibleTurns.Add(tr);
                        }
                    }
                }
            }
        }

        return possibleTurns;
    }
}
