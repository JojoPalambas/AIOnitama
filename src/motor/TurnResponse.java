package motor;

import javafx.util.Pair;

public class TurnResponse {
    public final String cardName;
    public final Piece piece;
    public final Pair<Integer, Integer> destination;

    public TurnResponse(String cardName, Piece piece, Pair<Integer, Integer> destination) {
        this.cardName = cardName;
        this.piece = piece;
        this.destination = destination;
    }
}
