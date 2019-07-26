using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGOPuppet : MonoBehaviour
{
    public Team team;

    public string cardName;
    public Vector2Int moveSource;
    public Vector2Int moveDestination;

    // Start is called before the first frame update
    void Start()
    {
        team = GameManager.instance.DeclareAI(new AIPuppet(this));
        name += " - " + team.ToString();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
