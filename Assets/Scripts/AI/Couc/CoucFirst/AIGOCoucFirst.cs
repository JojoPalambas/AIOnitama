using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGOCoucFirst : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.DeclareAI(new AICoucFirst(), gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
