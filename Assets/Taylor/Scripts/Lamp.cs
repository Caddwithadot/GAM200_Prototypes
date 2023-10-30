using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    private MouseControls mouseControls;
    public GameObject lampAura;

    private void Start()
    {
        mouseControls = GameObject.Find("MouseControls").GetComponent<MouseControls>();
    }

    public void TurnOn()
    {
        GetComponent<SpriteRenderer>().color = Color.yellow;
        lampAura.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ChargeLight" && mouseControls.kill)
        {
            TurnOn();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "ChargeLight" && mouseControls.kill)
        {
            TurnOn();
        }
    }
}
