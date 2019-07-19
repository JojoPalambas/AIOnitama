using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
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

    public GameObject cardA1Location;
    public GameObject cardA2Location;
    public GameObject cardB1Location;
    public GameObject cardB2Location;
    public GameObject freeCardLocation;

    public List<GameObject> cardPrefabs;

    public Board board;

    private AI AIA;
    private AI AIB;

    public GameObject AIPrefabA;
    public GameObject AIPrefabB;

    private GameStatus status;

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
            if (Random.Range(0, 1) > .5f)
            {
                AI tmp = AIA;
                AIA = AIB;
                AIB = tmp;
            }

            AIA.Init(Team.A);
            AIB.Init(Team.B);
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

        Vector3 pos = cardA1Location.transform.position;
        Destroy(cardA1Location);
        Instantiate(cardPrefabs[chosenCardsIndexes[0]], pos, new Quaternion());

        pos = cardA2Location.transform.position;
        Destroy(cardA2Location);
        Instantiate(cardPrefabs[chosenCardsIndexes[1]], pos, new Quaternion());

        pos = cardB1Location.transform.position;
        Destroy(cardB1Location);
        Instantiate(cardPrefabs[chosenCardsIndexes[2]], pos, new Quaternion());

        pos = cardB2Location.transform.position;
        Destroy(cardB2Location);
        Instantiate(cardPrefabs[chosenCardsIndexes[3]], pos, new Quaternion());

        pos = freeCardLocation.transform.position;
        Destroy(freeCardLocation);
        Instantiate(cardPrefabs[chosenCardsIndexes[4]], pos, new Quaternion());
    }

    private void InitAIs()
    {
        Instantiate(AIPrefabA);
        Instantiate(AIPrefabB);
    }

    public void DeclareAI(AI ai)
    {
        if (AIA == null)
        {
            Debug.Log("AIA registered");
            AIA = ai;
            return;
        }
        if (AIB == null)
        {
            Debug.Log("AIA registered");
            AIB = ai;
            return;
        }
        Debug.LogError("Too much AIs to register!");
    }
}
