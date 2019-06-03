package motor;

import javafx.util.Pair;

public class TurnResponse {
    public final Card card;
    public final Piece piece;
    public final int[] destination;

    public TurnResponse(Card card, Piece piece, int[] destination) {
        this.card = card;
        this.piece = piece;
        this.destination = destination;
    }
}
