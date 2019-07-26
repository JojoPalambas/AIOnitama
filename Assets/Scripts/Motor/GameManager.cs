using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Debug = UnityEngine.Debug;

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
    playing,
    end
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

    private Team winner;

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
        if (Input.GetKeyDown("escape"))
            Application.Quit();

        if (status == GameStatus.end)
            return;

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

    public Team DeclareAI(AI ai)
    {
        // If it is the first AI to be declared, its team is randomized
        if (AIA == null && AIB == null)
        {
            if (Random.Range(0, 2) >= 1)
            {
                AIA = ai;
                return Team.A;
            }
            else
            {
                AIB = ai;
                return Team.B;
            }
        }
        else
        {
            if (AIA == null)
            {
                AIA = ai;
                return Team.A;
            }
            if (AIB == null)
            {
                AIB = ai;
                return Team.B;
            }
        }

        Debug.LogError("Too much AIs to register!");
        return Team.none;
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
        if (status == GameStatus.end)
            return;

        Debug.Log("Next turn");

        if (currentPlayer == Team.A)
        {
            if (AIA != null)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                TurnResponse turn = AIA.PlayTurn();

                stopwatch.Stop();
                if (stopwatch.ElapsedMilliseconds > GameConstants.maxTurnTimeMillis)
                {
                    EndGame(Team.B, "Too much thinking time: " + stopwatch.ElapsedMilliseconds.ToString() + " milliseconds");
                    return;
                }

                if (!board.IsTurnValid(turn, Team.A))
                    return;
                else
                {
                    board.ApplyTurn(turn);
                    UseCard(turn.cardName, Team.A);
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
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                TurnResponse turn = AIA.PlayTurn();

                stopwatch.Stop();
                if (stopwatch.ElapsedMilliseconds > GameConstants.maxTurnTimeMillis)
                {
                    EndGame(Team.A, "Too much thinking time: " + stopwatch.ElapsedMilliseconds.ToString() + " milliseconds");
                    return;
                }

                if (!board.IsTurnValid(turn, Team.B))
                    return;
                else
                {
                    board.ApplyTurn(turn);
                    UseCard(turn.cardName, Team.B);
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
            return;
    }

    private void UseCard(string cardName, Team team)
    {
        if (team == Team.A)
        {
            if (cardA1.cardName == cardName)
            {
                Card tmpCard = freeCard;
                freeCard = cardA1;
                cardA1 = tmpCard;

                Vector3 tmpPos = freeCard.transform.position;
                freeCard.transform.position = cardA1.transform.position;
                cardA1.transform.position = tmpPos;

                return;
            }
            if (cardA2.cardName == cardName)
            {
                Card tmpCard = freeCard;
                freeCard = cardA2;
                cardA2 = tmpCard;

                Vector3 tmpPos = freeCard.transform.position;
                freeCard.transform.position = cardA2.transform.position;
                cardA2.transform.position = tmpPos;

                return;
            }
        }
        if (team == Team.B)
        {
            if (cardB1.cardName == cardName)
            {
                Card tmpCard = freeCard;
                freeCard = cardB1;
                cardB1 = tmpCard;

                Vector3 tmpPos = freeCard.transform.position;
                freeCard.transform.position = cardB1.transform.position;
                cardB1.transform.position = tmpPos;

                return;
            }
            if (cardB2.cardName == cardName)
            {
                Card tmpCard = freeCard;
                freeCard = cardB2;
                cardB2 = tmpCard;

                Vector3 tmpPos = freeCard.transform.position;
                freeCard.transform.position = cardB2.transform.position;
                cardB2.transform.position = tmpPos;

                return;
            }
        }
        Debug.LogError("This line should not be read");
    }

    public void EndGame(Team winner, string reason)
    {
        if (status == GameStatus.end)
            return;

        Debug.Log("Game won by team " + winner.ToString() + " - " + reason);
        this.winner = winner;
        this.status = GameStatus.end;
    }
}
