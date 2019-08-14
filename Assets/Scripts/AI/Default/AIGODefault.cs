using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGODefault : MonoBehaviour
{
    private Team team;

    // Start is called before the first frame update
    void Start()
    {
        team = GameManager.instance.DeclareAI(new AIDefault(), gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
