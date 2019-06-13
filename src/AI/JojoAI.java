package AI;

import motor.Board;
import motor.Player;
import motor.Team;
import motor.TurnResponse;

public class JojoAI extends Player {
    private Team team;

    public JojoAI(Team team) {
        this.team = team;
    }

    @Override
    public TurnResponse Play(Board boardCopy) {
        // Rotate the board if needed, so that the player to play is always on top

        // Rotate the board back if needed

        return null;
    }
}
