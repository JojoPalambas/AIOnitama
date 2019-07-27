using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGOJojoSimple : MonoBehaviour
{
    private Team team;

    // Start is called before the first frame update
    void Start()
    {
        team = GameManager.instance.DeclareAI(new AIJojoSimple());
        name += " - " + team.ToString();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
