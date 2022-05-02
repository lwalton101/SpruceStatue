using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelFlag : MonoBehaviour
{
    [SerializeField] private SceneID levelScene;

    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Collider2D coll;
    [SerializeField] private bool panelOpen = false;
    
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
        if (panelOpen && Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.Singleton.StartLevel(SceneID.Level01);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("LocalPlayer"))
        {
            panelOpen = true;
            infoPanel.SetActive(true);
        }
    }   
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("LocalPlayer"))
        {
            panelOpen = false;
            infoPanel.SetActive(false);
        }
    }
}
