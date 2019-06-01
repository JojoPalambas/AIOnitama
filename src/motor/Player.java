package motor;

import javafx.util.Pair;

public abstract class Player {
    public TurnResponse Play() {
        return new TurnResponse("Nothing", new Piece(PieceType.monk, Team.A), new Pair<Integer, Integer>(0, 0));
    }
}
