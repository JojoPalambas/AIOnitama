package AI;

import motor.Board;
import motor.Player;
import motor.Team;
import motor.TurnResponse;

public class RealPlayer extends Player {
    public RealPlayer(Team team) {
    }

    @Override
    public TurnResponse Play(Board boardCopy) {
        //System.out.println(boardCopy.toString());

        return null;
    }
}
