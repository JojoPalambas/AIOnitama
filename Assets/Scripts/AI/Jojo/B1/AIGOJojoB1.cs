using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGOJojoB1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.DeclareAI(new AIJojoB1(), gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
