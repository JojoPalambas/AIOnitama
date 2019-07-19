using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnResponse
{
    public string cardName;

    public Vector2Int source;
    public Vector2Int destination;
}

public abstract class AI
{
    public abstract void Init(Team team);
    public abstract TurnResponse PlayTurn();
}
