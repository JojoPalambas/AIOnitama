package motor;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Random;

public class Card {
    private final String name;
    private final int[][] moves;
    private static final Card[] cards = {
            new Card("+", new int[][]{
                    {0, 0, 0, 0, 0},
                    {0, 0, 1, 0, 0},
                    {0, 1, 2, 1, 0},
                    {0, 0, 1, 0, 0},
                    {0, 0, 0, 0, 0},
            }),
            new Card("x", new int[][]{
                    {0, 0, 0, 0, 0},
                    {0, 1, 0, 1, 0},
                    {0, 0, 2, 0, 0},
                    {0, 1, 0, 1, 0},
                    {0, 0, 0, 0, 0},
            }),
            new Card("|", new int[][]{
                    {0, 0, 1, 0, 0},
                    {0, 0, 1, 0, 0},
                    {0, 0, 2, 0, 0},
                    {0, 0, 1, 0, 0},
                    {0, 0, 1, 0, 0},
            }),
            new Card("-", new int[][]{
                    {0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0},
                    {1, 1, 2, 1, 1},
                    {0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0},
            }),
            new Card("m", new int[][]{
                    {0, 0, 0, 0, 0},
                    {0, 1, 0, 1, 0},
                    {1, 0, 2, 0, 1},
                    {0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0},
            }),
    };

    private Card(String name, int[][] moves) {
        this.name = name;
        this.moves = moves;
    }

    public static ArrayList<Card> Draw5() {
        ArrayList<Card> hand = new ArrayList<Card>(Arrays.asList(cards));
        ArrayList<Card> ret = new ArrayList<Card>();
        Random rand = new Random();

        // Selecting the cards to return
        for (int i = 0; i < 5; i++) {
            int index = rand.nextInt(hand.size());
            ret.add(hand.get(index));
            hand.remove(index);
        }

        return ret;
    }

    public String getName() {
        return name;
    }

    public int[][] getMoves() {
        int[][] ret = new int[5][5];

        for (int i = 0; i < ret.length; i++) {
            for (int j = 0; j < ret[i].length; j++) {
                ret[i][j] = moves[i][j];
            }
        }
        return ret;
    }
}
