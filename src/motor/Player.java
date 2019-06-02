package motor;

import javafx.util.Pair;

public abstract class Player {
    private Team team;

    public abstract TurnResponse Play(Board boardCopy);

    public Team getTeam() {
        return team;
    }
}
