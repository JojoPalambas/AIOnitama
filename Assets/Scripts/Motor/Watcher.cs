using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceState
{
    public Team team;
    public PieceType type;

    public PieceState(Piece piece)
    {
        team = piece.team;
        type = piece.type;
    }

    public PieceState(Team team, PieceType type)
    {
        this.team = team;
        this.type = type;
    }

    public bool IsEqual(PieceState pieceState)
    {
        if (team != pieceState.team || type != pieceState.type)
            return false;

        return true;
    }

    public PieceState Copy()
    {
        return new PieceState(team, type);
    }
}

public class CardState
{
    public readonly string cardName;
    public readonly int[][] moves;

    public CardState(Card card)
    {
        cardName = card.cardName;

        moves = new int[5][];
        for (int i = 0; i < 5; i++)
        {
            this.moves[i] = new int[5];
            for (int j = 0; j < 5; j++)
            {
                moves[i][j] = card.GetMoves()[i][j];
            }
        }
    }

    public CardState(string cardName, int[][] moves)
    {
        this.cardName = cardName;
        this.moves = moves;
    }

    public bool IsEqual(CardState cardState)
    {
        if (cardName != cardState.cardName)
            return false;
        
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (moves[i][j] != cardState.moves[i][j])
                    return false;
            }
        }

        return true;
    }
}

public class GameState
{
    private readonly CardState cardA1State;
    private readonly CardState cardA2State;
    private readonly CardState cardB1State;
    private readonly CardState cardB2State;
    private readonly CardState freeCardState;

    private readonly PieceState[][] tableState;

    public GameState(Card cardA1, Card cardA2, Card cardB1, Card cardB2, Card freeCard, Piece[][] table)
    {
        cardA1State = new CardState(cardA1);
        cardA2State = new CardState(cardA2);
        cardB1State = new CardState(cardB1);
        cardB2State = new CardState(cardB2);
        freeCardState = new CardState(freeCard);

        tableState = new PieceState[5][];
        for (int i = 0; i < 5; i++)
        {
            tableState[i] = new PieceState[5];
            for (int j = 0; j < 5; j++)
            {
                if (table[i][j] != null)
                    tableState[i][j] = new PieceState(table[i][j]);
                else
                    tableState[i][j] = null;
            }
        }
    }

    public bool IsEqual(GameState gameState)
    {
        if (
            !cardA1State.IsEqual(gameState.cardA1State) ||
            !cardA2State.IsEqual(gameState.cardA2State) ||
            !cardB1State.IsEqual(gameState.cardB1State) ||
            !cardB2State.IsEqual(gameState.cardB2State) ||
            !freeCardState.IsEqual(gameState.freeCardState)
        )
            return false;

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if ((tableState[i][j] == null && gameState.tableState[i][j] != null) || (tableState[i][j] != null && gameState.tableState[i][j] == null))
                    return false;
                if (!tableState[i][j].IsEqual(gameState.tableState[i][j]))
                    return false;
            }
        }

        return true;
    }
}

public class Watcher : MonoBehaviour
{
    private List<GameState> history;

    public void Start()
    {
        history = new List<GameState>();
    }

    void Save()
    {
        history.Add(new GameState(
            GameManager.instance.cardA1,
            GameManager.instance.cardA2,
            GameManager.instance.cardB1,
            GameManager.instance.cardB2,
            GameManager.instance.freeCard,
            GameManager.instance.board.table
        ));
    }

    public bool CheckSanity()
    {
        if (history.Count == 0)
            return true;

        if (!history[history.Count - 1].IsEqual(new GameState(
            GameManager.instance.cardA1,
            GameManager.instance.cardA2,
            GameManager.instance.cardB1,
            GameManager.instance.cardB2,
            GameManager.instance.freeCard,
            GameManager.instance.board.table
        )))
            return false;

        return true;
    }
}
