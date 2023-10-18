using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCanvas : MonoBehaviour
{
    public TextMeshProUGUI CoinCounter;
    public int CoinScore = 0;

    public void Start()
    {
        CoinCounter = transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void Update()
    {
        CoinCounter.SetText("x" + CoinScore.ToString());
    }

    public void AddScore(int Score)
    {
        CoinScore += Score;
    }
}
