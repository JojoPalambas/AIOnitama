package motor;

import AI.CoucAI;
import AI.JojoAI;
import AI.RealPlayer;

import java.util.Random;

public class Game {

    public Game() {
        Random rand = new Random();
        Player playerA;
        Player playerB;

        if (rand.nextBoolean()) {
            playerA = new RealPlayer(Team.A);
            playerB = new JojoAI(Team.B);
        }
        else {
            playerA = new JojoAI(Team.A);
            playerB = new RealPlayer(Team.B);
        }
        Board board = new Board();

        boolean teamATurn = true;
        while(board.HasWon() == Team.none) {
            TurnResponse turn = null;
            if (teamATurn)
                turn = playerA.Play(board.DeepCopy());
            else
                turn = playerB.Play(board.DeepCopy());
        }
        System.out.println(board.toString());
        System.out.println(board.HasWon());
    }
}
