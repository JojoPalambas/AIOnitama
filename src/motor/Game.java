package motor;

import AI.CoucAI;
import AI.JojoAI;

import java.util.Random;

public class Game {
    private Player playerA;
    private Player playerB;
    private Board board;

    public Game() {
        Random rand = new Random();
        if (rand.nextBoolean()) {
            playerA = new CoucAI(Team.A);
            playerB = new JojoAI(Team.B);
        }
        else {
            playerA = new JojoAI(Team.A);
            playerB = new CoucAI(Team.B);
        }
        board = new Board();

        while(board.HasWon() == Team.none) {
            // Play
        }
        System.out.println(board.HasWon());
    }
}
