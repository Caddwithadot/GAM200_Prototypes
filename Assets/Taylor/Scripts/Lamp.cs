using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    public GameObject lampAura;

    public void TurnOn()
    {
        GetComponent<SpriteRenderer>().color = Color.yellow;
        lampAura.SetActive(true);
    }
}
