using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private string moveName;
    [SerializeField] private List<string> movesStrings;
    private int[][] moves;

    // Start is called before the first frame update
    void Start()
    {
        moves = new int[5][];
        for (int i = 0; i < 5; i++)
        {
            moves[i] = new int[5];
            for (int j = 0; j < 5; j++)
            {
                Debug.Log(moves[i][j]);
                if (movesStrings[i][j] == '0')
                    moves[i][j] = 0;
                else
                    moves[i][j] = 1;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public int[][] GetMoves()
    {
        int[][] ret = new int[5][];
        for (int i = 0; i < 5; i++)
        {
            ret[i] = new int[5];
            for (int j = 0; j < 5; j++)
            {
                ret[i][j] = moves[i][j];
            }
        }
        return ret;
    }

    public int[][] GetMovesReversed()
    {
        int[][] ret = new int[5][];
        for (int i = 0; i < 5; i++)
        {
            ret[i] = new int[5];
            for (int j = 0; j < 5; j++)
            {
                ret[i][j] = moves[4 - i][4 - j];
            }
        }
        return ret;
    }
}
