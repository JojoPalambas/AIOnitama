using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    none,
    A,
    B
}

public enum GameStatus
{
    firstFrame,
    waiting,
    playing
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Card cardA1;
    public Card cardA2;
    public Card cardB1;
    public Card cardB2;
    public Card freeCard;

    public List<GameObject> cardPrefabs;

    public Board board;

    private AI AIA;
    private AI AIB;

    public GameObject AIPrefabA;
    public GameObject AIPrefabB;

    public GameObject playerIndicatorLeftPanel;
    public GameObject playerIndicatorRightPanel;

    public GameObject playerIndicatorAPanelPrefab;
    public GameObject playerIndicatorBPanelPrefab;
    public GameObject playerIndicatorHumanPanelPrefab;
    public GameObject playerIndicatorAIPanelPrefab;

    private GameStatus status;
    private Team currentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        status = GameStatus.firstFrame;

        InitCards();
        InitAIs();
    }

    // Update is called once per frame
    void Update()
    {
        if (status == GameStatus.firstFrame)
        {
            status = GameStatus.waiting;

            // 50% chance to switch the two AIs
            if (Random.Range(0, 2) >= 1)
            {
                AI tmp = AIA;
                AIA = AIB;
                AIB = tmp;
            }

            if (AIA != null)
                AIA.Init(Team.A);
            if (AIB != null)
                AIB.Init(Team.B);

            SetCurrentPlayer(Team.A);
        }
    }

    private void InitCards()
    {
        int[] chosenCardsIndexes = new int[5];
        for (int i = 0; i < 5; i++)
            chosenCardsIndexes[i] = -1;

        for (int i = 0; i < 5; i++)
        {
            int cardIndex = Random.Range(0, cardPrefabs.Count);
            chosenCardsIndexes[i] = cardIndex;
        }

        Vector3 pos = cardA1.transform.position;
        Destroy(cardA1.gameObject);
        cardA1 = Instantiate(cardPrefabs[chosenCardsIndexes[0]], pos, new Quaternion()).GetComponent<Card>();

        pos = cardA2.transform.position;
        Destroy(cardA2.gameObject);
        cardA2 = Instantiate(cardPrefabs[chosenCardsIndexes[1]], pos, new Quaternion()).GetComponent<Card>();

        pos = cardB1.transform.position;
        Destroy(cardB1.gameObject);
        cardB1 = Instantiate(cardPrefabs[chosenCardsIndexes[2]], pos, new Quaternion()).GetComponent<Card>();

        pos = cardB2.transform.position;
        Destroy(cardB2.gameObject);
        cardB2 = Instantiate(cardPrefabs[chosenCardsIndexes[3]], pos, new Quaternion()).GetComponent<Card>();

        pos = freeCard.transform.position;
        Destroy(freeCard.gameObject);
        freeCard = Instantiate(cardPrefabs[chosenCardsIndexes[4]], pos, new Quaternion()).GetComponent<Card>();
    }

    private void InitAIs()
    {
        if (AIPrefabA != null)
            Instantiate(AIPrefabA);
        if (AIPrefabB != null)
            Instantiate(AIPrefabB);
    }

    public void DeclareAI(AI ai)
    {
        if (AIA == null)
        {
            AIA = ai;
            return;
        }
        if (AIB == null)
        {
            AIB = ai;
            return;
        }
        Debug.LogError("Too much AIs to register!");
    }

    private void SetCurrentPlayer(Team player)
    {
        currentPlayer = player;

        if (player == Team.A)
        {
            Vector3 pos = playerIndicatorLeftPanel.transform.position;
            Destroy(playerIndicatorLeftPanel);
            playerIndicatorLeftPanel = Instantiate(playerIndicatorAPanelPrefab, pos, new Quaternion());

            if (AIA == null)
            {
                pos = playerIndicatorRightPanel.transform.position;
                Destroy(playerIndicatorRightPanel);
                playerIndicatorRightPanel = Instantiate(playerIndicatorHumanPanelPrefab, pos, new Quaternion());
            }
            else
            {
                pos = playerIndicatorRightPanel.transform.position;
                Destroy(playerIndicatorRightPanel);
                playerIndicatorRightPanel = Instantiate(playerIndicatorAIPanelPrefab, pos, new Quaternion());
            }
        }
        else
        {
            Vector3 pos = playerIndicatorLeftPanel.transform.position;
            Destroy(playerIndicatorLeftPanel);
            playerIndicatorLeftPanel = Instantiate(playerIndicatorBPanelPrefab, pos, new Quaternion());

            if (AIB == null)
            {
                pos = playerIndicatorRightPanel.transform.position;
                Destroy(playerIndicatorRightPanel);
                playerIndicatorRightPanel = Instantiate(playerIndicatorHumanPanelPrefab, pos, new Quaternion());
            }
            else
            {
                pos = playerIndicatorRightPanel.transform.position;
                Destroy(playerIndicatorRightPanel);
                playerIndicatorRightPanel = Instantiate(playerIndicatorAIPanelPrefab, pos, new Quaternion());
            }
        }
    }

    public void NextTurn()
    {
        Debug.Log("Next turn");

        if (currentPlayer == Team.A)
        {
            if (AIA != null)
            {
                TurnResponse turn = AIA.PlayTurn();
                if (!board.IsTurnValid(turn, Team.A))
                {
                    Debug.Log("Invalid turn!");
                }
                else
                {
                    board.ApplyTurn(turn);
                }
            }
            else
            {
                Debug.Log("Player A turn");
            }

            SetCurrentPlayer(Team.B);
        }
        else
        {
            if (AIB != null)
            {
                TurnResponse turn = AIB.PlayTurn();
                if (!board.IsTurnValid(turn, Team.B))
                {
                    Debug.Log("Invalid turn!");
                }
                else
                {
                    board.ApplyTurn(turn);
                }
            }
            else
            {
                Debug.Log("Player B turn");
            }

            SetCurrentPlayer(Team.A);
        }

        Team winner = board.HasGameEnded();
        if (winner != Team.none)
        {
            Debug.Log("Game won by player " + winner.ToString() + "!");
        }
    }
}
