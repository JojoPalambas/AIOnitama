package motor;

import javafx.util.Pair;

public class Board {
    public Piece[][] table = new Piece[5][5];
    public Pair<Card, Card> HandA;
    public Pair<Card, Card> handB;
    public Card freeCard;
}
