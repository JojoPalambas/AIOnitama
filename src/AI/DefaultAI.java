package AI;

import motor.*;

public class DefaultAI extends Player {
    private Team team;

    public DefaultAI(Team team) {
        this.team = team;
    }

    public Board flipIfNeeded(Board board) {
        if (team == Team.A)
            return board;

        Piece[][] table = new Piece[5][5];
        for (int i = 0; i < table.length; i++) {
            for (int j = 0; j< table[i].length; j++) {
                table[i][j] = board.table[4 - i][4 - j];
            }
        }

        Card card1 = null;
        Card card2 = null;
    }

    @Override
    public TurnResponse Play(Board boardCopy) {
        // Flips the board and the cards if player B

        // Finds the first valid move and plays it

        return null;
    }
}
