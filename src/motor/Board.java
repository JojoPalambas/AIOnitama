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
        table[4][0] = new Piece(PieceType.monk, Team.B);
        table[4][1] = new Piece(PieceType.monk, Team.B);
        table[4][2] = new Piece(PieceType.king, Team.B);
        table[4][3] = new Piece(PieceType.monk, Team.B);
        table[4][4] = new Piece(PieceType.monk, Team.B);

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

    // True if the turn has been played, false if it was invalid
    public boolean ApplyTurn(TurnResponse turn, Team team) {
        int x = 0;
        int y = 0;
        Piece pieceToMove = null;

        // Checks if the piece to move exists
        // FIXME For fuck's sake, rewrite this shit
        for (int i = 0; i < table.length; i++) {
            for (int j = 0; j < table[i].length; j++) {
                if (table[i][j] == turn.piece) {
                    x = i;
                    y = j;
                    pieceToMove = table[i][j];
                    break;
                }
            }
            if (pieceToMove != null)
                break;
        }
        if (pieceToMove == null)
            return false;

        // Checks if the card is available to this player
        if ((team == Team.A && turn.card != handA.getKey() && turn.card != handA.getValue())
                || ((team == Team.B && turn.card != handB.getKey() && turn.card != handB.getValue())))
            return false;

        int[] vect = {
                turn.destination[0] - x,
                turn.destination[1] - y,
        };

        // Checks if the move actually fits in a 5x5 square around the piece
        if (vect[0] + 2 < 0 || vect[0] + 2 > 4 || vect[1] + 2 < 0 || vect[1] + 2 >4)
            return false;

        // Checks if the movement is allowed by the card
        if (turn.card.GetMoves()[vect[0] + 2][vect[1] + 2] != 1)
            return false;

        // Checks if the movement leads to an existing cell
        if (turn.destination[0] < 0 || turn.destination[0] > 4 || turn.destination[1] < 0 || turn.destination[1] > 4)
            return false;

        // Makes the move
        table[turn.destination[0]][turn.destination[1]] = table[x][y];
        table[x][y] = null;

        return true;
    }

    public Board DeepCopy() {
        Piece[][] tableCopy = new Piece[5][5];
        for (int i = 0; i < tableCopy.length; i++)
            for (int j = 0; j < tableCopy[i].length; j++)
                tableCopy[i][j] = table[i][j];

        Pair<Card, Card> handACopy = new Pair<>(handA.getKey(), handA.getValue());
        Pair<Card, Card> handBCopy = new Pair<>(handB.getKey(), handB.getValue());
        Card freeCardCopy = freeCard;

        return new Board(tableCopy, handACopy, handBCopy, freeCardCopy);
    }

    public String toString() {
        StringBuilder ret = new StringBuilder();

        // Adds the table
        for (int k = 0; k < table[0].length * 5 + 1; k++) {
            ret.append("-");
        }
        ret.append("\n");
        for (int i = 0; i < table.length; i++) {
            ret.append("| ");
            for (int j = 0; j < table[i].length; j++) {
                if (table[i][j] == null) {
                    ret.append("  ");
                }
                else {
                    ret.append(table[i][j].team == Team.A ? "a" : "b");
                    ret.append(table[i][j].type == PieceType.monk ? "M" : "K");
                }
                ret.append(" | ");
            }
            ret.append("\n");
            for (int k = 0; k < table[0].length * 5 + 1; k++) {
                ret.append("-");
            }
            ret.append("\n");
        }
        ret.append("\n");

        ret.append("Team A cards: ").append(handA.getKey().GetName()).append(" | ").append(handA.getValue().GetName()).append("\n");
        ret.append("Team B cards: ").append(handB.getKey().GetName()).append(" | ").append(handB.getValue().GetName()).append("\n");
        ret.append("Free card: ").append(freeCard.GetName());
        ret.append("\n");

        return ret.toString();
    }
}
