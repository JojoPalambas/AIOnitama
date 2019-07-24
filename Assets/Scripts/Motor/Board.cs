using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board instance;

    public Piece[][] table;

    public GameObject AKPrefab;
    public GameObject AMPrefab;
    public GameObject BKPrefab;
    public GameObject BMPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        InitTable();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void InitTable()
    {
        // Creating the empty table
        table = new Piece[5][];

        // Managing the starting pieces
        Piece newPiece = null;
        for (int i = 0; i < 5; i++)
        {
            table[i] = new Piece[5];

            if (i == 2)
            {
                newPiece = Instantiate(AKPrefab, transform).GetComponent<Piece>();
                newPiece.SetPosition(new Vector2Int(i, 0));
                table[i][0] = newPiece;

                newPiece = Instantiate(BKPrefab, transform).GetComponent<Piece>();
                newPiece.SetPosition(new Vector2Int(i, 4));
                table[i][4] = newPiece;
            }
            else
            {
                newPiece = Instantiate(AMPrefab, transform).GetComponent<Piece>();
                newPiece.SetPosition(new Vector2Int(i, 0));
                table[i][0] = newPiece;

                newPiece = Instantiate(BMPrefab, transform).GetComponent<Piece>();
                newPiece.SetPosition(new Vector2Int(i, 4));
                table[i][4] = newPiece;
            }
        }
    }

    private void MovePiece(Vector2Int source, Vector2Int destination)
    {
        // Moving the pieces in the table
        Piece tmp = table[source.x][source.y];
        table[source.x][source.y] = table[destination.x][destination.y];
        table[destination.x][destination.y] = tmp;

        // Physically moving the pieces
        if (table[source.x][source.y] != null)
            table[source.x][source.y].SetPosition(new Vector2Int(source.x, source.y));

        if (table[destination.x][destination.y] != null)
            table[destination.x][destination.y].SetPosition(new Vector2Int(destination.x, destination.y));
    }

    public void ApplyTurn(TurnResponse turn)
    {
        MovePiece(turn.source, turn.destination);
    }
}
