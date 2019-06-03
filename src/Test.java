import motor.Board;
import motor.PieceType;
import motor.Team;

import java.util.ArrayList;

public class Test {
    private static ArrayList<String> successes = new ArrayList<>();
    private static ArrayList<String> fails = new ArrayList<>();

    private static void Fail(String s) {
        System.out.println("[KO] " + s);
        fails.add(s);
    }

    private static void Success(String s) {
        System.out.println("  [OK] " + s);
        successes.add(s);
    }

    public static void main(String[] args) {
        System.out.println("========== TESTS\n");

        System.out.println("---------- Board\n");

        System.out.println("\n----- Board creation: Board content\n");
        {
            Board board = new Board();

            if (board.table.length == 5)
                Success("Board height");
            else
                Fail("Board height");

            for (int i = 0; i < board.table.length; i++) {
                if (board.table[i].length == 5)
                    Success("Board width");
                else
                    Fail("Board width");
                for (int j = 0; j < board.table[i].length; j++) {
                    if (i == 0) {
                        if (j == 2) {
                            if (board.table[i][j] != null && board.table[i][j].team == Team.A && board.table[i][j].type == PieceType.king)
                                Success("Cell (" + i + ", " + j + ")");
                            else
                                Fail("Cell (" + i + ", " + j + ")");
                        }
                        else {
                            if (board.table[i][j] != null && board.table[i][j].team == Team.A && board.table[i][j].type == PieceType.monk)
                                Success("Cell (" + i + ", " + j + ")");
                            else
                                Fail("Cell (" + i + ", " + j + ")");
                        }
                    }
                    else if (i == 4) {
                        if (j == 2) {
                            if (board.table[i][j] != null && board.table[i][j].team == Team.B && board.table[i][j].type == PieceType.king)
                                Success("Cell (" + i + ", " + j + ")");
                            else
                                Fail("Cell (" + i + ", " + j + ")");
                        }
                        else {
                            if (board.table[i][j] != null && board.table[i][j].team == Team.B && board.table[i][j].type == PieceType.monk)
                                Success("Cell (" + i + ", " + j + ")");
                            else
                                Fail("Cell (" + i + ", " + j + ")");
                        }
                    }
                    else {
                        if (board.table[i][j] == null)
                            Success("Cell (" + i + ", " + j + ")");
                        else
                            Fail("Cell (" + i + ", " + j + ")");
                    }
                }
            }
        }

        System.out.println("\n----- Board creation: Cards\n");
        {
            Board board = new Board();

            if (board.handA.getKey() != null && board.handA.getValue() != null)
                Success("Hand A");
            else
                Fail("Hand A");

            if (board.handB.getKey() != null && board.handB.getValue() != null)
                Success("Hand B");
            else
                Fail("Hand B");

            if (board.freeCard != null)
                Success("Free card");
            else
                Fail("Free Card");
        }

        System.out.println("\n\n");
        System.out.println("Successes: " + successes.size());
        System.out.println("Fails: " + fails.size());
    }
}
