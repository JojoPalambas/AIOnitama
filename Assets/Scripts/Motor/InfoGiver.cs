using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InfoGiver
{
    public static Card cardA1
    {
        get { return GameManager.instance.cardA1; }
        set { return; }
    }
    public static Card cardA2
    {
        get { return GameManager.instance.cardA2; }
        set { return; }
    }
    public static Card cardB1
    {
        get { return GameManager.instance.cardB1; }
        set { return; }
    }
    public static Card cardB2
    {
        get { return GameManager.instance.cardB2; }
        set { return; }
    }
    public static Card freeCard
    {
        get { return GameManager.instance.freeCard; }
        set { return; }
    }

    public static Piece[][] table
    {
        get { return GameManager.instance.board.table; }
        set { return; }
    }
}
