using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindingsManager : MonoBehaviour
{
    public static KeyBindingsManager Instance;

    public KeyCode jump;
    public KeyCode left;
    public KeyCode right;
    public KeyCode talk;
    public KeyCode shoot;
    // Start is called before the first frame update
    void Awake()
    {
        //Load keybindings
        jump = KeyCode.Space;
        left = KeyCode.D;
        right = KeyCode.A;
        talk = KeyCode.C;
        shoot = KeyCode.X;

        DontDestroyOnLoad(this);

        if (Instance == null)
		{
            Instance = this;
		}
		else
		{
            Debug.LogError("THERE SHALL ONLY BE ONE " + typeof(KeyBindingsManager));
            Destroy(this);
		}
    }

    // Update is called once per frame
    void Update()
    { 

    }
}
