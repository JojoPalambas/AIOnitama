using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InfoGiver
{
    // GETTERS / SETTERS

    public static Card cardA1
    {
        get { return GameManager.instance.cardA1; }
        set { return; }
    }
    public static Card cardA2
    {
        get { return GameManager.instance.cardA2; }
        set { return; }
    }
    public static Card cardB1
    {
        get { return GameManager.instance.cardB1; }
        set { return; }
    }
    public static Card cardB2
    {
        get { return GameManager.instance.cardB2; }
        set { return; }
    }
    public static Card freeCard
    {
        get { return GameManager.instance.freeCard; }
        set { return; }
    }

    public static PieceState[][] table
    {
        get { return PieceTableToPieceStateTable(GameManager.instance.board.table); }
        set { return; }
    }

    // UTILS

    // Returns true if and only if the described turn is valid
    public static bool IsTurnValid(PieceState[][] table, Card card1, Card card2, Team team, TurnResponse turn)
    {
        if (turn == null)
            return false;
        if (team == Team.none)
            return false;


        // The source and the destination have to be in the bounds of the map
        if (turn.source.x < 0 || turn.source.x >= 5 || turn.source.y < 0 || turn.source.y >= 5)
            return false;
        if (turn.destination.x < 0 || turn.destination.x >= 5 || turn.destination.y < 0 || turn.destination.y >= 5)
            return false;

        // The source must contain a movable Piece
        if (table[turn.source.x][turn.source.y] == null || table[turn.source.x][turn.source.y].team != team)
            return false;

        // The destination must not contain a movable Piece
        if (table[turn.destination.x][turn.destination.y] != null && table[turn.destination.x][turn.destination.y].team == team)
            return false;

        // The player must own the designated card
        Card playedCard = null;
        if (card1.cardName == turn.cardName)
            playedCard = card1;
        if (card2.cardName == turn.cardName)
            playedCard = card2;

        if (playedCard == null)
            return false;

        // The move must be allowed by the designated card
        Vector2Int moveVector = turn.destination - turn.source;
        if (moveVector.x < -2 || moveVector.x >= 3 || moveVector.y < -2 || moveVector.y >= 3)
            return false;
        if (team == Team.A && playedCard.GetMoves()[moveVector.x + 2][moveVector.y + 2] == 0)
            return false;
        if (team == Team.B && playedCard.GetMovesReversed()[moveVector.x + 2][moveVector.y + 2] == 0)
            return false;

        return true;
    }

    // Returns Team.none if the described game has not ended, the winner in other cases
    public static Team HasGameEnded(PieceState[][] table)
    {
        // One of the players have lost their king
        bool kingAFound = false;
        bool kingBFound = false;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (table[i][j] != null && table[i][j].type == PieceType.king)
                {
                    if (table[i][j].team == Team.A)
                        kingAFound = true;
                    if (table[i][j].team == Team.B)
                        kingBFound = true;
                }
            }
        }
        if (!kingAFound)
        {
            return Team.B;
        }
        if (!kingBFound)
        {
            return Team.B;
        }

        // One of the player's pieces is on the other player's throne
        if (table[2][0] != null && table[2][0].team == Team.B)
        {
            return Team.B;
        }
        if (table[2][4] != null && table[2][4].team == Team.A)
        {
            return Team.B;
        }

        return Team.none;
    }

    // CONVERTERS

    // Converts a Piece[][] to a PieceState[][] (therefore also makes a copy of it)
    public static PieceState[][] PieceTableToPieceStateTable(Piece[][] table)
    {
        PieceState[][] ret = new PieceState[5][];

        for (int i = 0; i < 5; i++)
        {
            ret[i] = new PieceState[5];
            for (int j = 0; j < 5; j++)
            {
                if (table[i][j] == null)
                    ret[i][j] = null;
                else
                    ret[i][j] = new PieceState(table[i][j]);
            }
        }

        return ret;
    }
}
