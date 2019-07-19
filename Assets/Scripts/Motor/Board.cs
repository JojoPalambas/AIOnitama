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
            Destroy(gameObject);
        instance = this;

        InitTable();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void InitTable()
    {
        Piece newPiece = null;
        for (int i = 0; i < 5; i++)
        {
            if (i == 2)
            {
                newPiece = Instantiate(AKPrefab, transform).GetComponent<Piece>();
                newPiece.SetPosition(new Vector2Int(i, 0));
                newPiece = Instantiate(BKPrefab, transform).GetComponent<Piece>();
                newPiece.SetPosition(new Vector2Int(i, 4));
            }
            else
            {
                newPiece = Instantiate(AMPrefab, transform).GetComponent<Piece>();
                newPiece.SetPosition(new Vector2Int(i, 0));
                newPiece = Instantiate(BMPrefab, transform).GetComponent<Piece>();
                newPiece.SetPosition(new Vector2Int(i, 4));
            }
        }
    }
}
