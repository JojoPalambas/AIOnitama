using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class AIPuppet : AI
{
    private Team team;

    private AIGOPuppet master;

    public AIPuppet(AIGOPuppet master)
    {
        this.master = master;
    }

    public override void Init(Team team)
    {
        this.team = team;
        return;
    }

    public override TurnResponse PlayTurn()
    {
        return new TurnResponse(master.cardName, master.moveSource, master.moveDestination);
    }

    public override string name
    {
        get { return "Puppet"; }
    }
}
