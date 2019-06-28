package motor;

import javafx.util.Pair;

public class TurnResponse {
    public final String cardName;
    public final Piece piece;
    public final int[] destination;

    public TurnResponse(String cardName, Piece piece, int[] destination) {
        this.cardName = cardName;
        this.piece = piece;
        this.destination = destination;
    }
}
