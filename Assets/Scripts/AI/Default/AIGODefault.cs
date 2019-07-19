using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGODefault : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.DeclareAI(new AIDefault());
    }

    // Update is called once per frame
    void Update()
    {
    }
}
