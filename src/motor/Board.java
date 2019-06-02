package motor;

import javafx.util.Pair;

public class Board {
    public Piece[][] table = new Piece[5][5];
    public Pair<Card, Card> HandA;
    public Pair<Card, Card> handB;
    public Card freeCard;

    public Team HasWon() {
        boolean kingAFound = false;
        boolean kingBFound = false;

        for (int i = 0; i < table.length; i++) {
            Piece[] line = table[i];
            for (int j = 0; j < line.length; j++) {
                Piece piece = line[j];
                if (piece == null)
                    continue;

                // Checks if a piece occupies the opponent's base
                if (i == 0 && j == 2 && piece.team == Team.B)
                    return Team.B;
                if (i == 4 && j == 2 && piece.team == Team.A)
                    return Team.A;

                // Checks if the piece is a king
                if (piece.type == PieceType.king) {
                    if (piece.team == Team.A)
                        kingAFound = true;
                    else if (piece.team == Team.B)
                        kingBFound = true;
                }

                if (kingAFound && kingBFound)
                    break;
            }
        }

        // Checks if both kings are still in game
        if (!kingAFound)
            return Team.B;
        if (!kingBFound)
            return Team.A;

        return Team.none;
    }
}
