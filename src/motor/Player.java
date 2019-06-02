package motor;

public abstract class Player {
    protected Team team;

    public abstract TurnResponse Play(Board boardCopy);

    public Team getTeam() {
        return team;
    }
}
