using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState
{
    public PieceState[][] table;

    public CardState cardA1;
    public CardState cardA2;
    public CardState cardB1;
    public CardState cardB2;
    public CardState freeCard;

    public Team currentTeam;

    public BoardState(PieceState[][] table, CardState cardA1, CardState cardA2, CardState cardB1, CardState cardB2, CardState freeCard, Team currentTeam)
    {
        this.table = table;

        this.cardA1 = cardA1;
        this.cardA2 = cardA2;
        this.cardB1 = cardB1;
        this.cardB2 = cardB2;
        this.freeCard = freeCard;

        this.currentTeam = currentTeam;
    }

    public BoardState()
    {
        this.table = InfoGiver.PieceTableToPieceStateTable(Board.instance.table);

        this.cardA1 = new CardState(GameManager.instance.cardA1);
        this.cardA2 = new CardState(GameManager.instance.cardA2);
        this.cardB1 = new CardState(GameManager.instance.cardB1);
        this.cardB2 = new CardState(GameManager.instance.cardB2);
        this.freeCard = new CardState(GameManager.instance.freeCard);

        this.currentTeam = GameManager.instance.currentPlayer;
    }

    // Creates a deep copy of the BoardState (the copy is only deep on the table, not the cards)
    public BoardState DeepCopy()
    {
        PieceState[][] newTable = new PieceState[5][];
        for (int i = 0; i < 5; i++)
        {
            newTable[i] = new PieceState[5];
            for (int j = 0; j < 5; j++)
            {
                newTable[i][j] = table[i][j].Copy();
            }
        }

        return new BoardState(newTable, cardA1, cardA2, cardB1, cardB2, freeCard, currentTeam);
    }
}

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

    public bool ValidateTurn(TurnResponse turn, Team team)
    {
        if (turn == null)
        {
            GameManager.instance.EndGame(team == Team.A ? Team.B : Team.A, "No response given");
            return false;
        }
        if (team == Team.none)
        {
            GameManager.instance.EndGame(team == Team.A ? Team.B : Team.A, "Invalid team");
            return false;
        }

        // The source and the destination have to be in the bounds of the map
        if (turn.source.x < 0 || turn.source.x >= 5 || turn.source.y < 0 || turn.source.y >= 5)
        {
            GameManager.instance.EndGame(team == Team.A ? Team.B : Team.A, "The move source is out of bounds");
            return false;
        }
        if (turn.destination.x < 0 || turn.destination.x >= 5 || turn.destination.y < 0 || turn.destination.y >= 5)
        {
            GameManager.instance.EndGame(team == Team.A ? Team.B : Team.A, "The move destination is out of bounds");
            return false;
        }

        // The source must contain a movable Piece
        if (table[turn.source.x][turn.source.y] == null || table[turn.source.x][turn.source.y].team != team)
        {
            GameManager.instance.EndGame(team == Team.A ? Team.B : Team.A, "The move source does not contain a movable piece");
            return false;
        }

        // The destination must not contain a movable Piece
        if (table[turn.destination.x][turn.destination.y] != null && table[turn.destination.x][turn.destination.y].team == team)
        {
            Debug.Log(turn.destination.x.ToString() + " - " + turn.destination.y.ToString());
            Debug.Log(table[turn.destination.x][turn.destination.y].type);
            Debug.Log(table[turn.destination.x][turn.destination.y].team);

            GameManager.instance.EndGame(team == Team.A ? Team.B : Team.A, "The move destination contains a movable piece");
            return false;
        }

        // The player must own the designated card
        Card playedCard = null;
        if (team == Team.A && GameManager.instance.cardA1.cardName == turn.cardName)
            playedCard = GameManager.instance.cardA1;
        if (team == Team.A && GameManager.instance.cardA2.cardName == turn.cardName)
            playedCard = GameManager.instance.cardA2;
        if (team == Team.B && GameManager.instance.cardB1.cardName == turn.cardName)
            playedCard = GameManager.instance.cardB1;
        if (team == Team.B && GameManager.instance.cardB2.cardName == turn.cardName)
            playedCard = GameManager.instance.cardB2;
        if (playedCard == null)
        {
            GameManager.instance.EndGame(team == Team.A ? Team.B : Team.A, "The player does not own the right card");
            return false;
        }

        // The move must be allowed by the designated card
        Vector2Int moveVector = turn.destination - turn.source;
        if (moveVector.x < -2 || moveVector.x >= 3 || moveVector.y < -2 || moveVector.y >= 3)
        {
            GameManager.instance.EndGame(team == Team.A ? Team.B : Team.A, "This move exceeds 2 cells in height or width");
            return false;
        }
        if (team == Team.A && playedCard.GetMoves()[moveVector.x + 2][moveVector.y + 2] == 0)
        {
            GameManager.instance.EndGame(team == Team.A ? Team.B : Team.A, "The \"" + turn.cardName + "\" card does not allow this move");
            return false;
        }
        if (team == Team.B && playedCard.GetMovesReversed()[moveVector.x + 2][moveVector.y + 2] == 0)
        {
            GameManager.instance.EndGame(team == Team.A ? Team.B : Team.A, "The \"" + turn.cardName + "\" card does not allow this move");
            return false;
        }

        return true;
    }

    public void ApplyTurn(TurnResponse turn)
    {
        if (table[turn.destination.x][turn.destination.y] != null)
        {
            Destroy(table[turn.destination.x][turn.destination.y].gameObject);
            table[turn.destination.x][turn.destination.y] = null;
        }

        MovePiece(turn.source, turn.destination);
    }

    private void MovePiece(Vector2Int source, Vector2Int destination)
    {
        // Moving the pieces in the table
        table[destination.x][destination.y] = table[source.x][source.y];
        table[source.x][source.y] = null;

        // Physically moving the pieces
        if (table[source.x][source.y] != null)
            table[source.x][source.y].SetPosition(new Vector2Int(source.x, source.y));

        if (table[destination.x][destination.y] != null)
            table[destination.x][destination.y].SetPosition(new Vector2Int(destination.x, destination.y));
    }

    public Team HasGameEnded ()
    {
        // One of the players have lost their king
        bool kingAFound = false;
        bool kingBFound = false;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (table[i][j] != null && table[i][j].type == PieceType.king)
                {
                    if (table[i][j].team == Team.A)
                        kingAFound = true;
                    if (table[i][j].team == Team.B)
                        kingBFound = true;
                }
            }
        }
        if (!kingAFound)
        {
            GameManager.instance.EndGame(Team.B, "King A not found");
            return Team.B;
        }
        if (!kingBFound)
        {
            GameManager.instance.EndGame(Team.A, "King B not found");
            return Team.B;
        }

        // One of the player's pieces is on the other player's throne
        if (table[2][0] != null && table[2][0].team == Team.B)
        {
            GameManager.instance.EndGame(Team.B, "Throne A taken");
            return Team.B;
        }
        if (table[2][4] != null && table[2][4].team == Team.A)
        {
            GameManager.instance.EndGame(Team.A, "Throne B taken");
            return Team.B;
        }

        return Team.none;
    }
}
