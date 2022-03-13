using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DialogueEntity : MonoBehaviour
{
    public List<string> speech = new List<String>();
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.tag = "CanTalkTo";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
