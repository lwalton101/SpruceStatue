using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Values")]
    [SerializeField] private int coins;

    [Header("UI Assignables")]
    [SerializeField] private TextMeshProUGUI coinCounterText;
    // Start is called before the first frame update
    void Awake()
    {
        coins = 0;
    }

    // Update is called once per frame
    void Update()
    {
        coinCounterText.text = coins.ToString();
    }

    public void AddCoinAmount(int amount)
	{
        coins = coins + amount;
	}
}
