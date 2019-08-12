using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnResponse
{
    public string cardName;

    public Vector2Int source;
    public Vector2Int destination;

    public TurnResponse(string cardName, Vector2Int source, Vector2Int destination)
    {
        this.cardName = cardName;
        this.source = source;
        this.destination = destination;
    }
}

public abstract class AI
{
    public abstract string name { get; }

    public abstract void Init(Team team);
    public abstract TurnResponse PlayTurn();
}
