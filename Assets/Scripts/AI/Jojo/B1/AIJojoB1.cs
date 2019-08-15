using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIJojoB1 : AI
{
    private Team team;

    public override void Init(Team team)
    {
        this.team = team;
        return;
    }
    
    public override TurnResponse PlayTurn()
    {
        BoardState board = InfoGiver.board;

        System.Tuple<TurnResponse, TurnResponse> t = null;

        return DeepAnalysis(board, team, 2).Item1;
    }

    // FIXME Essayer en retournant la bestPositivity, la worstPositivity et la moyenne
    // Does a depth-first traversal of all the possibilities (with a max depth) to get the best path
    private System.Tuple<TurnResponse, float> DeepAnalysis(BoardState board, Team team, int depth)
    {
        Team winner = InfoGiver.HasGameEnded(board.table);
        if (winner != Team.none)
            return new System.Tuple<TurnResponse, float>(null, winner == team ? 1 : 0);

        List<TurnResponse> possibleTurns = GetAllTurns(board, team);

        List<System.Tuple<TurnResponse, float>> bestTurns = new List<System.Tuple<TurnResponse, float>>();
        float bestPositivity = 0;
        foreach (TurnResponse turn in possibleTurns)
        {
            BoardState newBoard = InfoGiver.ApplyTurn(board, turn);
            float positivity = 0;

            if (depth <= 0)
                positivity = LightAnalysis(newBoard.table);
            else
            {
                System.Tuple<TurnResponse, float> recursiveResponse = DeepAnalysis(newBoard, team == Team.A ? Team.B : Team.A, depth - 1);
                positivity = 1 - recursiveResponse.Item2;
            }

            if (bestPositivity < positivity)
            {
                bestTurns = new List<System.Tuple<TurnResponse, float>>();
                bestTurns.Add(new System.Tuple<TurnResponse, float>(turn, positivity));
                bestPositivity = positivity;
            }
            else if (bestPositivity == positivity)
                bestTurns.Add(new System.Tuple<TurnResponse, float>(turn, positivity));
        }

        if (bestTurns.Count == 0)
            return null;

        int randomIndex = Random.Range(0, bestTurns.Count);

        return bestTurns[randomIndex];
    }

    // Analyses a given situation and returns its "positivity"
    // Game lost -> 0
    // Game won -> 1
    // Other cases -> Proportion of allied pieces in all the pieces in game
    private float LightAnalysis(PieceState[][] table)
    {
        Team winner = InfoGiver.HasGameEnded(table);
        if (winner == team)
            return 1;

        if (winner == Team.none)
        {
            int allies = 0;
            int total = 0;

            // Counting the allied pieces and all the pieces
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (table[i][j] != null)
                    {
                        total += 1;
                        if (table[i][j].team == team)
                            allies += 1;
                    }
                }
            }

            return (float) allies / (float) total;
        }

        return 0;
    }

    private List<TurnResponse> GetAllTurns(BoardState board, Team team)
    {
        List<TurnResponse> possibleTurns = new List<TurnResponse>();

        PieceState[][] table = board.table;

        // Making the list of all the moves allowed by card 1;
        Card card1 = team == Team.A ? board.cardA1 : board.cardB1;
        int[][] card1MovesTable = team == Team.A ? card1.GetMoves() : card1.GetMovesReversed();
        List<Vector2Int> card1Moves = new List<Vector2Int>();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (card1MovesTable[i][j] == 1)
                    card1Moves.Add(new Vector2Int(i, j));
            }
        }

        // Making the list of all the moves allowed by card 2;
        Card card2 = team == Team.A ? board.cardA2 : board.cardB2;
        int[][] card2MovesTable = team == Team.A ? card2.GetMoves() : card2.GetMovesReversed();
        List<Vector2Int> card2Moves = new List<Vector2Int>();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (card2MovesTable[i][j] == 1)
                    card2Moves.Add(new Vector2Int(i, j));
            }
        }

        // Going through the table of the board to find allied pieces and check their possible moves
        TurnResponse tr = null;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (table[i][j] != null && table[i][j].team == team)
                {
                    // Going through card1 moves and trying them all
                    foreach (Vector2Int vect in card1Moves)
                    {
                        // At the end of the line, the "-2"s are added to center vect on (2, 2)
                        tr = new TurnResponse(card1.cardName, new Vector2Int(i, j), new Vector2Int(i - 2, j - 2) + vect);
                        if (InfoGiver.IsTurnValid(table, card1, card2, team, tr))
                            possibleTurns.Add(tr);
                    }
                    // Going through card2 moves and trying them all
                    foreach (Vector2Int vect in card2Moves)
                    {
                        // At the end of the line, the "-2"s are added to center vect on (2, 2)
                        tr = new TurnResponse(card2.cardName, new Vector2Int(i, j), new Vector2Int(i - 2, j - 2) + vect);
                        if (InfoGiver.IsTurnValid(table, card1, card2, team, tr))
                            possibleTurns.Add(tr);
                    }
                }
            }
        }

        return possibleTurns;
    }

    public override string name
    {
        get { return "AI Jojo B1"; }
    }
}
