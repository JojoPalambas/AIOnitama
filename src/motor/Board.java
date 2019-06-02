package motor;

import javafx.util.Pair;

import java.util.ArrayList;

public class Board {
    public Piece[][] table = new Piece[5][5];
    public Pair<Card, Card> handA;
    public Pair<Card, Card> handB;
    public Card freeCard;

    public Board() {
        table = new Piece[5][5];
        table[0][0] = new Piece(PieceType.monk, Team.A);
        table[0][1] = new Piece(PieceType.monk, Team.A);
        table[0][2] = new Piece(PieceType.king, Team.A);
        table[0][3] = new Piece(PieceType.monk, Team.A);
        table[0][4] = new Piece(PieceType.monk, Team.A);
        table[1][0] = new Piece(PieceType.monk, Team.B);
        table[1][1] = new Piece(PieceType.monk, Team.B);
        table[1][2] = new Piece(PieceType.king, Team.B);
        table[1][3] = new Piece(PieceType.monk, Team.B);
        table[1][4] = new Piece(PieceType.monk, Team.B);

        ArrayList<Card> cards = Card.Draw5();
        handA = new Pair<>(cards.get(0), cards.get(1));
        handB = new Pair<>(cards.get(2), cards.get(3));
        freeCard = cards.get(4);
    }

    public Board(Piece[][] table, Pair<Card, Card> handA, Pair<Card, Card> handB, Card freeCard) {
        this.table = table;
        this.handA = handA;
        this.handB = handB;
        this.freeCard = freeCard;
    }

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

    public Board deepCopy() {
        Piece[][] tableCopy = new Piece[5][5];
        for (int i = 0; i < tableCopy.length; i++)
            for (int j = 0; j < tableCopy[i].length; j++)
                tableCopy[i][j] = table[i][j];

        Pair<Card, Card> handACopy = new Pair<>(handA.getKey(), handA.getValue());
        Pair<Card, Card> handBCopy = new Pair<>(handB.getKey(), handB.getValue());
        Card freeCardCopy = freeCard;

        return new Board(tableCopy, handACopy, handBCopy, freeCardCopy);
    }
}
