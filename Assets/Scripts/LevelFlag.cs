using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelFlag : MonoBehaviour
{
    [SerializeField] private SceneID levelScene;

    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Collider2D coll;
    // Start is called before the first frame update
    void Start()
    {
        if (coll == null)
        {
            coll = GetComponent<Collider2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("LocalPlayer"))
        {
            infoPanel.SetActive(true);
        }
    }   
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("LocalPlayer"))
        {
            infoPanel.SetActive(false);
        }
    }
}
