package AI;

import motor.Board;
import motor.Player;
import motor.Team;
import motor.TurnResponse;

public class JojoAI extends Player {
    public JojoAI(Team team) {
        this.team = team;
    }

    @Override
    public TurnResponse Play(Board boardCopy) {
        return null;
    }
}
