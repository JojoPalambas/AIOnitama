using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType
{
    none,
    king,
    monk
}

public class Piece : MonoBehaviour
{
    public PieceType type;
    public Team team;
    public Vector2Int position;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetPosition(Vector2Int position)
    {
        this.position = position;

        transform.position = new Vector3(Board.instance.transform.position.x - 2 + this.position.x, Board.instance.transform.position.y - 2 + this.position.y, transform.position.z);
    }
}
