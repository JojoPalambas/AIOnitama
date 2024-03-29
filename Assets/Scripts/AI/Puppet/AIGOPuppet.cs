﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGOPuppet : MonoBehaviour
{
    private Team team;

    public string cardName;
    public Vector2Int moveSource;
    public Vector2Int moveDestination;

    // Start is called before the first frame update
    void Start()
    {
        team = GameManager.instance.DeclareAI(new AIPuppet(this), gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
