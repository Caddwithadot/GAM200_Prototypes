using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCanvas : MonoBehaviour
{
    public GameObject CoinCounter;
    public float CoinScore = 0f;

    public void Update()
    {
        CoinCounter.GetComponent<TextMeshProUGUI>().SetText("x" + CoinScore.ToString());
    }
}
