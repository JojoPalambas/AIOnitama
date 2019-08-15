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
                            tr = new TurnResponse(board.cardA1.cardName, new Vector2Int(i, j), new Vector2Int(k, l));
                            if (InfoGiver.IsTurnValid(table, board.cardA1, board.cardA2, Team.A, tr))
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

    public override string name
    {
        get { return "AI Jojo B1"; }
    }
}
